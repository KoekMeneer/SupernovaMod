using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Projectiles
{
    public class RagnaArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ragna Arrow");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 1;
            projectile.friendly = true;         
            projectile.hostile = false;         
            projectile.ranged = true;           
            projectile.penetrate = 2;           
            projectile.timeLeft = 600;           
            projectile.ignoreWater = true;      
            projectile.tileCollide = true;      
            projectile.extraUpdates = 1;                                           
            aiType = ProjectileID.WoodenArrowFriendly; 
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors
            //this is projectile dust
            int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 5f), projectile.width + 2, projectile.height + 2, DustID.Shadowflame, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), 1.2f);
            Main.dust[DustID2].noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, 294, (int)(projectile.damage * 0.7), 2.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
            float ShootKnockback = 3.6f;
            Vector2 Target = new Vector2((float)target.position.X, (float)projectile.position.Y);
            float ceilingLimit = 0f;//Target.Y
            if (ceilingLimit > target.Center.Y - 200f)
            {
                ceilingLimit = target.Center.Y - 200f;
            }

            for (int i = 0; i < 3; i++)
            {
                Vector2 position = target.Center;

                position.Y -= (400 * i);

                Vector2 heading = Target - position;
                if (heading.Y < 0f)
                {
                    heading.Y *= -1f;
                }
                if (heading.Y < 20f)
                {
                    heading.Y = 20f;
                }

                heading.Normalize();
                heading *= new Vector2(projectile.velocity.X, projectile.velocity.Y).Length();

                float speedX = heading.X + heading.Y + Main.rand.Next(-15, 16);

                float speedY = heading.Y + Main.rand.Next(-80, 81) + 380;

                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 495, damage, ShootKnockback, projectile.owner, 0f, ceilingLimit);
            }
        }
    }
}
