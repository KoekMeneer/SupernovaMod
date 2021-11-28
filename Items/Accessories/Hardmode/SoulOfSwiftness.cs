using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Hardmode
{
    public class SoulOfSwiftness : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul of Swiftness");
            Tooltip.SetDefault("The less health you have the faster you are.");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = Rarity.Pink;
            item.value = Item.buyPrice(0, 8, 36, 0);
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SwiftnessPotion, 4);
            recipe.AddIngredient(ItemID.SoulofLight, 7);
            recipe.AddIngredient(mod.GetItem("HelixStone"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false) => player.moveSpeed += 10 / (player.statLife / 100f);
    }
}
