﻿using SupernovaMod.Api.Integration.BossChecklist;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems
{
	public class ModIntegrationsSystem : ModSystem
	{
		public override void PostSetupContent()
		{
			HandleIntegrationBossChecklist();

            // Support Wikithis
			//
			if (Supernova.Instance.HasWikiThis)
			{
				Supernova.Instance.wikithis.Call("AddModURL", Mod, "https://terrariamods.wiki.gg/wiki/Supernova_Mod/{}");
			}
        }

        private void HandleIntegrationBossChecklist()
		{
			if (!Supernova.Instance.HasModBossChecklist)
			{
				return;
			}

			// [Pre-Harmode bosses]
			//
			// Harbinger of Annihilation
			//
			new BossChecklistItemBuilder()
				.ForBoss<Content.Npcs.HarbingerOfAnnihilation.HarbingerOfAnnihilation>()
				.SetWeight(VanillaWeights.EyeOfCthulhu + .5f)   // After the Eye of Cthulhu
				.SetAdditionalEntryData(
					new BossChecklistAdditionalEntryDataBuilder()
						.AddSpawnItem<Content.Items.Consumables.EerieCrystal>()
						.AddCollectibleItem<Content.Items.Placeable.Furniture.HarbingerOfAnnihilationTrophy>()
						.AddCollectibleItem<Content.Items.Placeable.Furniture.HarbingerOfAnnihilationRelic>()
				)
				.SetDownedCallback(() => DownedSystem.downedHarbingerOfAnnihilation)
				.AddBoss(Mod, Supernova.Instance.bossChecklist);

			// Flying Terror
			//
			new BossChecklistItemBuilder()
				.ForBoss<Content.Npcs.FlyingTerror.FlyingTerror>()
				.SetWeight(VanillaWeights.QueenBee + .1f)   // Just after the Queen bee
				.SetAdditionalEntryData(
					new BossChecklistAdditionalEntryDataBuilder()
						.AddSpawnItem<Content.Items.Consumables.BugOnAStick>()
						.AddCollectibleItem<Content.Items.Placeable.Furniture.FlyingTerrorRelic>()
						.AddCollectibleItem<Content.Items.Placeable.Furniture.FlyingTerrorTrophy>()
				)
				.SetDownedCallback(() => DownedSystem.downedFlyingTerror)
				.AddBoss(Mod, Supernova.Instance.bossChecklist);

			// [Pre-Harmode mini-bosses]
			//
			// Bloodweaver
			//
			new BossChecklistItemBuilder()
				.ForBoss<Content.Npcs.Bloodmoon.Bloodweaver>()
				.SetWeight(VanillaWeights.BloodMoon + .25f)   // Just after the blood moon
				.SetAdditionalEntryData(
					new BossChecklistAdditionalEntryDataBuilder()
						.AddCollectibleItem<Content.Items.Placeable.Furniture.BloodweaverRelic>()
						.AddCollectibleItem<Content.Items.Placeable.Furniture.BloodweaverTrophy>()
				)
				.SetDownedCallback(() => DownedSystem.downedBloodweaver)
				.AddMiniBoss(Mod, Supernova.Instance.bossChecklist);


            // [Harmode bosses]
            //
            // Cosmic Collective
			//
            new BossChecklistItemBuilder()
				.ForBoss<Content.Npcs.CosmicCollective.CosmicCollective>()
				.SetWeight(VanillaWeights.TheTwins + .5f)
				// TODO: .SetAdditionalEntryData;
				.SetDownedCallback(() => DownedSystem.downedCosmicCollective)
				.AddBoss(Mod, Supernova.Instance.bossChecklist);
            // The Fallen
            //
            //new BossChecklistItemBuilder()
            //    .ForBoss<Content.Npcs.Fallen.Fallen>()
            //    .SetWeight(VanillaWeights.Plantera + .25f)
            //    // TODO: .SetAdditionalEntryData;
            //    .SetDownedCallback(() => DownedSystem.downedFallen)
            //    .AddBoss(Mod, Supernova.Instance.bossChecklist);
        }
        public static void HandleIntegrationRogueCalamity()
		{
			if (!Supernova.Instance.HasModCalamity)
			{
				return;
			}
			// Set the calamity Rogue class as our throwing class
			//
			DamageClass rogue = ModLoader.GetMod("CalamityMod").Find<DamageClass>("RogueDamageClass");
			GlobalModifiers.DamageClass_ThrowingMelee = rogue;
			GlobalModifiers.DamageClass_ThrowingRanged = rogue;
		}
	}

	/// <summary>
	/// Vanilla Boss/Event weights
	/// </summary>
	public static class VanillaWeights
	{
		// Bosses
		public const float KingSlime = 1f;
		public const float EyeOfCthulhu = 2f;
		public const float EaterOfWorlds = 3f;
		public const float QueenBee = 4f;
		public const float Skeletron = 5f;
		public const float DeerClops = 6f;
		public const float WallOfFlesh = 7f;
		public const float QueenSlime = 8f;
		public const float TheTwins = 9f;
		public const float TheDestroyer = 10f;
		public const float SkeletronPrime = 11f;
		public const float Plantera = 12f;
		public const float Golem = 13f;
		public const float DukeFishron = 14f;
		public const float EmpressOfLight = 15f;
		public const float Betsy = 16f;
		public const float LunaticCultist = 17f;
		public const float Moonlord = 18f;

		// Mini-bosses and Events
		public const float TorchGod = 1.5f;
		public const float BloodMoon = 2.5f;
		public const float GoblinArmy = 3.33f;
		public const float OldOnesArmy = 3.66f;
		public const float DarkMage = OldOnesArmy + 0.01f;
		public const float Ogre = SkeletronPrime + 0.01f; // Unlocked once a mechanical boss has been defeated
		public const float FrostLegion = 7.33f;
		public const float PirateInvasion = 7.66f;
		public const float PirateShip = PirateInvasion + 0.01f;
		public const float SolarEclipse = 11.5f;
		public const float PumpkinMoon = 13.25f;
		public const float MourningWood = PumpkinMoon + 0.01f;
		public const float Pumpking = PumpkinMoon + 0.02f;
		public const float FrostMoon = 13.5f;
		public const float Everscream = FrostMoon + 0.01f;
		public const float SantaNK1 = FrostMoon + 0.02f;
		public const float IceQueen = FrostMoon + 0.03f;
		public const float MartianMadness = 13.75f;
		public const float MartianSaucer = MartianMadness + 0.01f;
		public const float LunarEvent = LunaticCultist + 0.01f; // Happens immediately after the defeation of the Lunatic Cultist
	}
}
