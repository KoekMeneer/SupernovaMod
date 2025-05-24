﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.IO;
using SupernovaMod.Api;
using SupernovaMod.Api.Helpers;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using SupernovaMod.Common.ItemDropRules.DropConditions;

namespace SupernovaMod.Content.Npcs.CosmicCollective
{
	public class CosmicCollective : ModNPC
	{
		private const int TENTACLE_SIZE = 80;
		private const int SPRITE_WIDTH  = 376;
		private const int SPRITE_HEIGHT = 372;

        private const int MAX_COSMOLINGS = 14;
		private const float ExpertDamageMultiplier = .7f;

		public enum AIState
		{
			Initial,
			Phase1,
			Phase2,
		}
		public Player target;
		public float npcLifeRatio;

		public AIState State { get; private set; }
		public int EyesActive => _eyesActive;

		private byte _eyesActive = 0;
		private int[] _cosmolingsOwned = new int[MAX_COSMOLINGS + 1];

		protected float Timer
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		protected float AtkPtr
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		protected int AtkTimer { get; set; }
		protected Vector2 NewPosition { get; set; }

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 2;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			NPCID.Sets.CantTakeLunchMoney[Type] = true;
			NPCID.Sets.MPAllowedEnemies[NPC.type] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.ShadowFlame] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Venom] = true;
			NPCID.Sets.TeleportationImmune[NPC.type] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				// Influences how the NPC looks in the Bestiary
				PortraitScale = .5f,
				Scale = .5f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),
            });
        }

        public override void SetDefaults()
		{
			NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
			NPC.lifeMax = 20000; // The Max HP the boss has on Normal
			NPC.damage = 34; // The NPC damage value the boss has on Normal
			NPC.defense = 10; // The NPC defense on Normal
			NPC.knockBackResist = 0f; // No knockback
			NPC.width = SPRITE_WIDTH - (TENTACLE_SIZE * 2);
			NPC.height = SPRITE_HEIGHT - (TENTACLE_SIZE * 2);
            NPC.value = Item.buyPrice(0, 3, 10, 0);
			NPC.npcSlots = 1f; // The higher the number, the more NPC slots NPC NPC takes.
			NPC.boss = true; // Is a boss
			NPC.lavaImmune = true; // Not hurt by lava
			NPC.noGravity = true; // Not affected by gravity
			NPC.noTileCollide = true; // Will not collide with the tiles. 
			NPC.HitSound = SoundID.NPCHit9;
			NPC.DeathSound = SoundID.NPCDeath60;
			Music = MusicID.Boss3;

			//AnimationType = NPCID.Harpy;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)((float)NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)((double)NPC.damage * ExpertDamageMultiplier);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			// Common Loot
			//
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.EldritchEssence>(), 1, 25, 40));

            //
            npcLoot.Add(Common.GlobalNPCs.DropRules.GetDropRule<ExpertModeDropCondition>(conditionalRule =>
            {
                conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Misc.CosmicClock>()));
            }));
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1; // Determines the animation speed. Higher value = faster animation. 
            NPC.frameCounter %= 20;//Main.npcFrameCount[NPC.type]; // TODO: Find out why 20 works here but the standardly used value not...
            int frame = (int)(NPC.frameCounter / 10.0);
            if (frame >= Main.npcFrameCount[NPC.type]) frame = 0;
            NPC.frame.Y = (int)(frame * frameHeight);
        }

        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((int)State);
			writer.Write(_eyesActive);
			writer.Write(AtkTimer);
			writer.WritePackedVector2(NewPosition);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			State = (AIState)reader.ReadInt32();
			_eyesActive = reader.ReadByte();
			AtkTimer = reader.ReadInt32();
            NewPosition = reader.ReadPackedVector2();
        }

		public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
		{
			if (_eyesActive > 0)
			{
				modifiers.SetMaxDamage(1);
				SoundEngine.PlaySound(SoundID.NPCHit4, NPC.position);
			}
		}
		public override bool? CanBeHitByItem(Player player, Item item) => _eyesActive <= 0 ? null : false;
		public override bool? CanBeHitByProjectile(Projectile projectile) => _eyesActive <= 0 ? null : false;
		public override bool CanBeHitByNPC(NPC attacker) => _eyesActive <= 0;

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            // When teleporting the scale of the NPC will be less than 1
            // so also don't hit the player to prevent "telefrag".
            return _eyesActive < 1 || NPC.scale < 1;
        }

        public override void AI()
		{
			AI_Setup();
			if (AI_Despawn())
			{
				return;
			}
			// Handle teleport when player is too far
			//
			if (NPC.ai[2] == 10)
			{
				if (HandleTeleport(ref NPC.ai[3]))
				{
					NPC.ai[2] = 0;

                }
				return;
			}
			switch (State)
			{
				case AIState.Phase1:
					AI_Stage1();
					break;
				case AIState.Phase2:
					AI_Stage2();
					break;
			}
		}
		private void AI_Setup()
		{
			npcLifeRatio = NPC.life / (float)NPC.lifeMax;
			target = Main.player[NPC.target];

			// Run this method once
			//
			if (State == AIState.Initial && Main.netMode != NetmodeID.MultiplayerClient)
			{
				// Set the phase
				State = AIState.Phase1;

				// Spawn eyes
				//
				SpawnEyes();
			}
		}
		private bool AI_Despawn()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}

			Player player = Main.player[NPC.target];
			if (NPC.ai[0] != 3f && (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 3000))
			{
				NPC.TargetClosest(true);
				player = Main.player[NPC.target];
				if (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 3000)
				{
					if (NPC.timeLeft > 180)
					{
						NPC.timeLeft = 180;
					}
					NPC.ai[0] = 0;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.netUpdate = true;

					NPC.velocity = new Vector2(0, -5);
					return true;
				}
			}
            else if (NPC.timeLeft < 1800)
            {
                NPC.timeLeft = 1800;
			}
            // Start teleport when the player is too far
            //
            if (Vector2.Distance(NPC.Center, player.Center) > 1300)
            {
                NPC.ai[2] = 10;
            }
            return false;
		}
		private void AI_Stage1()
		{
			// Move to the next state when at 50% health
			//
			if (State == AIState.Phase1 && Main.netMode != NetmodeID.MultiplayerClient)
			{
                if (npcLifeRatio < .5f)
                {
                    State = AIState.Phase2;
                    SpawnEyes();
                    // Reset:
                    Timer = 0;
                    NPC.scale = 1; // Just to be sure
                    return;
                }
            }

			// Stage 1 while the eyes are alive
			//
			if (_eyesActive > 0)
			{
				Timer++;
				MovementAI(target.Center + new Vector2(0, 25), .5f, .3f);


				// Only in phase 2
				//
				//if (State == AIState.Phase2)
				//{
    //                // Every x time shoot blood shots
    //                //
    //                int shootTime = Main.expertMode ? 375 : 400;
    //                if (Timer % shootTime == 0)
    //                {
    //                    int type = ProjectileID.BloodShot;
    //                    int damage = (int)(34 * ExpertDamageMultiplier);

    //                    SoundEngine.PlaySound(SoundID.NPCDeath55, NPC.Center);
    //                    Vector2 Velocity = -Vector2.UnitY * 12;

    //                    for (int i = 0; i < Main.rand.Next(7, 9); i++)
    //                    {
    //                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Velocity.RotatedByRandom(.8f), type, damage, 4.5f, ai0: NPC.target);
    //                    }
    //                }
    //            }

                // Every x time spawn minions
                //
                int spawnTime = 245;
                if (_eyesActive < 5)
                {
                    spawnTime -= 10;
                }
                if (_eyesActive < 4)
                {
                    spawnTime -= 20;
                }
                if (_eyesActive < 3)
                {
                    spawnTime -= 10;
                }
                if (Timer % spawnTime == 0 && CosmolingsActive() < MAX_COSMOLINGS)
				{
                    SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
                    SpawnCosmolings(Main.rand.Next(2, 3));
                }
			}
			else // Else when no eyes are left
			{
				MovementAI(target.Center + new Vector2(0, 25), 7, .01f);

				switch (AtkPtr)
				{
					case 1:
						if (Timer > 8)
						{
							Timer++;
							if (Timer > 120)
							{
								Timer = 0;
								AtkTimer = 0;
                                AtkPtr = Main.rand.Next(2, 4);
								break;
                            }
                        }
						AtkTimer++;
						if (AtkTimer % 20 == 0)
						{
                            SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
                            SpawnCosmolings(1);
							Timer++;
						}
						break;
					case 2: // Teleport attack
						if (Timer >= 3)
						{
							Timer++;
							if (Timer > 90)
							{
								Timer = 0;
                                AtkTimer = 0;

                                // Make sure a different attack than this one is chosen
                                //
                                // Use max tries to make sure the random value
                                // does not make an infinite loop.
                                const int maxTries = 6;
								for (int i = 0; (AtkPtr == 2 && i < maxTries); i++)
								{
                                    AtkPtr = Main.rand.Next(1, 5);
                                }
							}
							break;
						}
						if (AtkTimer > 0)
						{
							AtkTimer++;
							if (AtkTimer > 60)
							{
								AtkTimer = 0;
							}
							break;
						}
						if (HandleTeleport(ref NPC.ai[2]))
						{
                            int dustType = DustID.HallowedWeapons;
                            Vector2 dustPosition = NPC.Center + NPC.velocity * .4f;
                            for (int i = 0; i < 10; i++)
                            {
                                Vector2 dustVelocity = Utils.ToRotationVector2(Utils.ToRotation(NPC.velocity) + (float)Utils.ToDirectionInt(Utils.NextBool(Main.rand)) * 1.5707964f) * Utils.NextFloat(Main.rand, 2f, 6f);
                                Dust dust = Dust.NewDustDirect(dustPosition, 0, 0, dustType, dustVelocity.X, dustVelocity.Y, 0, default(Color), 1.75f);
                                dust.noGravity = true;
                            }

                            // Shoot
                            //
                            int type = ProjectileID.EyeLaser;
                            int damage = (int)(36 * ExpertDamageMultiplier);

                            SoundEngine.PlaySound(SoundID.NPCDeath55, NPC.Center);
                            Vector2 Velocity = Mathf.VelocityFPTP(NPC.Center, new Vector2(target.Center.X, target.Center.Y), 5.2f);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Velocity.RotatedBy(.25f), type, damage, 3, ai0: NPC.target);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + Velocity, Velocity.RotatedBy(.1f), type, damage, 3, ai0: NPC.target);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + Velocity, Velocity.RotatedBy(-.1f), type, damage, 3, ai0: NPC.target);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Velocity.RotatedBy(-.25f), type, damage, 3, ai0: NPC.target);

                            Timer++;
                            AtkTimer = 0;
                            NPC.ai[2] = 0;
						}
						break;

					case 3:

                        AtkTimer++;
						if (AtkTimer > 40 && AtkTimer < 210)
						{
							if (AtkTimer % 10 == 0)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.CosmicEyeProjectile>(), (int)(38 * ExpertDamageMultiplier), 1, Main.myPlayer, ai1: NPC.whoAmI);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.CosmicEyeProjectile>(), (int)(38 * ExpertDamageMultiplier), 1, Main.myPlayer, ai1: NPC.whoAmI, ai2: -1);
                                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, NPC.Center);
                            }
                        }
						else if (AtkTimer > 420)
						{
                            Timer = 0;
                            AtkTimer = 0;
                            AtkPtr = Main.rand.Next(1, 3);
                        }

                        break;

					default:
                        AtkTimer = 0;
                        AtkPtr = Main.rand.Next(1, 3);
                        break;
				}
			}
		}
		private void AI_Stage2()
		{
			// TODO: Make 1 AI method, and scale up the attacks in stage 2
			AI_Stage1();
		}

		public void OnEyeKilled(CosmicCollectiveEye npc)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				_eyesActive--;
			}
			SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
		}

		private void MovementAI(Vector2 destination, float velocity, float acceleration)
		{
			float gateValue = 100f;
			Vector2 distanceFromTarget = new Vector2(destination.X, destination.Y) - NPC.Center;
			SupernovaUtils.MoveNPCSmooth(NPC, gateValue, distanceFromTarget, velocity, acceleration, true);

			// Set direction
			//
			if (NPC.velocity.X < 0f)
			{
				NPC.direction = -1;
			}
			else
			{
				NPC.direction = 1;
			}
		}

        #region Attack methods

        /// <summary>
        /// Spawns {<paramref name="amount"/>} amount of <see cref="Cosmoling"/>s
        /// </summary>
        /// <param name="amount"></param>
        private void SpawnCosmolings(int amount)
		{
			for (int i = 0; i < _cosmolingsOwned.Length; i++)
			{
				// Check if we already summoned the amount we wanted to summon
				//
				if (amount < 1)
				{
					return;
				}
				// Check if the gazer saved to position i in the array is still active
				//
				int index = _cosmolingsOwned[i];
				if (Main.netMode != NetmodeID.MultiplayerClient && (index == 0 || !Main.npc[index].active || Main.npc[index].timeLeft < 1))
				{
					Vector2 summonPosition = NPC.Center;

					// Spawn effect
					//
					for (int d = 0; d < 4; d++)
					{
						Vector2 dustPos = summonPosition + new Vector2(Main.rand.Next(90, 110), 0).RotatedByRandom(MathHelper.ToRadians(360));
						Vector2 diff = summonPosition - dustPos;
						diff.Normalize();

						Dust.NewDustPerfect(dustPos, ModContent.DustType<Dusts.BloodDust>(), diff * 5, Scale: 1.2f).noGravity = true;
					}

					// Replace null or the old cosmoling with a new cosmoling
					_cosmolingsOwned[i] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)summonPosition.X, (int)summonPosition.Y, ModContent.NPCType<Cosmoling>());

					amount--;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aiSwitch"></param>
		/// <returns></returns>
		private bool HandleTeleport(ref float aiSwitch)
		{
			NPC.velocity = Vector2.Zero;

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{

                if (aiSwitch == 0)
                {
                    NewPosition = target.Center + Main.rand.NextVector2CircularEdge(500, 500);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.DimensionalRift>(), 0, 0);
					SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, NPC.Center);
                    aiSwitch = 1;
                }
                else if (aiSwitch == 1)
                {
                    NPC.scale -= .0375f;
                    if (NPC.scale <= .1f)
                    {
                        aiSwitch = 2;
                        NPC.Center = NewPosition;
                    }
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                    }
                }
                else if (aiSwitch == 2)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NewPosition, Vector2.Zero, ModContent.ProjectileType<Projectiles.DimensionalRift>(), 0, 0);
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, NPC.Center);
                    aiSwitch = 3;
                }
                else
                {
                    if (NPC.scale >= 1)
                    {
                        NPC.scale = 1; // Just to be sure
                        aiSwitch = 0;
                        NPC.ai[2] = 0;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                        }
                        return true; // DONE
                    }
                    NPC.scale += .0375f;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                    }
                }
            }
            return false;
		}

		#endregion

		#region Helper methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private int CosmolingsActive()
		{
			int result = 0;
			for (int i = 0; i < _cosmolingsOwned.Length; i++)
			{
				int npc = _cosmolingsOwned[i];

				if (npc > 0 && Main.npc[npc].active && Main.npc[npc].timeLeft > 1)
				{
					result++;
				}
			}
			return result;
		}

		#endregion

		/// <summary>
		/// Spawns 6 eyes.
		/// </summary>
		private void SpawnEyes()
		{
			// Only run for none muliplayer clients
			//
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

			int offsetXCenter = SPRITE_WIDTH / 2 - TENTACLE_SIZE;
			
			CosmicCollectiveEye newEye;
			NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CosmicCollectiveEye>(), 1, NPC.whoAmI, offsetXCenter - 30, 125);
			_eyesActive++;

			newEye = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CosmicCollectiveEye>(), 1, NPC.whoAmI, offsetXCenter, 35).ModNPC as CosmicCollectiveEye;
			newEye.extraShootDelay = 10;
			_eyesActive++;

            newEye = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CosmicCollectiveEye>(), 1, NPC.whoAmI, -offsetXCenter, 35).ModNPC as CosmicCollectiveEye;
			newEye.extraShootDelay = 20;
			_eyesActive++;

			newEye = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CosmicCollectiveEye>(), 1, NPC.whoAmI, -offsetXCenter + 30, 125).ModNPC as CosmicCollectiveEye;
			newEye.extraShootDelay = 30;
			_eyesActive++;

            newEye = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CosmicCollectiveEye>(), 1, NPC.whoAmI, offsetXCenter - 30, -50).ModNPC as CosmicCollectiveEye;
            newEye.extraShootDelay = 40;
			_eyesActive++;

			newEye = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CosmicCollectiveEye>(), 1, NPC.whoAmI, -offsetXCenter + 30, -50).ModNPC as CosmicCollectiveEye;
			newEye.extraShootDelay = 50;
			_eyesActive++;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                return true;
            }

            SpriteEffects spriteEffects = 0;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Npc[base.NPC.type].Value;
            Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[base.NPC.type] / 2));
            Vector2 npcOffset = base.NPC.Center - screenPos;
            npcOffset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[base.NPC.type])) * base.NPC.scale / 2f;
            npcOffset += origin * base.NPC.scale + new Vector2(0f, base.NPC.gfxOffY);

            // TODO: Add "Get fixed boi" seed intergration
            // 
            if (Main.zenithWorld)
            {
                // TODO
            }
            spriteBatch.Draw(texture, npcOffset, new Rectangle?(base.NPC.frame), base.NPC.GetAlpha(drawColor), base.NPC.rotation, origin, base.NPC.scale, spriteEffects, 0);
            return false;
        }
    }
}
