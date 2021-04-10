using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class HiTechFirearmManual : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hi Tech Firearm Manual");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 20;
            item.maxStack = 32;
            item.autoReuse = true;
            item.useTime = 10;
            item.useStyle = 1;
            item.rare = Rarity.LigtRed;
            item.value = Item.buyPrice(0, 3);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("FirearmManual"));
            recipe.AddIngredient(mod.GetItem("MechroDrive"), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
