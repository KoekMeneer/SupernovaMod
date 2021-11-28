using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Supernova.Npcs.Bosses.CosmicCollective
{
    public class Cosmoling : ModNPC
    {
        private float speed = 6f;
        private Player player;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmoling");
            Main.npcFrameCount[npc.type] = 2;
        }
        int timer;
        int ShootDamage;
        public override void SetDefaults()
        {
            npc.lifeMax = 100;
            npc.defense = 12;
            npc.width = 44;
            npc.height = 44;
            npc.damage = 40;
            npc.knockBackResist = 0.05f;
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true; // Not affected by gravity
        }

        public override void AI()
        {
            timer++;

            LookToPlayer();

            Target(); // Sets the Player Target
            DespawnHandler(); // Handles if the NPC should despawn.

            Move(new Vector2(-0, -0f)); // Calls the Move Method
            if(timer == 180)
            {
                if (speed < 19) speed += 6;
            }
            if (timer >= 245)
            {
                if (speed > 7) speed -= 6;
                else timer = 0;
            }
        }

        private void Move(Vector2 offset)
        {
            Vector2 moveTo = player.Center; // Gets the point that the npc will be moving to.
            Vector2 move = moveTo - npc.Center;
            float magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = 24f; // The larget the number the slower the npc will turn.
            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
                npc.velocity = move;
        }

        private void Target()
        {
            player = Main.player[npc.target]; // This will get the player target.
        }


        private void LookToPlayer()
        {

            Vector2 look = Main.player[npc.target].Center - npc.Center;

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

            npc.rotation = angle;

        }

        private float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }

        private void DespawnHandler()
        {
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
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
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 1;
            npc.frameCounter %= 10;
            int frame = (int)(npc.frameCounter / 2.0);
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
        }
    }
}