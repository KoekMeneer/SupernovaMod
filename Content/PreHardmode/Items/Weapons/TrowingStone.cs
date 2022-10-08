﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class TrowingStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Thrown Stone");
        }

        public override void SetDefaults()
        {   
            Item.maxStack = 999; // Makes it so the weapon stacks.
            Item.damage = 6;
            Item.crit = 4;
            Item.knockBack = 1f;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.width = 30;
            Item.height = 30;
            Item.consumable = true; // Makes it so one is taken from stack after use.
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = 5000;
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 8f;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.StoneProj>();
            Item.ammo = 1;

            Item.DamageType = DamageClass.Throwing;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.StoneBlock, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
