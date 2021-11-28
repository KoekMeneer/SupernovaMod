using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Supernova.Npcs.Bosses.CosmicCollective
{
    public class CosmicEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Balanced Orb");
        }
        CosmicCollective owner;

        public override void SetDefaults()
        {
            npc.lifeMax = 500;
            npc.defense = 12;
            npc.width = 80;
            npc.height = 80;
            npc.damage = 60;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true; // Not affected by gravity
        }

        public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.modProjectile != null)
                return projectile.modProjectile.OnTileCollide(oldVelocity);
            return true;
        }

        public override void AI()
        {
            Lighting.AddLight(npc.Center, 0, (255 - npc.alpha) / 255f, ((255 - npc.alpha) * 0.05f) / 255f);
            int DustID2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width - 10, npc.height - 10, DustID.Electric, npc.velocity.X * 1.5f, npc.velocity.Y * 1.5f, 80, default(Color));
            Main.dust[DustID2].noGravity = true;

            owner = (CosmicCollective)Main.npc[(int)npc.ai[0]].modNPC;

            //Factors for calculations 
            double deg = (double)npc.ai[1];         //The degrees, you can multiply projectile.ai[0] to make it orbit faster, may be choppy depending on the value 
            double rad = deg * (Math.PI / 180);     //Convert degrees to radians 
            double dist = 320;                      //Distance away from the owner 

            /*Position the owner based on where the owner is, the Sin/Cos of the angle times the / 
            /distance for the desired distance away from the owner minus the projectile's width   / 
            /and height divided by two so the center of the projectile is at the right place.     */
            npc.position.X = owner.npc.Center.X - (int)(Math.Cos(rad) * dist) - npc.width / 2;
            npc.position.Y = owner.npc.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height / 2;

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value 
            npc.ai[1] += 1f;
            if (((owner.Stage == 1 || owner.Stage == 2) && npc.ai[1] % 360 == 0) || (owner.Stage == 3 && npc.ai[1] % 180 == 0))
			{
                switch (owner.Stage)
				{
                    case 1:
                        npc.defense = 14;
                        break;
                    case 2:
                        npc.defense = 18;
                        break;
                    case 3:
                        npc.defense = 24;
                        break;
                }
                float Speed = 6.5f;
                int type = ProjectileID.PhantasmalBolt;
                Vector2 vector8 = new Vector2(npc.Center.X, npc.Center.Y);

                float rotation = (float)Math.Atan2(vector8.Y - (owner.targetPlayer.position.Y + (owner.targetPlayer.height * 0.5f)), vector8.X - (owner.targetPlayer.position.X + (owner.targetPlayer.width * 0.5f)));

                Projectile.NewProjectile(vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, 24, 1);
            }
            CheckActive();
        }
		public override void NPCLoot()
		{
            // Tell the owner we are killed
            if (owner.CheckActive())
                owner.OnEyeDestroyed(this);
        }
    }
}