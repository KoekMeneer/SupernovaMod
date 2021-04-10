using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.Hardmode
{
    public class TheForbiddenDisc : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Forbidden Disc");
        }

        public override void SetDefaults()
        {
            item.damage = 52;
            item.crit = 4;
            item.thrown = true;
            item.noMelee = true;
            item.maxStack = 1;
            item.width = 23;
            item.height = 23;
            item.useTime = 18;
            item.useAnimation = 18;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 0.1f;
            item.value = Item.buyPrice(0, 37, 14, 38);
            item.rare = Rarity.Pink;
            item.shootSpeed = 17f;
            item.shoot = mod.ProjectileType("ForbiddenDisc");
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(3783, 2);
            recipe.AddIngredient(mod.GetItem("DiscOfTheDesert"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}