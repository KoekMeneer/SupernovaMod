﻿using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Projectiles.Ranged.Bullets
{
    public class PearlsandBullet : SandBullet
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pearlsand Ball");
        }

        public override void AI()
        {
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), (int)(Projectile.width * .1f), (int)(Projectile.height * .1f), DustID.Pearlsand, Projectile.velocity.X * .1f, Projectile.velocity.Y * .1f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            int radius = 5;     //this is the explosion radius, the highter is the value the bigger is the explosion

            for (int x = -radius; x <= radius; x++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Pearlsand, Projectile.velocity.X * -0.6f, Projectile.velocity.Y * 1.2f, 80, default, 0.75f);
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.8f;
            }
        }
    }
}
