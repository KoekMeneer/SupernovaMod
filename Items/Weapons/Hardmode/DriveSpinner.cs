using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.Hardmode
{
    public class DriveSpinner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drive Spinner");
            Tooltip.SetDefault("Shoots a laser at your enemies!");
        }

        public override void SetDefaults()
        {
            item.knockBack = 3.4f;
            item.damage = 48;
            item.crit = 7;
            item.useStyle = 5; // The style used for YoYos.
            item.width = 24;
            item.height = 24;
            item.noUseGraphic = true; // Doesn't show Item in Hand.
            item.melee = true; // YoYos are a melee item.
            item.noMelee = true; // Don't damage enemies with the hand hitbox.
            item.channel = true; // ???
            item.UseSound = SoundID.Item1;
            item.useAnimation = 56;
            item.useTime = 56;
            item.shoot = mod.ProjectileType("DriveSpinnerProj"); // Projectile that is used with this weapon.
            item.shootSpeed = 23f; // How fast the projectile is shot.
            item.value = Item.buyPrice(0, 40, 0, 0);
            item.rare = Rarity.Pink;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MechroDrive"), 7);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddIngredient(ItemID.WhiteString);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
