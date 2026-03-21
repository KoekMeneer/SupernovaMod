using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.CosmicCollective
{
	public class Cosmoling : ModNPC
	{
		private float _speed = 8;
		private Player _player = null!;

		private Vector2 _dashDirection;

        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				new FlavorTextBestiaryInfoElement(
					"Fragments of cosmic flesh given form by the Cosmic Collective. " +
					"They swarm intruders relentlessly, darting in short bursts before regrouping."
				)
			});
        }

		public override void SetDefaults()
		{
			NPC.lifeMax = 60;
			NPC.defense = 9;
			NPC.width = 50;
			NPC.height = 40;
			NPC.damage = 42;
			NPC.knockBackResist = .5f;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.NPCHit9;
			NPC.DeathSound = SoundID.NPCDeath12;
			NPC.noGravity = true; // Not affected by gravity
			NPC.noTileCollide = true; // Can not collide with tiles
			NPC.scale = .8f;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)((float)NPC.lifeMax * 0.87f * balance);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.WriteVector2(_dashDirection);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            _dashDirection = reader.ReadVector2();
        }

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 2; // Determines the animation speed. Higher value = faster animation. 
            NPC.frameCounter++;
            if (NPC.frameCounter >= 20)
                NPC.frameCounter = 0;

            int frame = (int)(NPC.frameCounter / 10);
            if (frame >= Main.npcFrameCount[NPC.type]) frame = 0;
			NPC.frame.Y = (int)(frame * frameHeight);

			NPC.spriteDirection = -NPC.direction;
		}

		public override void AI()
		{
			NPC.ai[0]++;

			Target(); // Sets the Player Target
			DespawnHandler(); // Handles if the NPC should despawn.


            // Core fleshy trail
            //
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(
                    NPC.position,
                    NPC.width,
                    NPC.height,
                    DustID.Blood,
                    NPC.velocity.X * 0.15f,
                    NPC.velocity.Y * 0.15f,
                    100,
                    default,
                    1.1f
                );

                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }

            // Darker "meaty" particles for depth
            //
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(
                    NPC.Center,
                    6, 6,
                    DustID.CrimsonPlants,
                    0f, 0f,
                    0,
                    default,
                    0.9f
                );

                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = NPC.velocity * -0.05f;
            }

            // Add "pulse" effect
            float pulse = 0.05f * (float)Math.Sin(Main.GameUpdateCount * 0.2f + NPC.whoAmI);
            NPC.scale = 0.8f + pulse;

			// Look at the player unless dashing
			//
			if ((int)NPC.ai[1] != 1)
			{
                LookToPlayer();
            }

            Move();

			// Fix overlap with other npcs
			//
			float overlapVelocity = 0.12f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC other = Main.npc[i];

				if (i != NPC.whoAmI && other.active && Math.Abs(NPC.position.X - other.position.X) + Math.Abs(NPC.position.Y - other.position.Y) < NPC.width * 5)
				{
					if (NPC.position.X < other.position.X)
					{
						NPC.velocity.X -= overlapVelocity;
					}
					else
					{
						NPC.velocity.X += overlapVelocity;
					}

					if (NPC.position.Y < other.position.Y)
					{
						NPC.velocity.Y -= overlapVelocity;
					}
					else
					{
						NPC.velocity.Y += overlapVelocity;
					}
				}
			}
		}

        private void Move()
        {
            Vector2 toPlayer = _player.Center - NPC.Center;
            float distance = toPlayer.Length();

            float baseSpeed = _speed;     // now lower (e.g. 8.5f)
            float dashSpeed = 14f;

            switch ((int)NPC.ai[1])
            {
                case 0: // Normal movement
                    {
                        Vector2 desired = toPlayer.SafeNormalize(Vector2.Zero) * baseSpeed;
                        NPC.velocity = (NPC.velocity * 28f + desired) / 29f;

                        // Trigger pre-dash when close
                        if (distance < 220f && NPC.ai[0] > 80)
                        {
                            NPC.ai[1] = 3; // go to pre-dash
                            NPC.ai[0] = 0;
                        }
                    }
                    break;

                case 3: // Pre-dash (windup)
                    {
                        NPC.velocity *= 0.9f; // slight slowdown

                        // Lock direction once at start
                        if (NPC.ai[0] == 1)
                        {
                            _dashDirection = toPlayer.SafeNormalize(Vector2.UnitY);

                            SoundEngine.PlaySound(
                                SoundID.NPCHit13 with
                                {
                                    Volume = 0.35f,
                                    Pitch = Main.rand.NextFloat(-0.2f, 0.2f)
                                },
                                NPC.Center
                            );
                        }

                        if (NPC.ai[0] > 10)
                        {
                            NPC.ai[1] = 1; // dash
                            NPC.ai[0] = 0;
                        }
                    }
                    break;

                case 1: // Dash (non-tracking)
                    {
                        NPC.velocity = _dashDirection * dashSpeed;

                        if (NPC.ai[0] > 10)
                        {
                            NPC.ai[1] = 2; // slowdown
                            NPC.ai[0] = 0;
                        }
                    }
                    break;

                case 2: // Slowdown / recovery
                    {
                        NPC.velocity *= 0.92f;

                        if (NPC.ai[0] > 50)
                        {
                            NPC.ai[1] = 0; // back to normal
                            NPC.ai[0] = 0;
                        }
                    }
                    break;
            }
        }

        private void Target()
		{
			_player = Main.player[NPC.target]; // This will get the player target.
		}


		private void LookToPlayer()
		{
			Vector2 look = Main.player[NPC.target].Center - NPC.Center;
			LookInDirection(look);
		}

		private void LookInDirection(Vector2 look)
		{
			float angle = 0.5f * (float)Math.PI;

			if (look.X != 0f)
			{
				angle = (float)Math.Atan(look.Y / look.X);
			}

			else if (look.Y < 0f)
			{
				angle += (float)Math.PI;
			}

			if (look.X < 0f)
			{
				angle += (float)Math.PI;
			}

			// Rotate smoothly
            float targetRot = angle;
            NPC.rotation = MathHelper.Lerp(NPC.rotation, targetRot, 0.2f);
        }

		private void DespawnHandler()
		{
			if (!_player.active || _player.dead)
			{
				NPC.TargetClosest(false);
				_player = Main.player[NPC.target];
				if (!_player.active || _player.dead)
				{
					NPC.velocity = new Vector2(0f, -10f);
					if (NPC.timeLeft > 10)
					{
						NPC.timeLeft = 10;
					}
					return;
				}
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default(Color), 1.2f);
				}
			}

			int i = 0;
			while ((double)i < hit.Damage / (double)NPC.lifeMax * 100.0)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default(Color), 1.5f);
				i++;
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // When dashing, draw a red afterimage trail
			//
            if (NPC.ai[1] == 1)
            {
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    Vector2 drawPos = NPC.oldPos[i] - screenPos + NPC.Size / 2f;

                    Color color = new Color(180, 40, 40, 80) *
                                  (1f - i / (float)NPC.oldPos.Length);

                    spriteBatch.Draw(
                        TextureAssets.Npc[NPC.type].Value,
                        drawPos,
                        NPC.frame,
                        color,
                        NPC.rotation,
                        NPC.frame.Size() / 2,
                        NPC.scale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            return true;
        }
    }
}
