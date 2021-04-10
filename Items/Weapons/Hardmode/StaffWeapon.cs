using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.Hardmode
{
    public class StaffWeapon : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Staff Weapon");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 70;
            item.crit = 24;
            item.ranged = true;          //this make the item do magic damage
            item.width = 28;
            item.height = 34;
            item.useTime = 19;
            item.useAnimation = 19;
            item.useStyle = 5;        //this is how the item is holded
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 1000;
            item.rare = 6;
            item.UseSound = SoundID.Item105;            //this is the sound when you use the item
            item.shoot = 122;
            item.shootSpeed = 27f;    //projectile speed when shoot
			item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(3783);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}