using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Npcs.Bosses.CosmicCollective
{
    public class CosmicComet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Comet");
        }
        int timer;
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 6000;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.light = 0.2f;
            //projectile.extraUpdates = 1;
            aiType = -1;
            projectile.timeLeft = 150; //The amount of time the projectile is alive for
        }

        public override void AI()
        {
            timer++;
            if(timer >= 38)
            {
                Shoot();
                timer = 0;
            }
            if (projectile.ai[0] > 12f)  //this defines where the flames starts
            {
                if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
                {//                                                                                                                       DustID
                    int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 71, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 70, default(Color), 0.35f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.2f;
                    int dust2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 71, projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, 70, default(Color), 0.44f); //this defines the flames dust and color parcticles, like when they fall thru ground, change DustID to wat dust you want from Terraria

                    projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 0.80f;

                }
            }
            else
            {
                projectile.ai[0] += 1f;
            }
            if (projectile.localAI[0] > 70f) //projectile time left before disappears
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int Type = 671;
            int ShootDamage = 50;
            float ShootKnockback = 3.6f;
            const int ShootDirection = 7;

            Projectile.NewProjectile(position.X + 20, position.Y + 20, -ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, 0, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, 0, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, -ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, -ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
        }

        void Shoot()
        {
            /*
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int Type = 671;
            int ShootDamage = 35;
            float ShootKnockback = 3.6f;
            const int ShootDirection = -7;

            Projectile.NewProjectile(position.X + 20, position.Y + 20, -ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, 0, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, 0, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, -ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, -ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);*/

            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y + 5, 671, (int)((double)270 * 0.1), 3 * 1, projectile.owner, 1, 0);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y, 671, (int)((double)310 * 0.1), 3 * 1, projectile.owner, 1, 0);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y - 5, 671, (int)((double)150 * 0.1), 3 * 1, projectile.owner, 1, 0);
        }
    }
}
