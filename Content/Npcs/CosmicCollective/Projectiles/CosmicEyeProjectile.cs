using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SupernovaMod.Api.Helpers;
using SupernovaMod.Core.Helpers;

namespace SupernovaMod.Content.Npcs.CosmicCollective.Projectiles
{
    public class CosmicEyeProjectile : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NebulaEye}";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.ai = new float[] { Projectile.ai[0], Projectile.ai[1], Projectile.ai[2], 0 };
        }

        public override void AI()
        {
            Projectile.ai[3]++;
            // After ~1.5 seconds we fire at the player
            // 
            if (Projectile.ai[3] >= 180)
            {
                NPC owner = Main.npc[(int)Projectile.ai[1]];
                Player target = Main.player[owner.target];
                const int type = ProjectileID.EyeLaser;
                const int damage = 32;

                Vector2 targetCenter = target.Center;
                Vector2 velocity = Mathf.VelocityFPTP(Projectile.Center, targetCenter, 12);
                targetCenter = ProjectileHelper.CalculateBasicTargetPrediction(Projectile.Center, targetCenter, velocity);
                velocity = Mathf.VelocityFPTP(Projectile.Center, targetCenter, 12).RotatedByRandom(.01f);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, type, Projectile.damage, 0f, 0);

                for (int x = 0; x < 5; x++)
                {
                    int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.UndergroundHallowedEnemies, velocity.X, velocity.Y, 80, default, Main.rand.NextFloat(.9f, 1.6f));
                    Main.dust[dust].noGravity = true;
                }

                //
                Projectile.timeLeft = 0;
            }
            else
            {
                AI_Orbit();
            }
        }

        private void AI_Orbit()
        {
            //
            if (Projectile.ai[2] == 0)
            {
                Projectile.ai[2] = 1;
            }

            NPC owner = Main.npc[(int)Projectile.ai[1]];

            // Kill the projectile when the owner not active
            //
            if (owner == null || !owner.active)
            {
                Projectile.timeLeft = 0;
                return;
            }
            //Factors for calculations  
            double deg = (double)Projectile.ai[0];  //The degrees, you can multiply projectile.ai[0] to make it orbit faster, may be choppy depending on the value  
            double rad = deg * (Math.PI / 180);     //Convert degrees to radians  
            double dist = 120;                      //Distance away from the owner  

            /*Position the owner based on where the owner is, the Sin/Cos of the angle times the /  
            /distance for the desired distance away from the owner minus the projectile's width   /  
            /and height divided by two so the center of the projectile is at the right place.     */
            Projectile.position.X = owner.Center.X - ((int)(Math.Cos(rad) * dist) - Projectile.width / 2) * Projectile.ai[2];
            Projectile.position.Y = (owner.Center.Y - 40) - ((int)(Math.Sin(rad) * dist) - Projectile.height / 2) * Projectile.ai[2];

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value  
            float increase = 2;
            Projectile.ai[0] += increase;
        }
    }
}
