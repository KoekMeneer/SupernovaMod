using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Rings
{
    public class RingOfLight : RingBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ring of Light");
            Tooltip.SetDefault("When the 'Ring Ability button' is pressed" +
                "\nYou will have no mana costs for 4 seconds" + RingBase.RING_HELP);
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = 3;
            item.value = Item.buyPrice(0, 24, 0, 0);
        }
		public override int cooldown => 3000;

		public override void OnRingCooldown(int curentCooldown, Player player)
		{
            if (curentCooldown >= ((buffCooldown * SupernovaPlayer.ringCooldownDecrease) - 240))
                player.manaCost = 0;
            else player.manaCost = 1;
        }
        /*public override void AddRecipes()
        {
	        ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(mod.GetItem("GoldenRingMold"));

	        recipe.AddIngredient(ItemID.HellstoneBar, 5);

	        recipe.AddTile(mod.GetTile("RingForge"));
	        recipe.SetResult(this);
	        recipe.AddRecipe();
        }*/
    }
}
