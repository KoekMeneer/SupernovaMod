using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Supernova.Npcs.Bosses.CosmicCollective
{
    [AutoloadBossHead]
    public class CosmicCollective : Boss
    {
        private int _eyesActive = 0;
        public int Stage { get; private set; }
        /* Stats */
        public int smallAttackDamage = 27;
        public int largeAttackDamage = 38;

        /* Stage Attacks */
        public string[] stage0 = new string[] { "atkSpawn" };
        public string[] stage1 = new string[] { "atkSpawn", "atkSpawn", "atkSpawn", "atkSpawn", "atkEye" };
        public string[] stage2 = new string[] { "atkSpawn", "atkEye" };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Collective");
            Main.npcFrameCount[npc.type] = 2;
            Stage = 0;
        }

        public override void SetDefaults()
        {
            targetOffset = new Vector2(0, 0);
            velMax = 2;
            velAccel = .1f;

            attackPointer = 0;
            attacks = stage0;

            npc.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            npc.lifeMax = 3000; // The Max HP the boss has on Normal
            npc.damage = 32; // The base damage value the boss has on Normal
            npc.defense = 47; // The base defense on Normal
            npc.knockBackResist = 0f; // No knockback
            npc.width = 354;
            npc.height = 354;
            npc.value = 10000;
            npc.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
            npc.boss = true; // Is a boss
            npc.lavaImmune = true; // Not hurt by lava
            npc.noGravity = true; // Not affected by gravity
            npc.noTileCollide = true; // Will not collide with the tiles. 
            npc.HitSound = SoundID.NPCHit8;
            npc.DeathSound = SoundID.NPCDeath1;
            music = MusicID.Boss1;
            // bossBag = mod.ItemType("SepticFleshBossBag"); // Needed for the NPC to drop loot bag.
        }
        public override void NPCLoot()
        {
            Screen.Write("TODO: Add npc loot!", Color.Red);
            // Drop the correct drops
            if (Main.expertMode)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarbingersCrest"));
            for (int i = 0; i < Main.rand.Next(1, 2); i++)
                switch (Main.rand.Next(3))
                {
                    case 0:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarbingersSlicer"));
                        break;
                    case 1:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarbingersKnell"));
                        break;
                    default:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarbingersPick"));
                        break;
                }

            // For settings if the boss has been downed
            SupernovaWorld.downedCosmicCollective = true;
        }

        bool init = false;
        public override void AI()
        {

            if (init == false)
            {
                attacks = stage0;
                SpawnEyes(2);
                npc.netUpdate = true;
                init = true;
            }
            // Attack
            Attack();

            // Handle despawning
            DespawnHandler();

            // Move
            target = targetPlayer.Center;
            target.X += targetOffset.X;
            target.Y += targetOffset.Y;

            Move();
        }
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (_eyesActive > 0)
            {
                damage = 0;
                Main.PlaySound(SoundID.NPCHit4, npc.position);
                return false;
            }
            return true;
        }
        public void OnEyeDestroyed(ModNPC eye)
		{
            _eyesActive--;
		}
        public void SpawnEyes(int ammount)
		{
            Main.PlaySound(SoundID.Roar, npc.position, 0);
            _eyesActive = ammount;
            int deg = 360 / ammount;
            for (int i = 0; i < ammount; i++)
                NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("CosmicEye"), ai0: npc.whoAmI, ai1: deg * i);
        }
        #region Attacks
        public void atkSpawn()
		{
            npc.ai[0]++;
            if (npc.ai[0] == 80)
                NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("Cosmoling"));
            else if (npc.ai[0] > 260)
			{
                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkEye()
        {
            npc.ai[0]++;
            if (npc.ai[0] == 50 || npc.ai[0] == 70 || npc.ai[0] == 90)
			{
                float speed = 7;  //projectile speed
                int type = ProjectileID.PhantasmalEye;
                Main.PlaySound(SoundID.Item20, npc.Center);

                Projectile.NewProjectile(npc.position.X + (npc.width / 3), npc.position.Y - 700, speed * Main.rand.NextFloat(.9f, 1.1f), speed * Main.rand.NextFloat(.9f, 1.1f), type, smallAttackDamage, 3, 0);
            }
            else if (npc.ai[0] > 125)
			{
                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        #endregion

        public Vector2 target;
        public Vector2 targetOffset = new Vector2(0, -200);
        public int velMax = 0;
        public float velAccel = 0;
        public float targetVel = 0;
        public float velMagnitude = 0;
        private void Move()
        {
            //float dist = (float)(Math.Sqrt(((target.X - npc.Center.X) * (target.X - npc.Center.X)) + ((target.Y - npc.Center.Y) * (target.Y - npc.Center.Y))));
            float dist = Vector2.Distance(npc.Center, target);
            targetVel = dist / 20;

            // Accel if our velocity is smaller than the taget velocity and max velocity
            if (velMagnitude < velMax && velMagnitude < targetVel)
                velMagnitude += velAccel;

            if (velMagnitude > targetVel)
                velMagnitude -= velAccel;

            // Make sure we don't devide by 0
            if (dist != 0)
                // Move to 'target'
                npc.velocity = npc.DirectionTo(target) * velMagnitude;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter %= 6;
            int frame = (int)(npc.frameCounter / 2.0);
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
        }
        private void DespawnHandler()
        {
            if (targetPlayer == null || !targetPlayer.active || targetPlayer.dead)
            {
                npc.TargetClosest(false);
                targetPlayer = Main.player[npc.target];
                if (!targetPlayer.active || targetPlayer.dead)
                {
                    npc.velocity = new Vector2(0f, -10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            try
			{
                // Stages
               /* if (npc.life <= (npc.lifeMax * .35f))
                    attacks = stage2;
                else if (npc.life <= (npc.lifeMax * .7f))
                    attacks = stage1;*/
                if (npc.life <= (npc.lifeMax * .24f))
                    attacks = stage2;
                else if (npc.life <= (npc.lifeMax * .25f) && Stage < 3)
			    {
                    SpawnEyes(4);
                    Stage = 3;
                }
                else if (npc.life <= (npc.lifeMax * .5f) && Stage < 2)
			    {
                    attacks = stage1;
                    SpawnEyes(4);
                    Stage = 2;
                }
                else if (npc.life <= (npc.lifeMax * .75f) && Stage < 1)
				{
                    SpawnEyes(2);
                    Stage = 1;
                }

                // Reset attack pointer when we have done all the attacks for this stage
                if (attackPointer >= attacks.Length) attackPointer = 0;
            }
			catch
			{

			}
            return true;
        }
    }
}
