using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles.Minions
{
    public class RedDevilMinion : ModProjectile
    {
        /// <summary> 
        /// Gets red devil texture 
        /// </summary> 
        public override string Texture => "Terraria/Npc_" + NPCID.RedDevil;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Devil");
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            Main.projFrames[projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.CloneDefaults(317);
            projectile.aiStyle = 62; // for shooting minions

            projectile.width = 25;
            projectile.height = 25;

            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 0;
            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;

            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            projectile.frame = 5;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)

            {
                projectile.tileCollide = false;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.tileCollide = false;
            }
            return false;
        }
        int proj;
        public override void AI()
        {
            CheckActive();
            base.AI();

            projectile.ai[0]++;
            if (projectile.ai[0] >= 180)
            {
                projectile.ai[1]++;
                if (projectile.ai[1] >= 20)
                {
                    TryShoot();
                    projectile.ai[1] = 0;
                    proj++;
                    if (proj >= 5)
                    {
                        projectile.ai[0] = 0;
                        proj = 0;
                    }
                }
            }
            Animate();
        }
        public void Animate()
        {
            // Tickes btween frame 
            if (++projectile.frameCounter >= 8)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 5) // Frames 
                    projectile.frame = 0;
            }
        }

        public void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            SupernovaPlayer modPlayer = (SupernovaPlayer)player.GetModPlayer(mod, "SupernovaPlayer");
            if (player.dead)
                modPlayer.minionRedDevil = false;
            if (modPlayer.minionRedDevil)
                projectile.timeLeft = 2;
            if (!player.HasBuff(mod.BuffType("RedDevilBuff")))
                modPlayer.minionRedDevil = false;
        }

        bool TryShoot()
        {
            NPC target;
            for (int i = 0; i < 200; i++)
            {
                target = Main.npc[i];
                //If the npc is hostile 
                if (!target.friendly)
                {
                    //Get the shoot trajectory from the projectile and target 
                    float shootToX = target.position.X + (float)target.width * .5f - projectile.Center.X;
                    float shootToY = target.position.Y + (float)target.height * .5f - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    //If the distance between the live targeted npc and the projectile is less than 1000 pixels 
                    if (distance < 2500 && !target.friendly && target.active)
                    {
                        // Divide the factor, 3f, which is the desired velocity 
                        distance = 3f / distance;

                        //Multiply the distance by a multiplier if you wish the projectile to have go faster 
                        shootToX *= distance * 3;
                        shootToY *= distance * 3;

                        //Set the velocities to the shoot values 
                        int id = Projectile.NewProjectile(new Vector2(projectile.position.X, projectile.position.Y + 32), Vector2.Zero, ProjectileID.UnholyTridentFriendly, projectile.damage, 2, projectile.owner);
                        Projectile proj = Main.projectile[id];

                        proj.velocity.X = shootToX;
                        proj.velocity.Y = shootToY;

                        return true;
                    }
                }
            }
            return false;
        }
        public override bool MinionContactDamage() => false;
    }
}