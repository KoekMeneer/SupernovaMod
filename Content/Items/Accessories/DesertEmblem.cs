﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class DesertEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Desert Emblem");
            Tooltip.SetDefault("8% increased movementspeed\nIn a sandstorm 16% increased movementspeed");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 2, 67, 80);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            if (player.ZoneSandstorm == true)
            {
                player.statDefense += 3;
                player.moveSpeed += 16;
            }
            player.moveSpeed += 8;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Amber);
            recipe.AddIngredient(ItemID.AntlionMandible, 2);
            recipe.AddIngredient(ItemID.SandBlock, 6);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
