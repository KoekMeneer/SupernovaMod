using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class TitaniumRingMold : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titanium Ring Mold");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.rare = Rarity.Orange;
            item.value = Item.buyPrice(0, 2);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 8);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 8);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
