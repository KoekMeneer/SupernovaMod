﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;

namespace SupernovaMod.Content.Npcs
{
    public class TerrorBat : ModNPC // ModNPC is used for Custom NPCs
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Bat");
            Main.npcFrameCount[NPC.type] = 2;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Small but ferocious beastlings seen wandering the underground. Formed in the thick of pitch-black darkness under their master's influence."),
            });
        }


        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 20;
            NPC.defense = 13;
            NPC.lifeMax = 47;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 100f;
            NPC.knockBackResist = 1f;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = false; // Will not collide with the tiles.

            NPC.aiStyle = NPCAIStyleID.Bat;
            AIType = NPCID.CaveBat; // Will not have any AI from any existing AI styles. 
            AnimationType = NPCID.CaveBat;

            NPC.rarity = 3; // For Lifeform Analyzer
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter -= .9F; // Determines the animation speed. Higher value = faster animation.
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;

            NPC.spriteDirection = NPC.direction;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => !spawnInfo.Lihzahrd && !spawnInfo.Invasion && !spawnInfo.SpiderCave && !spawnInfo.DesertCave && !spawnInfo.Player.ZoneDungeon && spawnInfo.Player.ZoneRockLayerHeight == true ? 0.025f : 0;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // 15% Drop chance
            //
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.HorridChunk>(), 7));
            base.ModifyNPCLoot(npcLoot);
        }

        public override void OnKill()
        {
            SoundEngine.PlaySound(SoundID.NPCDeath4);
            base.OnKill();
        }
    }
}
