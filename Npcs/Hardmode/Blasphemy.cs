using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Npcs.Hardmode
{
    public class Blasphemy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blasphemy");
            Main.npcFrameCount[npc.type] = 20;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 400;
            npc.damage = 50;
            npc.defense = 23;
            npc.knockBackResist = 0.3f;
            npc.width = 36;
            npc.height = 44;
            npc.lavaImmune = true;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.aiStyle = 3;
            aiType = NPCID.SkeletonArcher;
            animationType = NPCID.SkeletonArcher;
            npc.value = Item.buyPrice(0, 0, 40, 0);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => Main.hardMode && spawnInfo.player.ZoneUnderworldHeight ? .015f : 0;
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FurOfBlasphemy"), Main.rand.Next(0, 2));
		}
	}
}

