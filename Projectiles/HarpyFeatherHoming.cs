using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Supernova.Projectiles
{
    public class HarpyFeatherHoming : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harpy Feather");
        }
        int Timer;
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 14;
            projectile.aiStyle = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.timeLeft = 300;  //The amount of time the projectile is alive for
        }

        public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.modProjectile != null)
                return projectile.modProjectile.OnTileCollide(oldVelocity);
            return true;
        }

        public override void AI()
        {
            if(Timer >= 24)
            {
                NPC target;
                float targetDist = 1000;
                for (int i = 0; i < 200; i++)
                {
                    target = Main.npc[i];
                    //If the npc is hostile
                    if (!target.friendly)
                    {
                        //Get the shoot trajectory from the projectile and target
                        float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                        float shootToY = target.position.Y - projectile.Center.Y;
                        float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                        //If the distance between the live targeted npc and the projectile is less than 480 pixels
                        if (distance < 480 && !target.friendly && target.active && distance < targetDist)
                        {
                            targetDist = distance;

                            //Divide the factor, 3f, which is the desired velocity
                            distance = 3f / distance;

                            //Multiply the distance by a multiplier if you wish the projectile to have go faster
                            shootToX *= distance * 2.5f;
                            shootToY *= distance * 2.5f;

                            //Set the velocities to the shoot values
                            projectile.velocity.X = shootToX;
                            projectile.velocity.Y = shootToY;
                        }
                    }
                }
            }
            else
                Timer++;

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 9.57f;
        }
    }
}
