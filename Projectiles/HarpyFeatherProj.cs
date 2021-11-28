using System;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace Supernova.Projectiles
{
    public class HarpyFeatherProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.width = 24;
            projectile.height = 24;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.timeLeft = 55;
            aiType = 1;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - 4.57f;
        }

		public override void Kill(int timeLeft)
		{
            if (projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 4; i++)
				{
                    Vector2 velocity = Calc.RandomSpread(projectile.velocity.X, projectile.velocity.Y, 2.5f) * 1.2f;
                    Projectile.NewProjectile(projectile.position.X, projectile.position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<HarpyFeatherHoming>(), projectile.damage, 1f, projectile.owner);
                }
            }
			base.Kill(timeLeft);
		}
	}
}
