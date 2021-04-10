using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.Hardmode
{
    public class BlackRoseBurst : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Rose Burst");

            //Tooltip.SetDefault("Shoots a lightning blot tat on hit shoots an other lighting bolt, creating chain lightning");
        }
        public override void SetDefaults()
        {
            item.damage = 43;  //The damage stat for the Weapon.
            item.crit = 4;
            item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            item.noUseGraphic = false;
            item.magic = true;    //This defines if it does magic damage and if its effected by magic increasing Armor/Accessories.
            item.channel = true;                            //Channel so that you can held the weapon
            item.rare = Rarity.LigtRed;   //The color the title of your Weapon when hovering over it ingame
            item.width = 28;   //The size of the width of the hitbox in pixels.
            item.height = 30;    //The size of the height of the hitbox in pixels.
            item.UseSound = SoundID.Item17;

            item.useAnimation = 16;
            item.useTime = 4;
            item.reuseDelay = 23;

            item.shootSpeed = 5;
            item.mana = 6;
            item.useStyle = 5;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            item.value = Item.sellPrice(0, 12, 4, 34);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("RoseBurst");//660
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodRose"), 7);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
