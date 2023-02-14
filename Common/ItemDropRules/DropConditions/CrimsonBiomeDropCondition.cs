﻿using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace Supernova.Common.ItemDropRules.DropConditions
{
	internal class CrimsonBiomeDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return Main.player[Player.FindClosest(info.npc.position, info.npc.width, info.npc.height)].ZoneCrimson;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return false;
		}

		public string GetConditionDescription()
		{
			return "Drops in the Crimson biome";
		}
	}
}
