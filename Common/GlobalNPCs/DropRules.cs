﻿using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ID;
using SupernovaMod.Common.ItemDropRules.DropConditions;
using System;

namespace SupernovaMod.Common.GlobalNPCs
{
    public class DropRules : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			// Drop from any zombie
			//
			if (NPCID.Sets.Zombies[npc.type])
			{
				// 1/7 (14,28571%) Drop chance after the EoC is downed
				//
				npcLoot.Add(GetDropRule<EoCDownedDropCondition>(conditionalRule =>
				{
					conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Materials.BoneFragment>(), 7, maximumDropped: 3));
				}));
			}

			// Drop from any demon
			if (npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.RedDevil)
			{
				// 1/30 (3.3333%) Drop chance
				//
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.DemonHorns>(), 30));
			}

			/* Register Event Handlers */
			npcLoot.Add(GetDropRule<BloodMoonDropCondition>(NPCEventBloodMoonLoot));

			/* Register Biome Handlers */
			npcLoot.Add(GetDropRule<MushroomBiomeDropCondition>(NPCBiomeGlowingMushroomLoot));
			npcLoot.Add(GetDropRule<JungleBiomeDropCondition>(NPCBiomeJungleLoot));
			npcLoot.Add(GetDropRule<SnowBiomeDropCondition>(NPCBiomeSnowLoot));
			npcLoot.Add(GetDropRule<CorruptBiomeDropCondition>(NPCBiomeEvilLoot));
			npcLoot.Add(GetDropRule<CrimsonBiomeDropCondition>(NPCBiomeEvilLoot));
			npcLoot.Add(GetDropRule<CrimsonBiomeDropCondition>(NPCBiomeEvilLoot));

			base.ModifyNPCLoot(npc, npcLoot);
		}

		private IItemDropRule GetDropRule<T>(Action<IItemDropRule> conditionCallback) where T : IItemDropRuleCondition, new()
		{
			T dropCondition = new T();
			IItemDropRule conditionalRule = new LeadingConditionRule(dropCondition);
			conditionCallback(conditionalRule);
			return conditionalRule;
		}

		#region Biome Loot Handlers
		public void NPCBiomeJungleLoot(IItemDropRule conditionalRule)
		{
			// 1/80 (1.25%) Drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Weapons.Magic.StaffOfThorns>(), 80));
		}
		public void NPCBiomeGlowingMushroomLoot(IItemDropRule conditionalRule)
		{
			// 1/50 (2%) Drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.BagOfFungus>(), 50));
		}
		public void NPCBiomeSnowLoot(IItemDropRule conditionalRule)
		{
			// 1/12 (8,33333%) drop chance after the Queen bee is downed
			//
			conditionalRule.OnSuccess(GetDropRule<QueenBeeDownedDropCondition>(conditionalRule =>
			{
				conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Materials.Rime>(), 5, maximumDropped: 2));
			}));
			/*if (NPC.downedQueenBee == true)
			{
				conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.PreHardmode.Items.Materials.Rime>(), 5, maximumDropped: 2));
			}*/
		}
		public void NPCBiomeEvilLoot(IItemDropRule conditionalRule)
		{
			// 1/120 (0.83333%) Drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.SacrificialTalisman>(), 120));
		}
		#endregion

		#region Event Loot Handlers
		public void NPCEventBloodMoonLoot(IItemDropRule conditionalRule)
		{
			// 1/10 (10%) drop chance
			//
			conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Materials.BloodShards>(), 10, maximumDropped: 3));
		}
		#endregion
	}
}
