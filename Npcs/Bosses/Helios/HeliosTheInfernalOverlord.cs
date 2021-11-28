using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Supernova.Npcs.Bosses.Helios
{
	[AutoloadBossHead]
	public class HeliosTheInfernalOverlord : Boss
    {
        /* Stats */
        public int smallAttackDamage = 32;
        public int largeAttackDamage = 45;
        const float ShootKnockback = 5f;
        const int ShootDirection = 7;

        /* Stage Attacks */
        public string[] stage0 = new string[] { "atkFireBall", "atkFireBall", "atkChestFireBurst" };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helios the Infernal Overlord");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            targetOffset = new Vector2(0, -80);
            velMax = 1;
            velAccel = .05f;

            attackPointer = 0;
            attacks = stage0;

            npc.aiStyle = -1;
            npc.lifeMax = 26000;
            npc.damage = 42;
            npc.defense = 14;
            npc.knockBackResist = 0f; // No knockback
            npc.width = 150;
            npc.height = 300;
            npc.value = Item.buyPrice(0, 3, 46, 82);
            npc.npcSlots = 1; // The higher the number, the more NPC slots this NPC takes.
            npc.boss = true; // Is a boss
            npc.lavaImmune = false; // Not hurt by lava
            npc.noGravity = true; // Not affected by gravity
            npc.noTileCollide = true; // Will not collide with the tiles. 
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath1;
            music = MusicID.Boss1;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.ShadowFlame] = true;
            bossBag = mod.ItemType("FlyingTerrorBag"); // Needed for the NPC to drop loot bag.
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * bossLifeScale);
            npc.defense = (int)(npc.defense + numPlayers);
        }
		public override void NPCLoot()
		{
            // Drop the correct drops
            if (Main.expertMode)
                npc.DropBossBags();
            else
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TerrorWing"), 2);

            // For settings if the boss has been downed
            SupernovaWorld.downedFlyingTerror = true;
        }

		bool init = false;
        public override void AI()
		{
            // Set target
            if (init == false)
            {
                attacks = stage0;
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

            // Add light to the boss
            //Lighting.AddLight(npc.Center, 183, 0, 255);
        }
        #region Attacks
        public void atkFireBall()
		{
            npc.ai[0]++;
            if (npc.ai[0] > 80 && npc.ai[0] % 20 == 0 && npc.ai[0] < 240)
            {
                float Speed = 27f;  //projectile speed

                int damage = 24;  //projectile damage
                int type = 327;
                Main.PlaySound(SoundID.Item20, npc.Center);

                // Shoot right flame
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 4), npc.position.Y + (-npc.height + 200));
                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.2f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.2f)));
                Projectile.NewProjectile(vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, damage, 0f, 0);

                // Shoot left falme
                vector8 = new Vector2(npc.position.X + (npc.width - 50), npc.position.Y + (-npc.height + 200));

                rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));

                Projectile.NewProjectile(vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * Speed)), (float)(-(Math.Sin(rotation) * Speed)), type, damage, 0f, 0);
            }
            else if(npc.ai[0] > 300)
			{
                npc.ai[0] = 0;
                attackPointer++;
            }
        }
        public void atkChestFireBurst()
		{
            npc.ai[0]++;
            if (npc.ai[0] == 120)
            {
                float Speed = 10;  //projectile speed

                int type = 292;  //put your projectile
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.5f)), vector8.X - (targetPlayer.position.X + (targetPlayer.width * 0.5f)));
                Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -2), (float)((Math.Sin(rotation) * Speed) * -1), type, largeAttackDamage, 0f, 0);
            }
            else if (npc.ai[0] >= 240)
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
            npc.frameCounter += 1;
            npc.frameCounter %= 20;
            int frame = (int)(npc.frameCounter / 10.0);
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
        }
        private void DespawnHandler()
        {
            if (!targetPlayer.active || targetPlayer.dead || !targetPlayer.ZoneUnderworldHeight)
            {
                npc.TargetClosest(false);
                targetPlayer = Main.player[npc.target];
                if (!targetPlayer.active || targetPlayer.dead || !targetPlayer.ZoneUnderworldHeight)
                {
                    npc.velocity = new Vector2(0f, 15f);
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
            // Stages
            /*if (npc.life <= (npc.lifeMax * .3f))
                attacks = stage4;

            else if (npc.life <= (npc.lifeMax * 50f))
                attacks = stage3;

            else if (npc.life <= (npc.lifeMax * .70))
                attacks = stage2;

            else if (npc.life <= (npc.lifeMax * .9f))
                attacks = stage1;*/

            // Reset attack pointer when we have done all the attacks for this stage
            if (attackPointer >= attacks.Length) attackPointer = 0;

            return true;
        }
    }
}
