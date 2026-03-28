using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Typeless
{
    public class AlgizShieldProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_950";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 60 * 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.Supernova().algizShieldHits <= 0)
            {
                Projectile.timeLeft = 0;
            }

            // Stick to player
            Projectile.Center = player.Center;

            // Flicker when about to expire
            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha = (Main.GameUpdateCount % 20 < 10) ? 200 : 0;
            }

            // Pulse effect
            float pulse = (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.1f;
            Projectile.scale = 1f + pulse;
        }

        private void BreakEffect(Player player)
        {
            Vector2 center = player.Center;

            for (int i = 0; i < 25; i++)
            {
                float angle = MathHelper.ToRadians(Main.rand.NextFloat(360));
                Vector2 dir = angle.ToRotationVector2();
                Dust d = Dust.NewDustPerfect(center, DustID.WhiteTorch, dir * Main.rand.NextFloat(2f, 5f));
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1, 2);
            }

            SoundEngine.PlaySound(SoundID.Item29, center);
        }

        public override void OnKill(int timeLeft)
        {
            BreakEffect(Main.player[Projectile.owner]);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Terraria/Images/Projectile_950").Value;

            Vector2 pos = Projectile.Center - Main.screenPosition;

            //
            Color color = new Color(
                255,
                255,
                255,
                Projectile.alpha
            );

            // Glow
            Main.EntitySpriteDraw(
                texture,
                pos,
                null,
                color,
                0f,
                texture.Size() / 2f,
                Projectile.scale * .8f,
                SpriteEffects.None,
                0
            );

            return false; // prevent default drawing
        }
    }
    //public class AlgizSparkProj : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.width = 6;
    //        Projectile.height = 6;
    //        Projectile.friendly = true;
    //        Projectile.hostile = false;
    //        Projectile.tileCollide = false;
    //        Projectile.timeLeft = 30;
    //        Projectile.alpha = 0;
    //    }

    //    public override void AI()
    //    {
    //        // simple fade out and slight rotation
    //        Projectile.rotation += 0.3f;
    //        Projectile.velocity *= 0.95f;
    //        Projectile.alpha += 8;
    //    }

    //    public override bool PreDraw(ref Color lightColor)
    //    {
    //        Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
    //        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition,
    //            null, Color.White * (1f - Projectile.alpha / 255f),
    //            Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
    //        return false;
    //    }
    //}
}
