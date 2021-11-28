using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;


namespace Supernova
{
	public class SupernovaWorld : ModWorld
	{
        /* World Generation */
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
            // [Ore Generation]
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Supernova Ores", delegate (GenerationProgress progress)
                {
                    progress.Message = "Generating Supernova Ores";
                    for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 8E-05); i++)
                    {
                        WorldGen.TileRunner(
                            WorldGen.genRand.Next(0, Main.maxTilesX),
                            WorldGen.genRand.Next((int)WorldGen.worldSurfaceHigh, Main.maxTilesY),
                            (double)WorldGen.genRand.Next(5, 8),
                            WorldGen.genRand.Next(3, 9),
                            mod.TileType("ZirconiumOreTile"), false, 0f, 0f, false, true);
                    }
                }));
            }
        }

        /* Bosses Downed */
        // PreHardmode
        public static bool downedHarbingerOfAnnihilation = false;
        public static bool downedFlyingTerror = false;
        public static bool downedStoneManta = false;
        // Hardmode
        public static bool downedCosmicCollective = false;
        public override void Initialize()
        {
            downedHarbingerOfAnnihilation = false;
            downedFlyingTerror = false;
            downedStoneManta = false;
            downedCosmicCollective = false;
        }
        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedHarbingerOfAnnihilation) downed.Add("Harbinger of Annihilation");
            if (downedFlyingTerror) downed.Add("FlyingTerror");
            if (downedStoneManta) downed.Add("Stone MantaRay");
            if (downedCosmicCollective) downed.Add("Cosmic Collective");
            return new TagCompound
            {
                {"downed", downed }
            };
        }

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedHarbingerOfAnnihilation = downed.Contains("Harbinger of Annihilation");
            downedFlyingTerror = downed.Contains("FlyingTerror");
            downedStoneManta = downed.Contains("Stone MantaRay");
            downedCosmicCollective = downed.Contains("Cosmic Collective");
        }

        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            if (loadVersion == 0)
            {
                BitsByte flags = reader.ReadByte();
                downedHarbingerOfAnnihilation = flags[0];
                downedFlyingTerror = flags[0];
                downedStoneManta = flags[0];
                downedCosmicCollective = flags[0];
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedHarbingerOfAnnihilation;
            flags[0] = downedFlyingTerror;
            flags[0] = downedStoneManta;
            flags[0] = downedCosmicCollective;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedHarbingerOfAnnihilation = flags[0];
            downedFlyingTerror = flags[0];
            downedStoneManta = flags[0];
            downedCosmicCollective = flags[0];
        }
    }
}
