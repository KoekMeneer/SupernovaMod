﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class TomeOfIceAndFire : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Frost and Fire");

            Tooltip.SetDefault("Shoots fire and frostburn");
        }
        int _leftClick;
        public override void SetDefaults()
        {
            item.damage = 22;  //The damage stat for the Weapon.
            item.crit = 3;
            item.knockBack = 2;
            item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            item.noUseGraphic = false;
            item.magic = true;    //This defines if it does magic damage and if its effected by magic increasing Armor/Accessories.
            item.channel = true;                            //Channel so that you can held the weapon
            item.rare = Rarity.Orange;   //The color the title of your Weapon when hovering over it ingame
            item.width = 28;   //The size of the width of the hitbox in pixels.
            item.height = 30;    //The size of the height of the hitbox in pixels.
            item.UseSound = SoundID.Item20;
            item.useTime = 19;
            item.useAnimation = 19;
            item.shootSpeed = 4.25f;
            item.mana = 9;
            item.useStyle = 5;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            item.value = Item.sellPrice(0, 10, 0, 0);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
            item.autoReuse = true;

            if (_leftClick == 0)
            {
                item.shoot = mod.ProjectileType("FrostFlame");
                _leftClick = 1;
            }
            else
            {
                item.shoot = 85;
                _leftClick = 0;
            }
        }
        public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * 0.0344532925);
            float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = System.Math.Atan2(speedX, speedY);
            double randomAngle;
            for (int i = 0; i < num; ++i)
            {
                randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
            }
            return (Vector2[])posArray;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = randomSpread(speedX, speedY, 8, 6);
            for (int i = 0; i < 5; ++i)
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            return false;
        }


        public override bool CanUseItem(Player player)
        {

            if (_leftClick == 0)
            {
                item.shoot = mod.ProjectileType("FrostFlame");
                _leftClick = 1;
            }
            else
            {
                item.shoot = 85;
                _leftClick = 0;
            }
            return base.CanUseItem(player);
        }
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			base.OnHitNPC(player, target, damage, knockBack, crit);
		}
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Book);
            recipe.AddIngredient(mod.GetItem("BlazeBolt"));
            recipe.AddIngredient(mod.GetItem("IceBolt"));
            recipe.AddIngredient(ItemID.Bone, 32);
            recipe.AddIngredient(ItemID.Book);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
