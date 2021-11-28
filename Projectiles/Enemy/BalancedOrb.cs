using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Supernova.Projectiles.Enemy
{
    public class BalancedOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Balanced Orb");
        }
        NPC owner;
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 720;
        }

        public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.modProjectile != null)
                return projectile.modProjectile.OnTileCollide(oldVelocity);
            return true;
        }
        bool isOrbit = true;

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0, (255 - projectile.alpha) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);

            owner = Main.npc[(int)projectile.ai[1]];
            if (isOrbit)
            {
                //Factors for calculations 
                double deg = (double)projectile.ai[0];  //The degrees, you can multiply projectile.ai[0] to make it orbit faster, may be choppy depending on the value 
                double rad = deg * (Math.PI / 180);     //Convert degrees to radians 
                double dist = 120;                      //Distance away from the owner 

                /*Position the owner based on where the owner is, the Sin/Cos of the angle times the / 
                /distance for the desired distance away from the owner minus the projectile's width   / 
                /and height divided by two so the center of the projectile is at the right place.     */
                projectile.position.X = owner.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
                projectile.position.Y = owner.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2;

                //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value 
                projectile.ai[0] += 1f;
            }

            // After 2 seconds we fire at the player 
            if (projectile.ai[0] >= 360 && isOrbit)
            {
                Main.PlaySound(SoundID.Item105);
                Player target = Main.player[owner.target];
                // Get the shooting trajectory 
                float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                float shootToY = target.position.Y - projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                // Dividing the factor of 3f which is the desired velocity by distance 
                distance = 3f / distance;

                // Multiplying the shoot trajectory with distance times a multiplier if you so choose to 
                shootToX *= distance * 2;
                shootToY *= distance * 2;

                projectile.velocity = new Vector2(shootToX, shootToY);
                isOrbit = false;
            }
            CheckActive();
        }
        public override void Kill(int timeLeft)
        {
            // Tell the owner we are killed 
            Main.npc[(int)projectile.ai[1]].ai[0]--;
        }
        public void CheckActive()
        {
            //if (!Main.npc[(int)projectile.ai[1]].active) 
            //projectile.timeLeft = 2; 
        }
    }
}