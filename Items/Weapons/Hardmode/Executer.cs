using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.Hardmode
{
    public class Executer : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Executer");

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 3);
        }

        public override void SetDefaults()
        {
            item.damage = 33;
            item.ranged = true;
            item.width = 40;
            item.crit = 4;
            item.height = 20;
            item.useTime = 56;
            item.useAnimation = 56;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 5.4f;
            item.value = 10000;
            item.autoReuse = true;
            item.rare = 2;
            item.UseSound = SoundID.Item38;
            item.shoot = 10;
            item.shootSpeed = 5;
            item.useAmmo = AmmoID.Bullet;
            item.scale *= .7f;
            item.ranged = true; // For Ranged Weapon
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = Calc.RandomSpread(speedX, speedY, 8, .014f, 6);
            for (int i = 0; i < 5; ++i)
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X - Main.rand.NextFloat(-.25f, .5f), speeds[i].Y - Main.rand.NextFloat(-.25f, .5f), type, damage, knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Boomstick);
            recipe.AddIngredient(ItemID.Shotgun);
            recipe.AddIngredient(mod.GetItem("HiTechFirearmManual"));
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}