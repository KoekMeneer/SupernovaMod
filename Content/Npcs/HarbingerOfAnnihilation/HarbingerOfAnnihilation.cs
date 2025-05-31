using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent;
using SupernovaMod.Content.Npcs.HarbingerOfAnnihilation.Projectiles;
using Filters = Terraria.Graphics.Effects.Filters;
using Terraria.GameContent.ItemDropRules;
using SupernovaMod.Common.ItemDropRules.DropConditions;
using SupernovaMod.Api;
using SupernovaMod.Common.Systems;
using SupernovaMod.Common;
using System.IO;
using SupernovaMod.Core.Helpers;

namespace SupernovaMod.Content.Npcs.HarbingerOfAnnihilation
{
    [AutoloadBossHead]
    public class HarbingerOfAnnihilation : ModNPC
    {
        protected float npcLifeRatio;
        public Player target;

		private const int DAMAGE_PROJ_MISSILE = 20;
		private const int DAMAGE_PROJ_ORB = 31;

		private readonly int _projIdMissile = ModContent.ProjectileType<HarbingerMissile>();

		/// <summary>
		/// Handles syncing the <see cref="WaitForAllArmsToReturn"/> method return
		/// value between server and client.
		/// </summary>
		private bool _syncWaitForArmsToReturn = false;

		/// <summary>
		/// A collection of all the Harbingers arms.
		/// </summary>
		/// <remarks>NOTE: For <see cref="NetmodeID.MultiplayerClient"/> this array will be empty. When using this array guard the operation so it is not called by a Multiplayer client, otherwise this results in a <see cref="NullReferenceException"/>.</remarks>
		private readonly HarbingerOfAnnihilation_Arm[] _arms = new HarbingerOfAnnihilation_Arm[4];

		private const float ExpertDamageMultiplier = .7f;

		public override void SetStaticDefaults()
        {
			NPCID.Sets.MPAllowedEnemies[NPC.type] = true;
			NPCID.Sets.CantTakeLunchMoney[Type] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;
			NPCID.Sets.TeleportationImmune[NPC.type] = true;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                // Influences how the NPC looks in the Bestiary
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
				new FlavorTextBestiaryInfoElement("An enigmatic subsistence. This stellar entity with a seemingly inexplicable nature all the way back to its formation is tasked with one thing and one thing only - to destitute all planets of their lifeforms."),
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            NPC.lifeMax = 2600; // The Max HP the boss has on Normal
            NPC.damage = 13; // The base damage value the boss has on Normal
            NPC.defense = 8; // The base defense on Normal
            NPC.knockBackResist = 0f; // No knockback
            NPC.width = 68;
            NPC.height = 68;
            NPC.value = 10000;
            NPC.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
            NPC.boss = true; // Is a boss
            NPC.lavaImmune = true; // Not hurt by lava
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles. 
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			// Master mode loot
			//
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.Furniture.HarbingerOfAnnihilationRelic>()));

