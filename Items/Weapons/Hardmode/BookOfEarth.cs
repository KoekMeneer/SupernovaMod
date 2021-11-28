using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.Hardmode
{
    public class BookOfEarth : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Book of Earth");
        }

        public override void SetDefaults()
        {
            item.crit = 7;
            item.damage = 52;  //The damage stat for the Weapon.
            item.useTime = 28;
            item.useAnimation = 28;
            item.mana = 9; //How mutch mana this weapon use
            item.shootSpeed = 12;
            item.knockBack = 6;
            item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            item.noUseGraphic = false;
            item.magic = true;    //This defines if it does magic damage and if its effected by magic increasing Armor/Accessories.
            item.channel = true;                            //Channel so that you can held the weapon
            item.rare = 3;   //The color the title of your Weapon when hovering over it ingame
            item.width = 28;   //The size of the width of the hitbox in pixels.
            item.height = 30;    //The size of the height of the hitbox in pixels.
            item.autoReuse = true;
            item.useStyle = 5;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            item.UseSound = SoundID.Item20;
            item.shoot = mod.ProjectileType("BookEarthProj");
            item.value = Item.sellPrice(0, 30, 80, 64);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddIngredient(mod.GetItem("TomeOfIceAndFire"));
            recipe.AddIngredient(mod.GetItem("GraniteStorm"));
            recipe.AddIngredient(ItemID.SoulofNight, 9);
            recipe.AddIngredient(ItemID.SoulofLight, 9);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
