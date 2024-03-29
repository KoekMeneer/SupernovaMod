using log4net;
using SupernovaMod.Common.Systems;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace SupernovaMod
{
    public class Supernova : Mod
	{
		public static ILog Log { get; private set; }
		public override void Load()
		{
			Log = Logger;
		}

		public override void PostSetupContent()
		{
			// Check if the user has downloaded the bossChecklist mod
			//
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				bossChecklist.Call("AddBoss", 1.8f,
					new List<int> { ModContent.NPCType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingerOfAnnihilation>() },
					this,
					"Harbinger of Annihilation",
					() => SupernovaBosses.downedHarbingerOfAnnihilation,
					0,//ModContent.NPCType<Content.Npcs.CosmicAnomaly>(),
					new List<int> {
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersCrest>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersKnell>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersPick>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersSlicer>(),
					},
					new List<int> {
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersCrest>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersKnell>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersPick>(),
						ModContent.ItemType<Content.Npcs.Bosses.HarbingerOfAnnihilation.HarbingersSlicer>(),
					},
					"Kill a Cosmic Anomaly in Space"
				);

				bossChecklist.Call("AddBoss", 3.7f,
					new List<int> { ModContent.NPCType<Content.Npcs.Bosses.FlyingTerror.FlyingTerror>() },
					this,
					"Flying Terror",
					() => SupernovaBosses.downedFlyingTerror,
					ModContent.ItemType<Content.Items.Misc.HorridChunk>(),
					new List<int> {
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.FlyingTerrorBag>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorTuft>()
					},
					new List<int> {
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.FlyingTerrorBag>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorInABottle>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.BlunderBuss>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorCleaver>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorKnife>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorRecurve>(),
						ModContent.ItemType<Content.Npcs.Bosses.FlyingTerror.TerrorTome>(),
					},
					"Use a [i:" + ModContent.ItemType<Content.Items.Misc.HorridChunk>() + "] at night"
				);
			}


			base.PostSetupContent();
		}
	}
}