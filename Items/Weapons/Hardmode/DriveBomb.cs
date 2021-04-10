using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.Hardmode
{
    public class DriveBomb : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drive Bomb");
        }

        public override void SetDefaults()
        {
            item.damage = 57;
            item.crit = 4;
            item.thrown = true;
            item.noMelee = true;
            item.maxStack = 1;
            item.width = 30;
            item.height = 30;
            item.useTime = 42;
            item.useAnimation = 42;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 7f;
            item.value = Item.buyPrice(0, 40, 0, 0);
            item.rare = Rarity.Pink;
            item.shootSpeed = 16f;
            item.shoot = mod.ProjectileType("DriveBombProj");
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MechroDrive"), 7);
            recipe.AddIngredient(ItemID.SoulofMight, 3);
            recipe.AddIngredient(ItemID.SoulofFright, 3);
            recipe.AddIngredient(ItemID.Bomb, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}