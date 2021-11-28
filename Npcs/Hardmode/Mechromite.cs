using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Npcs.Hardmode
{
    public class Mechromite : ModNPC // ModNPC is used for Custom NPCs
    {
        private Player player;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechromite");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.damage = 33;
            npc.defense = 11;
            npc.lifeMax = 230;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 7000f;
            npc.knockBackResist = .25f;
            npc.aiStyle = 3;
            aiType = NPCID.ArmoredSkeleton;  //npc behavior
            animationType = NPCID.Harpy;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter -= .6F; // Determines the animation speed. Higher value = faster animation.
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;

            npc.spriteDirection = -npc.direction;
        }

        public override void AI()
        {
            Target(); // Sets the Player Target
        }

        private void Target()
        {
            player = Main.player[npc.target]; // This will get the player target.
        }

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
            target.AddBuff(BuffID.Electrified, 60);
			base.OnHitPlayer(target, damage, crit);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => (Main.hardMode && !spawnInfo.lihzahrd && !spawnInfo.invasion && !spawnInfo.player.ZoneDungeon) && Main.dayTime && spawnInfo.player.ZoneOverworldHeight ? 0.08f : 0f; //100f is the spown rate so If you want your NPC to be rarer just change that value less the 100f or something.

        public override void NPCLoot()
        {
            if (Main.rand.NextFloat() <= .4f) // .4f = 40%
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MechroDrive"));
        }
    }
}
