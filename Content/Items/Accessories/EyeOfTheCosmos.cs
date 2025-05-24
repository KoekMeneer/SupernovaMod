using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Accessories
{
    public class EyeOfTheCosmos : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(0, 8, 0, 0);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.magicCuffs = true;
            player.statManaMax2 += 20;

            player.manaFlower = true;
            player.manaCost -= .08f;

            SacrificialTalisman.HandleUpdateEffect(player, hideVisual);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ManaFlower);
            recipe.AddIngredient(ItemID.MagicCuffs);
            recipe.AddIngredient<SacrificialTalisman>();
            recipe.AddIngredient<Materials.Starcore>();
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
