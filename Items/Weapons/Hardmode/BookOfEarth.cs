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
            item.damage = 47;  //The damage stat for the Weapon.
            item.crit = 8;
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

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        bool fire;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // Right Click function
            {
                item.crit = 13;
                item.damage = 42;  //The damage stat for the Weapon.
                item.useTime = 10;
                item.useAnimation = 10;
                item.mana = 3; //How mutch mana this weapon use
                item.shootSpeed = 5;
                //item.shoot = mod.ProjectileType("BoEarthPro2");
                if (!fire)
                {
                    item.shoot = mod.ProjectileType("FrostFlame");
                    fire = true;
                }
                else
                {
                    item.shoot = 85;
                    fire = false;
                }
                item.knockBack = 2.5f;
            }
            else // Default Left Click
            {
                item.crit = 16;
                item.damage = 67;  //The damage stat for the Weapon.
                item.useTime = 32;
                item.useAnimation = 32;
                item.mana = 9; //How mutch mana this weapon use
                item.shootSpeed = 12;
                item.shoot = mod.ProjectileType("BookEarthProj");
                item.knockBack = 12;
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("TomeOfIceAndFire"));
            recipe.AddIngredient(mod.GetItem("GraniteStorm"));
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 20);
            recipe.AddIngredient(ItemID.HellstoneBar, 20);
            recipe.AddIngredient(mod.GetItem("ZirconiumBar"), 20);
            recipe.AddIngredient(ItemID.SoulofNight, 9);
            recipe.AddIngredient(ItemID.SoulofLight, 9);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
