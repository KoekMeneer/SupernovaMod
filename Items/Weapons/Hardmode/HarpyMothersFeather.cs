using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.Hardmode
{
    public class HarpyMothersFeather : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harpy Mothers Feather");
        }

        public override void SetDefaults()
        {
            item.damage = 38;
            item.crit = 7;
            item.thrown = true;
            item.noMelee = true;
            item.maxStack = 1;
            item.width = 30;
            item.height = 30;
            item.useTime = 14;
            item.useAnimation = 14;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 1.2f;
            item.value = Item.buyPrice(0, 3, 46, 82);
            item.rare = Rarity.Orange;
            item.shootSpeed = 5;
            item.shoot = mod.ProjectileType("HarpyFeatherProj");
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GiantHarpyFeather);
            recipe.AddIngredient(ItemID.Feather, 20);
            recipe.AddIngredient(ItemID.SoulofFlight, 5);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}