using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class BookEarthProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Book of Earth");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.penetrate = 7;       //this is the projectile penetration
            projectile.hostile = false;
            projectile.magic = true;        //this make the projectile do magic damage
            projectile.tileCollide = true;  //this make that the projectile does not go thru walls
            projectile.ignoreWater = true;
            projectile.timeLeft = 164;      //The amount of time the projectile is alive for
        }

        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width + 2, projectile.height + 2, 0, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 80, default(Color), 2.9f);
            Main.dust[DustID2].noGravity = true;
            //this make that the projectile faces the right way
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            projectile.localAI[0] += 1f;
            projectile.alpha = (int)projectile.localAI[0] * 2;
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            Explode();
            base.OnHitNPC(target, damage, knockback, crit);
            projectile.timeLeft = 5;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
            Explode();
            projectile.timeLeft = 5;
            return false;
		}

		public void Explode()
		{
            if (projectile.owner == Main.myPlayer)
            {
                projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                projectile.position = projectile.Center;
                //projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                //projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.width = 165;
                projectile.height = 165;
                projectile.Center = projectile.position;
                //projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                //projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            }
		}
		public override void Kill(int timeLeft)
        {
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            // Smoke Dust spawn

            for (int i = 0; i < 50; i++)
            {

                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width / 2, projectile.height / 2, 31, 0f, 0f, 100, default(Color), 2f);

                Main.dust[dustIndex].velocity *= 1.4f;

            }

            // Fire Dust spawn

            for (int i = 0; i < 80; i++)
            {

                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width / 2, projectile.height / 2, 6, 0f, 0f, 100, default(Color), 3f);

                Main.dust[dustIndex].noGravity = true;

                Main.dust[dustIndex].velocity *= 5f;

                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width / 2, projectile.height / 2, 6, 0f, 0f, 100, default(Color), 2f);

                Main.dust[dustIndex].velocity *= 3f;

            }

            // Large Smoke Gore spawn

            for (int g = 0; g < 2; g++)
            {

                int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;

                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;

                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;

                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);

                Main.gore[goreIndex].scale = 1.5f;

                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;

                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;

                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);

                Main.gore[goreIndex].scale = 1.5f;

                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;

                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;

                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);

                Main.gore[goreIndex].scale = 1.5f;

                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
            }
            // reset size to normal width and height.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }
    }
}
