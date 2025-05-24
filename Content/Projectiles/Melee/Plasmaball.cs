using Microsoft.Xna.Framework;
using SupernovaMod.Api.Effects;
using SupernovaMod.Api;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SupernovaMod.Content.Projectiles.Melee
{
    public class Plasmaball : ModProjectile
    {
        const float MAX_SCALE = 3;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 90;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            // Decrease velocity
            //
            Projectile.velocity *= .9875f;

            // Collision loop
            //
            //foreach (var proj in Main.projectile)
            //{
            //    // Ignore other types first
            //    //
            //    if (proj.type != Type)
            //    {
            //        continue;
            //    }

            //    // Ignore self
            //    //
            //    if (Projectile.whoAmI == proj.whoAmI)
            //    {
            //        continue;
            //    }

            //    // Check if the projectile has collided with this projectile
            //    //
            //    if (Projectile.Colliding(Projectile.getRect(), proj.getRect()))
            //    {
            //        Main.NewText("!!!COLLISION!!!");
            //        OnHitProjectile(proj);
            //        break; // Stop looping
            //    }
            //}

            // Dust effects
            //
            int actualWidth = (int)(Projectile.width * Projectile.scale);
            int actualHeight = (int)(Projectile.height * Projectile.scale);
            int num3;
            int num240 = (int)Projectile.ai[0];
            for (int num241 = 0; num241 < 3; num241 = num3 + 1)
            {
                int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), actualWidth, actualHeight, DustID.Electric, Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 1.2f);
                Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center) / 2f;
                Main.dust[num242].noGravity = true;
                Dust dust2 = Main.dust[num242];
                dust2.velocity *= 0.5f;
                num3 = num241;
            }
            for (int num243 = 0; num243 < 2; num243 = num3 + 1)
            {
                int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Vortex, Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 0.4f);
                if (num243 == 0)
                {
                    Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center * 5f) / 6f;
                }
                else if (num243 == 1)
                {
                    Main.dust[num242].position = (Main.dust[num242].position + (Projectile.Center + Projectile.velocity / 2f) * 5f) / 6f;
                }
                Dust dust2 = Main.dust[num242];
                dust2.velocity *= 0.1f;
                Main.dust[num242].noGravity = true;
                Main.dust[num242].fadeIn = 1f;
                num3 = num243;
            }
        }

        public void OnHitProjectile(Projectile proj)
        {
            float newScale = Projectile.scale + (proj.scale / 2);
            if (newScale <= MAX_SCALE)
            {
                proj.Kill();
                Projectile.scale = newScale; // Rescale to the combined scale
                Projectile.penetrate = (int)Math.Round(newScale); // Reset to avoid despawning after collision
                Projectile.damage += (int)(proj.damage / 2);
                Projectile.timeLeft = 320; // Extend the lifetime
                Projectile.velocity = Projectile.velocity * 1.5f; // Decrease the velocity a bit after collision
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false; // return false because we are handling collision
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool())
            {
                target.AddBuff(BuffID.Electrified, 180);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool())
            {
                target.AddBuff(BuffID.Electrified, 180);
            }
        }

        public override void OnKill(int timeLeft)
        {
            DrawDust.MakeExplosion(Projectile.Center, 4.5f, DustID.Electric, 17, 4f, 10f, 70, 120, .75f, 1.1f, true);
            Projectile.CreateExplosion(64, 64, killProjectile: false);

            SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
        }
    }
}
