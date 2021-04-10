using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class RagnaBowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ragna Bow");

        }
        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.ranged = false;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.velocity = Microsoft.Xna.Framework.Vector2.Zero;
            projectile.tileCollide = false;
            projectile.alpha = 155;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors
            //rotation
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
            //this is projectile dust
            //int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 10), projectile.width + 2, projectile.height + 2, mod.DustType("Blood"), projectile.velocity.X * .2f, projectile.velocity.Y * .2f, 2, default(Color), 0.7f);
            //Main.dust[DustID2].noGravity = true;
            #region shooting
            projectile.ai[0]++;
            if (projectile.ai[0] > 20) //Assuming you are already incrementing this in AI outside of for loop
            {

                //Shoot projectile and set ai back to 0
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 11); //Bullet noise
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X * 100, projectile.velocity.Y * 100, mod.ProjectileType("RagnaArrow"), (int)(projectile.damage * 0.7), 2.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
                projectile.Kill();
            }

            #endregion
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 12; k++)
            {
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Shadowflame, projectile.oldVelocity.X * 0.8f, projectile.oldVelocity.Y * 0.8f);
            }
        }
    }
}
