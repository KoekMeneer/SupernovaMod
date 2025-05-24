using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Core.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class Kusanagi_Proj : ModProjectile
    {
        public override string Texture => "SupernovaMod/Content/Items/Weapons/Melee/Kusanagi";

        private float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public float[] oldrot = new float[7];
        private Vector2 startVector;
        private Vector2 vector;
        public ref float Length => ref Projectile.localAI[0];
        public ref float Rot => ref Projectile.localAI[1];
        private float speed;
        private Vector2 mouseOrig;
        protected float glow;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override bool ShouldUpdatePosition() => false;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;

            Projectile.width = 78;
            Projectile.height = 86;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.scale = 1.1f;

            Rot = MathHelper.ToRadians(2);
            Length = 70;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return !target.friendly && Timer < 15 && Projectile.ai[0] != 2 ? null : false;
        }

        public float SetSwingSpeed(float speed)
        {
            Player owner = Main.player[Projectile.owner];
            return speed / owner.GetAttackSpeed(DamageClass.Melee);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.noItems || player.CCed || player.dead || !player.active)
            {
                Projectile.Kill();
            }

            float swingSpeed = SetSwingSpeed(2);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            Projectile.spriteDirection = player.direction;

            if (Projectile.spriteDirection == 1)
            {
                Projectile.rotation = (Projectile.Center - player.Center).ToRotation() + MathHelper.PiOver4;
            }
            else
            {
                Projectile.rotation = (Projectile.Center - player.Center).ToRotation() - MathHelper.Pi - MathHelper.PiOver4;
            }
            //glow += 0.03f;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2);
            if (Main.myPlayer == Projectile.owner)
            {
                Timer++;
                if (Timer == 1)
                {
                    mouseOrig = Main.MouseWorld;
                    speed = MathHelper.ToRadians(1);
                    startVector = MathUtils.NewPolarVector(1, Projectile.velocity.ToRotation() - ((MathHelper.PiOver2 + 0.6f) * Projectile.spriteDirection));
                    vector = startVector * Length;
                    SoundEngine.PlaySound(SoundID.Item71, player.position);
                }
                //if (Timer == (int)(4 * swingSpeed))
                //{
                //    Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center,
                //        MathUtils.NewPolarVector(14, (mouseOrig - player.Center).ToRotation()),
                //        ModContent.ProjectileType<Kusanagi_Proj>(), (int)(Projectile.damage * .75f), Projectile.knockBack / 2, Projectile.owner);
                //}
                if (Timer < 6 * swingSpeed)
                {
                    Rot += speed / swingSpeed * Projectile.spriteDirection;
                    speed += 0.15f;
                    vector = startVector.RotatedBy(Rot) * Length;
                }
                else
                {
                    Rot += speed / swingSpeed * Projectile.spriteDirection;
                    speed *= 0.7f;
                    vector = startVector.RotatedBy(Rot) * Length;
                }
                if (Timer >= 15 * swingSpeed)
                {
                    if (!player.channel)
                    {
                        Projectile.Kill();
                        return;
                    }
                    if (Main.MouseWorld.X < player.Center.X)
                    {
                        player.direction = -1;
                    }
                    else
                    {
                        player.direction = 1;
                    }
                    Projectile.velocity = MathUtils.NewPolarVector(5, (Main.MouseWorld - player.Center).ToRotation());
                    Projectile.alpha = 255;
                    speed = MathHelper.ToRadians(1);
                    Rot = MathHelper.ToRadians(2);
                    startVector = MathUtils.NewPolarVector(1, (Main.MouseWorld - player.Center).ToRotation() + ((MathHelper.PiOver2 + 0.6f) * player.direction));
                    vector = startVector * Length;
                    mouseOrig = Main.MouseWorld;
                    Projectile.ai[0]++;
                    SoundEngine.PlaySound(SoundID.Item71, Projectile.position);
                    glow = 0;
                    Timer = 0;
                    Projectile.netUpdate = true;
                }

                if (Timer > 1)
                {
                    Projectile.alpha = 0;
                }

                Projectile.Center = player.MountedCenter + vector;
                for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
                {
                    oldrot[k] = oldrot[k - 1];
                }
                oldrot[0] = Projectile.rotation;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Only hit each target once every swing
            Projectile.localNPCImmunity[target.whoAmI] = 8;
            target.immune[Projectile.owner] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new(texture.Width / 2f, texture.Height / 2f);
            Vector2 trialOrigin = new(texture.Width / 2f - 8, Projectile.height / 2f);

            int shader = ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex;
            Vector2 v = MathUtils.NewPolarVector(20, (Projectile.Center - player.Center).ToRotation());

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            //GameShaders.Armor.ApplySecondary(shader, Main.player[Main.myPlayer], null);

            //for (int k = 0; k < Projectile.oldPos.Length; k++)
            //{
            //    Vector2 drawPos = Projectile.oldPos[k] - v - Main.screenPosition + trialOrigin + new Vector2(0f, Projectile.gfxOffY);
            //    Color color = Color.LimeGreen * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
            //    Main.EntitySpriteDraw(texture, drawPos, null, color * Projectile.Opacity * glow, oldrot[k], origin, Projectile.scale, spriteEffects, 0);
            //}

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - v - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}
