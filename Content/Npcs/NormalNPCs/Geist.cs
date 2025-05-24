using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using SupernovaMod.Api;
using System.IO;

namespace SupernovaMod.Content.Npcs.NormalNPCs
{
    public class Geist : ModNPC
    {
		private Player player;

        public override void SetStaticDefaults()
        {
			Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x directions
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				// Sets the description of this NPC that is listed in the bestiary.
				//new FlavorTextBestiaryInfoElement(""),
            });
        }
        public override void SetDefaults()
        {
            NPC.width = 46;
            NPC.height = 54;
            NPC.damage = 15;
            NPC.defense = 4;
            NPC.lifeMax = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = Item.buyPrice(0, 0, 0, 50);
            NPC.knockBackResist = .5f;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles.
            NPC.aiStyle = NPCAIStyleID.HoveringFighter;
            AIType = NPCID.Ghost;
		}

        private byte _blinkState;
        protected short _blinkDelay;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(_blinkState);
            writer.Write(_blinkDelay);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            _blinkState = reader.ReadByte();
            _blinkDelay = reader.ReadInt16();
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (NPC.alpha > 128)
            {
                modifiers.SetMaxDamage(1);
                SoundEngine.PlaySound(SoundID.NPCHit4, NPC.position);
            }
        }
        public override bool? CanBeHitByItem(Player player, Item item) => NPC.alpha < 128 ? null : false;
        public override bool? CanBeHitByProjectile(Projectile projectile) => NPC.alpha < 128 ? null : false;
        public override bool CanBeHitByNPC(NPC attacker) => NPC.alpha > 128;

        #region AI
        public override void AI()
        {
            NPC.netUpdate = true;
            HandleBlink();
        }

        private void HandleBlink()
        {
            switch (_blinkState)
            {
                case 0:
                    if (_blinkDelay > 80)
                    {
                        NPC.alpha += 5;
                        if (NPC.alpha > 188)
                        {
                            _blinkState = 1;
                        }
                    }
                    else
                    {
                        _blinkDelay++;
                    }
                    break;

                case 1:
                    if (NPC.alpha > 0)
                    {
                        NPC.alpha -= 5;
                    }
                    else
                    {
                        _blinkState = 2;
                    }
                    break;

                case 2: // Reset
                    _blinkState = 0;
                    _blinkDelay = 0;
                    break;
            }
        }
		#endregion

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
            if (Main.rand.NextBool(4))
            {
				target.AddBuff(BuffID.Darkness, Main.rand.Next(2, 6) * 60);
			}
		}

		public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter -= .3f; // Determines the animation speed. Higher value = faster animation. 
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)(NPC.frameCounter / 6.7);
            if (frame >= Main.npcFrameCount[NPC.type] - 1) frame = 0;
            NPC.frame.Y = frame * frameHeight;

            NPC.spriteDirection = NPC.direction;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneGraveyard)
            {
                return .01f;
            }

            if (Main.dayTime || !spawnInfo.Player.ZoneOverworldHeight || spawnInfo.PlayerSafe || spawnInfo.Player.ZoneDungeon || spawnInfo.PlayerInTown || spawnInfo.Player.ZoneOldOneArmy || Main.snowMoon || Main.pumpkinMoon)
            {
                return 0f;
            }
            return .02f;
        }

		public override void HitEffect(NPC.HitInfo hit)
		{
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }
            for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadewood, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 10; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadewood, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // TODO?
        }
    }
}
