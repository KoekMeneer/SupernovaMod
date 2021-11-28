using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Hardmode
{
    public class IronSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("IronSkull");
            Tooltip.SetDefault("The less health you have the more defence you have");
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
            recipe.AddIngredient(ItemID.SoulofNight, 7);
            recipe.AddIngredient(ItemID.IronBar, 5);
            recipe.AddIngredient(ItemID.Bone, 8);
            recipe.anyIronBar = true;
            recipe.AddIngredient(mod.GetItem("HelixStone"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false) => player.statDefense += (int)(7 / (player.statLife / 100f));
    }
}
