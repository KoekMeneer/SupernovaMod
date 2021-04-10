using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace Supernova.Items.Weapons.Hardmode
{
    public class RagnaMagnitrix : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RagnaMagnitrix");
            Tooltip.SetDefault("Shoots a Ragna Bow");
        }
        int C = 0;
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override void SetDefaults()
        {

            item.damage = 82;
            item.autoReuse = true;
            item.crit = 12;
            item.width = 16;
            item.height = 24;
            item.useStyle = 5; // Bow Use Style
            item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            item.value = Item.buyPrice(0, 32, 0, 0); // Another way to handle value of item.
            item.rare = 2;
            item.UseSound = SoundID.Item5; // Sound for Bows
            //item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            item.shoot = mod.ProjectileType("RagnaBowProj");
            item.shootSpeed = 1.2f;
            item.ranged = true; // For Ranged Weapon
            item.useTime = 17;
            item.useAnimation = 17;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Magnitrix"));
            recipe.AddIngredient(mod.GetItem("Starcore"));
            recipe.AddIngredient(ItemID.WoodenBow);
            recipe.AddIngredient(ItemID.BorealWoodBow);
            recipe.AddIngredient(ItemID.PalmWoodBow);
            recipe.AddIngredient(ItemID.RichMahoganyBow);
            recipe.AddIngredient(ItemID.PearlwoodBow);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
