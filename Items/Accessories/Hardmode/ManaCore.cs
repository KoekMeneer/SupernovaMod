using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Hardmode
{
    public class ManaCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Core");
            Tooltip.SetDefault("The more health you have the faster you regen mana.");
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
            recipe.AddIngredient(ItemID.SoulofLight, 7);
            recipe.AddIngredient(ItemID.ManaCrystal, 4);
            recipe.AddIngredient(mod.GetItem("HelixStone"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.statManaMax2 += 50;
            player.manaRegen += (int)(player.statLife / 100f);
        }
    }
}