			// Expert mode loot
			//
			npcLoot.Add(Common.GlobalNPCs.DropRules.GetDropRule<ExpertModeDropCondition>(conditionalRule =>
			{
				conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.HarbingersCrest>()));
			}));

			// Add common loot
			//
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.HarbingerOfAnnihilationTrophy>(), 10));
			npcLoot.Add(ItemDropRule.OneFromOptions(1, new int[]
			{
				ModContent.ItemType<Items.Weapons.Magic.HarbingersKnell>(),
				ModContent.ItemType<Items.Weapons.Melee.HarbingersSlicer>(),
				ModContent.ItemType<Items.Tools.HarbingersPickaxe>(),
			}));
		}

		protected float velocity;
        protected float acceleration;
        public bool SecondPhase { get; private set; } = false;

		private int _attackPointer2 = 0;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(_syncWaitForArmsToReturn);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			_syncWaitForArmsToReturn = reader.ReadBoolean();
		}

		public override void AI()
        {
            // Run this method once, for none multiplayer clients
            //
            if (Main.netMode != NetmodeID.MultiplayerClient && _arms[0] == null)
			{
				int deg = 360 / _arms.Length;
				for (int i = 0; i < _arms.Length; i++)
                {
					int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, 0, 0, ModContent.ProjectileType<HarbingerOfAnnihilation_Arm>(), (int)(16 * ExpertDamageMultiplier), 7, Main.myPlayer, ai0: NPC.whoAmI, ai1: deg * i);
                    _arms[i] = Main.projectile[proj].ModProjectile as HarbingerOfAnnihilation_Arm;
				}
			}

			// Set/Reset all required values
			//
			npcLifeRatio = NPC.life / (float)NPC.lifeMax;

            ref float timer = ref NPC.ai[1];
            ref float attackPointer = ref NPC.ai[0];
            target = Main.player[NPC.target];

            // Handle AI
            //
            if (DespawnAI())
			{
				return; // Don't run any other AI
			}
			if (DeathAI())
			{
				return; // Don't run any other AI
			}

			//
			timer++;

			//
			NPC.netUpdate = true;

			// First phase
			//
			if (npcLifeRatio > .5)
			{
				velocity = Main.masterMode ? 6f : Main.expertMode ? 5f : 4.5f;
				acceleration = .02f;

				ref float index = ref NPC.ai[2];

				if (attackPointer == 0 || attackPointer == 3)
				{
					if (timer == 90 || timer == 100 || timer == 110 || timer == 120)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							// Set a random arm to shoot to the player
							_arms[(int)index].SetState(HoaArmAI.LaunchAtPlayer);
							index++;
						}
					}
					if (timer >= 460 && WaitForAllArmsToReturn())
					{
						index = 0;
						timer = 0;
						attackPointer++;
					}
				}
				else if (attackPointer == 1)
				{
					ref float attackCount = ref NPC.ai[2];

					if (timer == 1)
					{
						attackCount = 1;

						if (npcLifeRatio < .80)
							attackCount++;
						if (npcLifeRatio < .60)
							attackCount++;

						if (Main.masterMode || Main.expertMode)
							attackCount++;
					}
					// Use Main.netMode to guard against multplayer client calls of the GetRandomArm method
					//
					if (Main.netMode != NetmodeID.MultiplayerClient && attackCount > 0 && timer % 90 == 0)
					{
						GetRandomArm().SetState(HoaArmAI.SmashPlayer);
						attackCount--;
					}

					/*if (timer == 40 && (Main.expertMode || Main.masterMode))
                    {
						GetRandomArm().SetState(2);
					}
					if (timer == 120)
                    {
                        GetRandomArm().SetState(2);
					}*/

					if (timer >= 440 && WaitForAllArmsToReturn())
					{
						index = 0;
						timer = 0;
						attackPointer++;
					}
				}
				else if (attackPointer == 2)
				{
					if (timer == 1)
					{
						ForeachArm(arm => arm.SetState(HoaArmAI.CirclePlayerAndShoot));
					}
					if (timer >= 480 && WaitForAllArmsToReturn())
					{
						index = 0;
						timer = 0;
						attackPointer++;
					}
				}
				else if (attackPointer == 4)
				{
					AttackCastOrb(ref timer, ref attackPointer);
					return;
				}
				else
				{
					attackPointer = 0;
				}

				MovementAI(GetDesiredDestination(), velocity, acceleration);
			}
			// Transition betweem phase 1 and 2
			//
			else if (!SecondPhase)
			{
				NPC.localAI[0]++;

				if (NPC.localAI[0] == 1)
				{
					ForeachArm(arm =>
					{
						arm.Projectile.alpha = 250;
						arm.Projectile.hostile = false;
						arm.SetState(HoaArmAI.Reset);
					});
					NPC.dontTakeDamage = true;
				}

				if (NPC.localAI[0] < 140)
				{
					NPC.velocity = Vector2.Zero;
					NPC.rotation += (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.ToRadians(10);

					// Charge dust effect
					//
					SoundEngine.PlaySound(SoundID.Item15);
					Vector2 dustPos = NPC.Center + new Vector2(120, 0).RotatedByRandom(MathHelper.ToRadians(360));
					Vector2 diff = NPC.Center - dustPos;
					diff.Normalize();

					Dust.NewDustPerfect(dustPos, DustID.UndergroundHallowedEnemies, diff * 10).noGravity = true;
					Dust.NewDustPerfect(dustPos, DustID.Vortex, diff * 10).noGravity = true;
				}
				else if (NPC.localAI[0] == 140)
				{
					NPC.rotation = 0;
				}

				// End animation
				//
				if (NPC.localAI[0] > 160/* && WaitForAllArmsToReturn()*/)
				{
					NPC.localAI[0] = 0;
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.ai[2] = 0;
					SecondPhase = true;
					NPC.dontTakeDamage = false;
					ForeachArm(arm =>
					{
						arm.Projectile.alpha = 0;
						arm.Projectile.hostile = true;
					});
				}
			}
			else
			{
				/*if (_attackPointer2 == 0 || _attackPointer2 == 1)
				{
					if (_attackPointer2 == 0)
					{
						AttackMeteorShower(ref timer, ref attackPointer, 4);
					}
					else
					{
						timer = 0;
						attackPointer++;
					}
				}
				else*/ if (_attackPointer2 == 0 || _attackPointer2 == 1)
				{
					if (attackPointer == 0)
					{
						AttackCastOrb(ref timer, ref attackPointer, 1.25f);
						return;
					}
					else if (attackPointer == 1 && _attackPointer2 == 0)
					{
						ref float direction = ref NPC.ai[2];

						if ((NPC.position.X - target.position.X) > 0)
						{
							direction = 1;
						}
						else
						{
							direction = -1;
						}

						if (timer < 400)
						{
							if (timer == 1)
							{
								ForeachArm(arm => arm.canDealDamage = true, true);
							}
							velocity = 15;
							acceleration = .1f;

							NPC.rotation += MathHelper.ToRadians(15) * direction;
							NPC.rotation = NPC.rotation % 360;
						}
						else
						{
							NPC.velocity = Vector2.Zero;
							NPC.rotation = 0;

							if (timer > 520 && NPC.rotation == 0 && WaitForAllArmsToReturn())
							{
								timer = 0;
								direction = 0;
								attackPointer++;
							}
						}
						MovementAI(GetDesiredDestinationDirect(), velocity, acceleration);
					}
					else if (attackPointer == 1)
					{
						MovementAI(GetDesiredDestination(), velocity, acceleration);
						ref float index = ref NPC.ai[2];

						if (timer == 90 || timer == 100 || timer == 110 || timer == 120)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								// Set a random arm to shoot to the player
								_arms[(int)index].SetState(HoaArmAI.LaunchAtPlayer);
							}
							index++;
						}
						if (timer >= 400 && WaitForAllArmsToReturn())
						{
							index = 0;
							timer = 0;
							attackPointer++;
						}
					}
					else if (attackPointer == 2 || attackPointer == 3)
					{
						if (_attackPointer2 == 0)
						{
							AttackLightningArms(ref timer, ref attackPointer);
						}
						else if (attackPointer == 2)
						{
							AttackBlackHole(ref timer, ref attackPointer);
						}
						else if (attackPointer == 3)
							attackPointer++;
					}
					else
					{
						timer = 0;
						attackPointer = 0;
						_attackPointer2 = Main.rand.Next(3);
					}
				}
				else
				{
					if (attackPointer == 0)
					{
						NPC.velocity = Vector2.Zero;
						ref float attackCount = ref NPC.ai[2];

						if (timer == 1)
						{
							attackCount = 3;
							if (Main.masterMode || Main.expertMode)
								attackCount++;
						}

						// Use Main.netMode to guard against multplayer client calls of the GetRandomArm method
						//
						if (Main.netMode != NetmodeID.MultiplayerClient && attackCount > 0 && timer % 80 == 0)
						{
							GetRandomArm().SetState(HoaArmAI.SmashPlayer);
							attackCount--;
						}

						if (timer >= 380 && WaitForAllArmsToReturn())
						{
							timer = 0;
							attackPointer++;
						}
					}
					else if (attackPointer == 1)
					{
						MovementAI(GetDesiredDestination(), velocity, acceleration);

						if (timer > 120)
						{
							timer = 0;
							attackPointer++;
						}
					}
					else if (attackPointer == 2 || attackPointer == 3)
					{
						if (_attackPointer2 == 0)
						{
							AttackLightningArms(ref timer, ref attackPointer);
						}
						else if (attackPointer == 2)
						{
							AttackBlackHole(ref timer, ref attackPointer);
						}
						else if (attackPointer == 3)
							attackPointer++;
					}
					else
					{
						timer = 0;
						attackPointer = 0;
						_attackPointer2 = Main.rand.Next(3);
					}
				}
			}
        }

		#region AI Methods
		public override bool CheckDead()
		{
			if (NPC.ai[3] == 0f)
			{
				NPC.ai[3] = 1f;
				NPC.damage = 0;
				NPC.life = NPC.lifeMax;
				NPC.dontTakeDamage = true;
				NPC.netUpdate = true;
				return false;
			}
			return true;
		}

		private int rippleCount = 3;
		private int rippleSize = 5;
		private int rippleSpeed = 30;
		private float distortStrength = 200f;
		private bool DeathAI()
		{
			if (NPC.ai[3] == 1)
			{
				NPC.velocity = Vector2.Zero;
			}
			if (NPC.ai[3] > 0)
			{
				ForeachArm(arm => arm.Projectile.Kill());
				NPC.dontTakeDamage = true;
				NPC.ai[3]++; // increase our death timer.
							 //npc.velocity = Vector2.UnitY * npc.velocity.Length();
				/*NPC.velocity.X *= 0.95f; // lose inertia
				if (NPC.velocity.Y < 0.5f)
				{
					NPC.velocity.Y = NPC.velocity.Y + 0.02f;
				}
				if (NPC.velocity.Y > 0.5f)
				{
					NPC.velocity.Y = NPC.velocity.Y - 0.02f;
				}*/

				if (NPC.ai[3] > 150 && Main.netMode != NetmodeID.Server)
				{
					if (Filters.Scene["SupernovaMod:shockwave"].IsActive())
					{
						float progress = (180 - NPC.ai[3]) / 40;
						Filters.Scene["SupernovaMod:shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
					}
					else
					{
						Filters.Scene.Activate("SupernovaMod:shockwave", NPC.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(NPC.Center);
					}
				}

				if (NPC.ai[3] > 120)
				{
					NPC.Opacity = 1f - (NPC.ai[3] - 120f) / 60f;
				}
				if (NPC.ai[3] < 180)
				{
					NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
					NPC.velocity = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi).ToRotationVector2();

					// Charge dust effect
					//
					SoundEngine.PlaySound(SoundID.Item15);
					Vector2 dustPos = NPC.Center + new Vector2(120, 0).RotatedByRandom(MathHelper.ToRadians(360));
					Vector2 diff = NPC.Center - dustPos;
					diff.Normalize();

					Dust.NewDustPerfect(dustPos, DustID.UndergroundHallowedEnemies, diff * 10).noGravity = true;
					Dust.NewDustPerfect(dustPos, DustID.Vortex, diff * 10).noGravity = true;
				}

				if (NPC.ai[3] % 60f == 1f)
				{
					//SoundEngine.PlaySound(4, npc.Center, 22);
					SoundEngine.PlaySound(SoundID.NPCDeath22, NPC.Center); // every second while dying, play a sound
				}
				if (NPC.ai[3] >= 180)
				{
					if (Main.netMode != NetmodeID.Server && Filters.Scene["SupernovaMod:shockwave"].IsActive())
					{
						Filters.Scene["SupernovaMod:shockwave"].Deactivate();
					}

					SoundEngine.PlaySound(SoundID.Item14);

					for (int num925 = 0; num925 < 4; num925++)
					{
						int num915 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Vortex, 0f, 0f, 100, default, 1.5f);
						Main.dust[num915].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
					}
					for (int num924 = 0; num924 < 30; num924++)
					{
						int num917 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.UndergroundHallowedEnemies, 0f, 0f, 200, default, 3.7f);
						Main.dust[num917].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
						Main.dust[num917].noGravity = true;
						Dust dust24 = Main.dust[num917];
						dust24.velocity *= 3f;
						num917 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Vortex, 0f, 0f, 100, default, 1.5f);
						Main.dust[num917].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
						dust24 = Main.dust[num917];
						dust24.velocity *= 2f;
						Main.dust[num917].noGravity = true;
						Main.dust[num917].fadeIn = 2.5f;
					}
					for (int i = 0; i < 25; i++)
					{
						Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(-40, 40), 0).RotatedByRandom(MathHelper.ToRadians(360));
						Vector2 diff = NPC.Center - dustPos;
						diff.Normalize();

						Dust.NewDustPerfect(dustPos, DustID.UndergroundHallowedEnemies, -diff * 20, Scale: Main.rand.Next(1, 4)).noGravity = true;
						Dust.NewDustPerfect(dustPos, DustID.Vortex, -diff * 20, Scale: Main.rand.Next(1, 4)).noGravity = true;
					}
					for (int num921 = 0; num921 < 2; num921++)
					{
						Vector2 position16 = NPC.position + new Vector2(NPC.width * Main.rand.Next(100) / 100f, NPC.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f;
						Vector2 vector33 = default;
						int num920 = Gore.NewGore(NPC.GetSource_Death(), position16, vector33, Main.rand.Next(61, 64), 1f);
						Main.gore[num920].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
						Gore gore2 = Main.gore[num920];
						gore2.velocity *= 0.3f;
						Gore expr_79A3_cp_0 = Main.gore[num920];
						expr_79A3_cp_0.velocity.X = expr_79A3_cp_0.velocity.X + Main.rand.Next(-10, 11) * 0.05f;
						Gore expr_79D3_cp_0 = Main.gore[num920];
						expr_79D3_cp_0.velocity.Y = expr_79D3_cp_0.velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
					}

					NPC.life = 0;
					NPC.HitEffect(0, 0);
					NPC.checkDead(); // This will trigger ModNPC.CheckDead the second time, causing the real death.
				}
				return true;
			}
			return false;
		}
        private bool DespawnAI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 10000)
            {
                NPC.TargetClosest(true);
            }

            Player player = Main.player[NPC.target];
            if (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 10000)
            {
                NPC.TargetClosest(true);
                player = Main.player[NPC.target];
                if (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 10000)
                {
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;

					NPC.velocity = Vector2.Zero;
					NPC.ai[3]++;

					if (NPC.alpha < 255)
					{
						NPC.alpha++;
					}
					if (NPC.ai[3] == 1)
					{
						ForeachArm(arm => arm.SetState(HoaArmAI.Reset));
					}
					else if (NPC.ai[3] < 180)
					{
						
						// Charge dust effect
						//
						SoundEngine.PlaySound(SoundID.Item15);
						Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(110, 130), 0).RotatedByRandom(MathHelper.ToRadians(360));
						Vector2 diff = NPC.Center - dustPos;
						diff.Normalize();

						Dust.NewDustPerfect(dustPos, DustID.UndergroundHallowedEnemies, diff * 10, Scale: 2).noGravity = true;
						Dust.NewDustPerfect(dustPos, DustID.Vortex, diff * 10, Scale: 2).noGravity = true;
					}
					else
					{
						NPC.timeLeft = 0;
						NPC.active = false;
					}

					return true;
                }
            }
            else if (NPC.timeLeft < 1800)
            {
                NPC.timeLeft = 1800;
            }
			return false;
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

        private Vector2 _desiredProjectileDestination;
		private Vector2 _desiredDestination;
		private Vector2 GetDesiredDestinationDirect()
        {
			if (target.position == Vector2.Zero)
			{
				return target.position;
			}
			if (_desiredDestination == Vector2.Zero ||
				Vector2.Distance(target.position, _desiredDestination) >= 350 ||    // Make sure the boss will not get to far from the player
				Vector2.Distance(NPC.position, _desiredDestination) <= 50           // Pick a new target destination when in 50 blocks of the prev target
			)
			{
				_desiredDestination = target.position;

                Vector2 extra = target.position - NPC.position;
				extra.Normalize();
				_desiredDestination += extra * target.velocity;
			}
			return _desiredDestination;
		}
		private Vector2 GetDesiredDestination()
		{
            if (target.position == Vector2.Zero)
            {
                return target.position;
            }

			if (_desiredDestination == Vector2.Zero || 
                Vector2.Distance(target.position, _desiredDestination) >= 500 ||    // Make sure the boss will not get to far from the player
				Vector2.Distance(target.position, _desiredDestination) <= 80 ||     // Make sure the boss will not get to close to the player (this may limit player movement options)
				Vector2.Distance(NPC.position, _desiredDestination) <= 50           // Pick a new target destination when in 50 blocks of the prev target
			)
			{
                _desiredDestination = target.position;

				if (Main.rand.NextBool())
                {
                    _desiredDestination.X += Main.rand.Next(15, 250) * -NPC.direction;
				}
                else
                {
					_desiredDestination.Y += Main.rand.Next(15, 250) * -NPC.direction;
				}
			}
			return _desiredDestination;
		}
		#endregion

		#region Attack Methods

		void AttackLightningArms(ref float timer, ref float attackPointer)
		{
			if (attackPointer == 2)
			{
				ref float direction = ref NPC.localAI[0];
				if (timer < 300)
				{
					if (direction == 0)
					{
						direction = Main.rand.NextBool() ? 1 : -1;
						ForeachArm(arm => arm.canDealDamage = false, true);
					}
					if (timer <= 221)
					{
						_desiredProjectileDestination = target.Center - new Vector2(0, 500 * direction);
					}
					if (timer == 222 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						ForeachArm(arm => arm.canDealDamage = true, true);
						_desiredProjectileDestination += new Vector2(0, 1000 * direction);
						SoundEngine.PlaySound(SoundID.Item117, _arms[1].Projectile.Center);
					}

					if (timer < 400 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						HarbingerOfAnnihilation_Arm arm = _arms[0];
						arm.customTarget = _desiredProjectileDestination - new Vector2(200, 0) * direction;
						arm.customDuration = 380;
						arm.Projectile.ai[1] = _arms[1].Projectile.whoAmI;
						arm.SetState(HoaArmAI.LightningLink);

						arm = _arms[1];
						arm.customTarget = _desiredProjectileDestination + new Vector2(200, 0) * direction;
						arm.customDuration = 380;
						arm.Projectile.ai[1] = _arms[0].Projectile.whoAmI;
						arm.SetState(HoaArmAI.LightningLink);
					}
				}
				else
				{
					timer = 0;
					direction = 0;
					attackPointer++;
				}
			}
			else if (attackPointer == 3)
			{
				ref float direction = ref NPC.localAI[0];
				if (timer < 480)
				{
					if (direction == 0)
					{
						direction = Main.rand.NextBool() ? 1 : -1;
						ForeachArm(arm => arm.canDealDamage = false, true);
					}
					if (timer <= 221)
					{
						_desiredProjectileDestination = target.Center - new Vector2(500 * direction, 0);
					}
					if (timer == 222 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						ForeachArm(arm => arm.canDealDamage = true, true);

						_desiredProjectileDestination += new Vector2(1000 * direction, 0);
						SoundEngine.PlaySound(SoundID.Item117, _arms[1].Projectile.Center);
					}

					if (timer < 380 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						HarbingerOfAnnihilation_Arm arm = _arms[2];
						arm.customTarget = _desiredProjectileDestination - new Vector2(0, 200) * direction;
						arm.customDuration = 380;
						arm.Projectile.ai[1] = _arms[3].Projectile.whoAmI;
						arm.SetState(HoaArmAI.LightningLink);

						arm = _arms[3];
						arm.customTarget = _desiredProjectileDestination + new Vector2(0, 200) * direction;
						arm.customDuration = 380;
						arm.Projectile.ai[1] = _arms[2].Projectile.whoAmI;
						arm.SetState(HoaArmAI.LightningLink);
					}
				}
				else if (WaitForAllArmsToReturn())
				{
					timer = 0;
					direction = 0;
					attackPointer++;
				}
			}
		}
		void AttackBlackHole(ref float timer, ref float attackPointer)
		{
			ref float isAtTarget = ref NPC.localAI[0];
			ref float angle = ref NPC.localAI[1];

			if (isAtTarget == 1)
			{
				// Play Charge effect
				//
				if (timer < 90)
				{
					NPC.velocity = Vector2.Zero;
					NPC.rotation = 0;

					if (timer == 20)
					{
						SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
					}
					Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(115, 125), 0).RotatedByRandom(MathHelper.ToRadians(360));
					Vector2 diff = NPC.Center - dustPos;
					diff.Normalize();

					Dust.NewDustPerfect(dustPos, DustID.Demonite, diff * 10, Scale: 2).noGravity = true;
					Dust.NewDustPerfect(dustPos, DustID.Corruption, diff * 10, Scale: 2).noGravity = true;
					return;
				}

				if (timer == 90 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HoaBlackHole>(), (int)(30 * ExpertDamageMultiplier), 4, Main.myPlayer, 1200, .2f);
					Main.projectile[proj].timeLeft = 440;
					ForeachArm(arm =>
					{
						arm.customTarget = Main.projectile[proj].Center;
						arm.customDuration = 440;
						arm.SetState(HoaArmAI.CircleTarget);
					});
				}
				else if (timer > 120 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					velocity = Main.masterMode ? 7.5f : 5;
					acceleration = .04f;
					MovementAI(GetDesiredDestination(), velocity, acceleration);
					if (timer % 90 == 0)
					{
						ShootToPlayer(_projIdMissile, DAMAGE_PROJ_MISSILE, 1.2f, Main.rand.NextFloat(.995f, 1.05f));
					}
				}

				if (timer >= 600)
				{
					timer = 0;
					isAtTarget = 0;
					attackPointer++;
				}
			}
			else if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (Vector2.Distance(NPC.Center, target.Center - new Vector2(0, 250)) <= 75)
				{
					isAtTarget = 1;
				}
				timer = 0;
				MovementAI(target.Center - new Vector2(0, 250), velocity * 1.5f, .2f);
			}
		}

		private void StartCast(Vector2 lookTarget)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

			HarbingerOfAnnihilation_Arm arm = _arms[0];
			arm.customTarget = NPC.Center + (new Vector2(100, 100) * NPC.direction);
			arm.customLookTarget = lookTarget;
			arm.SetState(HoaArmAI.GotoTarget);

			arm = _arms[1];
			arm.customTarget = NPC.Center + (new Vector2(50, 50) * NPC.direction);
			arm.customLookTarget = lookTarget;
			arm.SetState(HoaArmAI.GotoTarget);

			arm = _arms[2];
			arm.customTarget = NPC.Center + (new Vector2(50, -50) * NPC.direction);
			arm.customLookTarget = lookTarget;
			arm.SetState(HoaArmAI.GotoTarget);

			arm = _arms[3];
			arm.customTarget = NPC.Center + (new Vector2(100, -100) * NPC.direction);
			arm.customLookTarget = lookTarget;
			arm.SetState(HoaArmAI.GotoTarget);
		}
		void AttackCastOrb(ref float timer, ref float attackPointer, float timeLeftMulti = 1)
        {
			NPC.velocity = Vector2.Zero;

			if ((NPC.Center.X - target.Center.X) > 0)
			{
				NPC.direction = -1;
			}
			else
			{
				NPC.direction = 1;
			}

			Vector2 lookTarget = NPC.Center + (new Vector2(100, 0) * NPC.direction);
			if (timer == 50 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				StartCast(lookTarget);

				NPC.localAI[3] = Projectile.NewProjectile(NPC.GetSource_FromAI(), lookTarget, Vector2.Zero, ModContent.ProjectileType<HarbingerOrb>(), (int)(DAMAGE_PROJ_ORB * .65f), 6, Main.myPlayer);
				Projectile proj = Main.projectile[(int)NPC.localAI[3]];
				proj.timeLeft = (int)(proj.timeLeft * timeLeftMulti);
			}

			if (timer >= 220 && WaitForAllArmsToReturn())
			{
				NPC.localAI[3] = 0;
				timer = 0;
				attackPointer++;
			}
			// When the projectile dies while it is cast, stop casting
			//
			else if (NPC.localAI[3] != 0 && !Main.projectile[(int)NPC.localAI[3]].active)
			{
				timer = 220;
			}
		}
		void AttackMeteorShower(ref float timer, ref float attackPointer, int amount)
		{
			if (timer == 20)
			{
				// SHOW TRAGET POSITIONS
			}

			if (timer == 80)
			{
				for (int i = 0; i < amount; i++)
				{
					Vector2 velocity = Vector2.UnitY * 6.5f;
					velocity.X = Main.rand.NextFloat(-1, 1);
					int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), target.Center + new Vector2(0, -900), velocity, TerrariaRandom.NextProjectileIDMeteor(), DAMAGE_PROJ_MISSILE, 5, Main.myPlayer, 1, 1, 1);
					Main.projectile[proj].friendly = false;
					Main.projectile[proj].hostile = true;
				}
			}

			if (timer >= 140)
			{
				timer = 0;
				attackPointer++;
			}
		}
		#endregion

		#region Helper Methods
		private void ShootToPlayer(int type, int damage, float velocityMulti = 1, float rotationMulti = 1)
		{
			//int type = ModContent.ProjectileType<TerrorProj>();
			SoundEngine.PlaySound(SoundID.Item20, NPC.Center);

			Vector2 position = NPC.Center;
			float rotation = (float)Math.Atan2(position.Y - (target.position.Y + target.height * 0.2f), position.X - (target.position.X + target.width * 0.15f));
			rotation *= rotationMulti;

			Vector2 velocity = new Vector2((float)-(Math.Cos(rotation) * 18) * .75f, (float)-(Math.Sin(rotation) * 18) * .75f) * velocityMulti;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, (int)(damage * ExpertDamageMultiplier), 0f, 0);

			for (int x = 0; x < 5; x++)
			{
				int dust = Dust.NewDust(position, NPC.width, NPC.height, DustID.UndergroundHallowedEnemies, velocity.X / 2, velocity.Y / 2, 80, default, Main.rand.NextFloat(.9f, 1.6f));
				Main.dust[dust].noGravity = true;
			}
		}
		/// <summary>
		/// Calls the given <paramref name="action"/> for each one of the arms.
		/// </summary>
		/// <remarks>Note: This method has a build in guard agains multiplayer client calls</remarks>
		/// <param name="autoSync">When run by <see cref="NetmodeID.Server"/> syncs the projectile to all Multiplayer clients.</param>
		private void ForeachArm(Action<HarbingerOfAnnihilation_Arm> action, bool autoSync = false)
        {
			for (int i = 0; i < _arms.Length; i++)
            {
				if (_arms[i] == null)
				{
					continue;
				}
                action(_arms[i]);
				if (autoSync && Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.SyncProjectile, number: _arms[i].Projectile.whoAmI);
				}
			}
		}
		/// <summary>
		/// Gets a random one of the arms.
		/// </summary>
		/// <remarks>Note: Guard this method call for multiplayer!</remarks>
		/// <param name="getInactiveOnly">Makes the method only return an arm in state Idle</param>
		/// <returns></returns>
        private HarbingerOfAnnihilation_Arm GetRandomArm(bool getInactiveOnly = false)
        {
			HarbingerOfAnnihilation_Arm randArm = _arms[Main.rand.Next(0, _arms.Length)];
            if (!getInactiveOnly || randArm.Projectile.ai[0] == 0)
            {
                return randArm;
            }
            return GetRandomArm();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="timer"></param>
		/// <returns>If all arms have returned to thier start position</returns>
		private bool WaitForAllArmsToReturn()
		{
			// Only run this method only for the server
			// or single player client.
			//
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return _syncWaitForArmsToReturn;
			}
			// Reset the sync handle
			//
			if (Main.netMode == NetmodeID.Server)
			{
				_syncWaitForArmsToReturn = false;
			}

			// Check for all arms if they have the idle AI
			//
			for (int i = 0; i < _arms.Length; i++)
			{
				// Check if the arm has the idle AI
				//
				if (_arms[i].AttackPointer != HoaArmAI.CircleHoa)
				{
					return false;
				}
			}
			// Set the sync handle to true
			//
			if (Main.netMode == NetmodeID.Server)
			{
				_syncWaitForArmsToReturn = true;
			}
			return true;
		}
		#endregion

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			int m = 0;
			while ((double)m < hit.Damage / (double)NPC.lifeMax * 50.0)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UndergroundHallowedEnemies, hit.HitDirection, -1f, 0, default, 1.2f);
				m++;
			}
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)((float)NPC.lifeMax * 0.8f * balance);
			NPC.damage = (int)((double)NPC.damage * (ExpertDamageMultiplier + .1f));
		}

		public override void OnKill()
		{
			DownedSystem.downedHarbingerOfAnnihilation = true;
			SupernovaNetworking.SyncWorldData();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = 0;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Color color24 = NPC.GetAlpha(drawColor);
            Color color25 = Lighting.GetColor((int)(NPC.position.X + NPC.width * 0.5) / 16, (int)((NPC.position.Y + NPC.height * 0.5) / 16.0));
            Texture2D texture2D3 = TextureAssets.Npc[NPC.type].Value;
            int num156 = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            int y3 = num156 * (int)NPC.frameCounter;
            Rectangle rectangle = new Rectangle(0, y3, texture2D3.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            int num157 = 8;
            int num158 = 2;
            int num159 = 1;
            float num160 = 0f;
            int num161 = num159;
            spriteBatch.Draw(texture2D3, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), new Rectangle?(NPC.frame), color24, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 0f);
            while (num158 > 0 && num161 < num157 || num158 < 0 && num161 > num157)
            {
                Color color26 = NPC.GetAlpha(color25);
                float num162 = num157 - num161;
                if (num158 < 0)
                {
                    num162 = num159 - num161;
                }
                color26 *= num162 / (NPCID.Sets.TrailCacheLength[NPC.type] * 1.5f);
                Vector2 value4 = NPC.oldPos[num161];
                float num163 = NPC.rotation;
                Main.spriteBatch.Draw(texture2D3, value4 + NPC.Size / 2f - screenPos + new Vector2(0f, NPC.gfxOffY), new Rectangle?(rectangle), color26, num163 + NPC.rotation * num160 * (num161 - 1) * -(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin2, NPC.scale, spriteEffects, 0f);
                num161 += num158;
            }
			return false;
		}
    }
}
