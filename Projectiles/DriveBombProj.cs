using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class DriveBombProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drive Bomb");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 1;                       //this is the projectile penetration
            projectile.hostile = false;
            projectile.thrown = true;                        //this make the projectile do magic damage
            projectile.tileCollide = true;                 //this make that the projectile does not go thru walls
            projectile.ignoreWater = true;
        }
        private const float maxTicks = 45;
        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width + 2, projectile.height + 2, DustID.Electric, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 80, default(Color), 0.9f);
            Main.dust[DustID2].noGravity = true;
            //this make that the projectile faces the right way
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            projectile.alpha = (int)projectile.localAI[0] * 2;

            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }

            projectile.ai[1] += 1f;
            if (projectile.ai[1] >= maxTicks)
            {
                float velXmult = 0.98f;
                float velYmult = 0.35f;
                projectile.ai[1] = maxTicks;
                projectile.velocity.X = projectile.velocity.X * velXmult;
                projectile.velocity.Y = projectile.velocity.Y + velYmult;
            }

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Electrified, 180);
        }
        public override void Kill(int timeLeft)
        {
            // Play explosion sound
            Main.PlaySound(SoundID.Item15, projectile.position);
            // If we are the original projectile, spawn the 5 child projectiles
            if (projectile.ai[0] == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    // Random upward vector.
                    Vector2 vel = new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-10, -8));
                    Projectile.NewProjectile(projectile.Center, vel, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 1);
                }
                Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.position);


                // Smoke Dust spawn
                for (int i = 0; i < 40; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 60; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 3f; //5
                    dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.5f;//3
                }
                // reset size to normal width and height.
                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.width = 10;
                projectile.height = 10;
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

                // TODO, tmodloader helper method
                {
                    int explosionRadius = 12;
                    int minTileX = (int)(projectile.position.X / 16f - (float)explosionRadius);
                    int maxTileX = (int)(projectile.position.X / 16f + (float)explosionRadius);
                    int minTileY = (int)(projectile.position.Y / 16f - (float)explosionRadius);
                    int maxTileY = (int)(projectile.position.Y / 16f + (float)explosionRadius);
                    if (minTileX < 0)
                    {
                        minTileX = 0;
                    }
                    if (maxTileX > Main.maxTilesX)
                    {
                        maxTileX = Main.maxTilesX;
                    }
                    if (minTileY < 0)
                    {
                        minTileY = 0;
                    }
                    if (maxTileY > Main.maxTilesY)
                    {
                        maxTileY = Main.maxTilesY;
                    }
                }
            }
        }
    }
}
