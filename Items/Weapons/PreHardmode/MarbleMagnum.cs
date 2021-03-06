using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class MarbleMagnum : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Marble Magnum");
            Tooltip.SetDefault("Fires a dense spread of bullets");
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);


        public override void SetDefaults()
        {
            item.damage = 7;
            item.ranged = true;
            item.width = 40;
            item.crit = 3;
            item.height = 20;
            item.useAnimation = 68;
            item.useTime = 68;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 5;
            item.value = Item.buyPrice(0, 6, 23);
            item.autoReuse = true;
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item38;
            item.shoot = 10; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 8f;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true; // For Ranged Weapon

            item.scale = .8f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = Calc.RandomSpread(speedX, speedY, 8, 0.0025f, 6);
            for (int i = 0; i < Main.rand.Next(3, 6); ++i)
            {
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SandBlock, 12);
            recipe.AddIngredient(ItemID.MarbleBlock, 37);
            recipe.AddIngredient(ItemID.IronBar, 7);
            recipe.AddIngredient(mod.GetItem("FirearmManual"), 2);
            recipe.anyIronBar = true;
            recipe.anySand = true;
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}