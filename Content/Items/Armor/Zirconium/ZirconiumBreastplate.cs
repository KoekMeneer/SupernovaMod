using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Armor.Zirconium
{
    // Added instead of AutoLoad
    [AutoloadEquip(EquipType.Body)]
    public class ZirconiumBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.defense = 4;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.ZirconiumBar>(), 32);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
