using System;
using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class ZirconProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zircon Shot");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;                       //this is the projectile penetration
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;                 //this make that the projectile does not go thru walls
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 72;
        }

        public override void AI()
        {
            // Main dust
            int dustAmount = 3;
            for (int i = 0; i < dustAmount; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.ZirconDust>(), Projectile.velocity.X, Projectile.velocity.Y);
                Dust dust = Main.dust[dustIndex];

                // Position dust slightly off from the projectile center
                dust.position = (dust.position + Projectile.Center) / 2f;
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            // Secondary dust
            dustAmount = 2;
            for (int i = 0; i < dustAmount; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.ZirconDust>(), Projectile.velocity.X, Projectile.velocity.Y);
                Dust dust = Main.dust[dustIndex];

                // Adjust positions based on iteration to create a trailing effect
                if (i == 0)
                {
                    dust.position = (dust.position + Projectile.Center * 5f) / 6f;
                }
                else if (i == 1)
                {
                    dust.position = (dust.position + (Projectile.Center + Projectile.velocity / 2f) * 5f) / 6f;
                }

                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.velocity *= 0.1f;
            }

            // Ensure the projectile faces the direction it's moving
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
        }

		public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ZicroniumExplosion>(), (int)(Projectile.damage * .6f), Projectile.knockBack, Projectile.owner, 90);
            SoundEngine.PlaySound(SoundID.Item14);
		}
    }
}

