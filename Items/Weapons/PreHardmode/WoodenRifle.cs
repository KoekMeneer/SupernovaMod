using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class WoodenRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wood Rifle");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-2, 2);

        public override void SetDefaults()
        {
            item.damage = 8;
            item.ranged = true;
            item.width = 40;
            item.crit = 4;
            item.height = 20;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1.2f;
            item.value = Item.buyPrice(0, 10, 50, 0);
            item.autoReuse = true;
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item11;
            item.shoot = 10; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 12f;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true; // For Ranged Weapon
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("WoodGun"));
            recipe.AddIngredient(ItemID.IronBar, 12);
            recipe.anyIronBar = true;
            recipe.AddIngredient(mod.GetItem("FirearmManual"), 2);
            recipe.AddIngredient(ItemID.Wood, 100);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .12f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
    }
}