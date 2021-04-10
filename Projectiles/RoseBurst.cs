using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class RoseBurst : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rose Burst");
            Main.projFrames[projectile.type] = 3;

        }
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.ranged = false;
            projectile.magic = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 500;
            projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 10), projectile.width + 2, projectile.height + 2, mod.DustType("BloodDust"), projectile.velocity.X * .25f, projectile.velocity.Y * .25f, 2, default(Color), 0.5f);
            Main.dust[DustID2].noGravity = true;

            //tickes btween frame
            if (++projectile.frameCounter >= 25)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 3)
                    projectile.frame = 0;
            }
        }
    }
}
