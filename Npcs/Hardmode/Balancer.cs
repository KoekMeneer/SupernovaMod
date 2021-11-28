using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Npcs.Hardmode
{
    public class Balancer : ModNPC // ModNPC is used for Custom NPCs 
    {
        private float speed;
        private Player player;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Balancer");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 70;
            npc.damage = 62;
            npc.defense = 60;
            npc.lifeMax = 450;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 100f;
            npc.knockBackResist = 0f;
            npc.noGravity = true; // Not affected by gravity 
            npc.noTileCollide = true; // Will not collide with the tiles. 
            npc.aiStyle = -1; // Will not have any AI from any existing AI styles.  
        }

        bool awaitBuffer = false;
        public override void AI()
        {
            Target(); // Sets the Player Target 

            // Spawn new orbit projectiles if we have less than 4 
            if (npc.ai[0] < 4)
            {
                if (!awaitBuffer)
                {
                    npc.ai[1]++;
                    if (npc.ai[1] >= 90)
                    {
                        Projectile.NewProjectile(npc.position, Vector2.One, mod.ProjectileType("BalancedOrb"), npc.damage / 4, 4, Main.myPlayer, ai1: npc.whoAmI);
                        Main.PlaySound(SoundID.DD2_DarkMageAttack);
                        npc.ai[1] = 0;
                        npc.ai[0]++;    // Add a proj to the proj counter 
                        if (npc.ai[0] >= 4) awaitBuffer = true;
                    }
                }
                else if (npc.ai[0] == 0) awaitBuffer = false;
            }

            DespawnHandler(); // Handles if the NPC should despawn. 

            Move(new Vector2(-0, -0f)); // Calls the Move Method 
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
        private void Move(Vector2 offset)
        {
            speed = 0.9f; // Sets the max speed of the npc. 
            if (Main.netMode != 1)
            {
                Vector2 moveTo = player.Center + offset; // Gets the point that the npc will be moving to. 
                Vector2 move = moveTo - npc.Center;

                float magnitude = Magnitude(move);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                float turnResistance = 21f; // The larget the number the slower the npc will turn. 
                move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
                magnitude = Magnitude(move);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                npc.velocity = move;
            }
        }
        private void Target()
        {
            player = Main.player[npc.target]; // This will get the player target. 
        }
        private float Magnitude(Vector2 mag) => (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 1;
            npc.frameCounter %= 20;
            int frame = (int)(npc.frameCounter / 6.7);
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
            npc.spriteDirection = npc.direction;
        }
        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HelixStone"));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.player.ZoneRockLayerHeight == true && Main.hardMode == true ? 0.0048f : 0;
    }
}