using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class Starcore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starcore");
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofSight);
            item.width = refItem.width;
            item.height = refItem.height;
            item.rare = Rarity.Pink;
            item.value = Item.buyPrice(0, 7, 0, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Heliolite"), 5);
            recipe.AddIngredient(mod.GetItem("CosmicScales"), 5);
            recipe.AddIngredient(ItemID.SoulofSight);
            recipe.AddIngredient(ItemID.SoulofFright);
            recipe.AddIngredient(ItemID.SoulofMight);
            recipe.AddIngredient(ItemID.SoulofLight, 9);
            recipe.AddIngredient(ItemID.SoulofNight, 9);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
