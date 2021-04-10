using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Projectiles
{
    public class DriveSpinnerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drive Spinner YoYo");
        }
        int Timer;
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 99; // All Yoyos use the style 99.
            projectile.friendly = true; // Doesn't harm NPCs
            projectile.penetrate = -1; // -1 = Will not go through enemy.
            projectile.melee = true; // The projectile is a melee projectile.
            projectile.scale = 1f; // The scale of the projectile. 2f is double size. 0.5f is half size.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 18;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 320f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 21f;
        }
        public override void AI()
        {
            Timer++;
            if (Main.rand.Next(2) == 0)
			{
                int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width + 2, projectile.height + 2, DustID.Electric, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 80, default(Color), 0.7f);
                Main.dust[DustID2].noGravity = true;
            }
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                //If the npc is hostile
                if (!target.friendly)
                {
                    //Get the shoot trajectory from the projectile and target
                    float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                    float shootToY = target.position.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    //If the distance between the live targeted npc and the projectile is less than 480 pixels
                    if (distance < 310 && !target.friendly && target.active)
                    {
                        //Divide the factor, 3f, which is the desired velocity
                        distance = 3f / distance;

                        //Multiply the distance by a multiplier if you wish the projectile to have go faster
                        shootToX *= distance * 5;
                        shootToY *= distance * 5;
                        if (Timer == 1)
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, 255, (int)(projectile.damage * .75f), projectile.knockBack, Main.myPlayer, 0f, 0f);
                    }
                }
            }

            // Reset timer
            if (Timer > 30)
                Timer = 0;
        }
    }
}
