﻿using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Rings
{
    public class ProspectorsRing : SupernovaRingItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Prospectors Ring");
            Tooltip.SetDefault("Gives spelunker effect and faster mining speed for 12 seconds when '{0}' is pressed.");

			base.SetStaticDefaults();
		}
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.accessory = true;
        }
        public override int Cooldown => 1800;
        public override void RingActivate(Player player)
        {
            player.AddBuff(BuffID.Spelunker, 720);

            // Add dust effect
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Gold);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.5f;
                Main.dust[dust].velocity *= 1.5f;
            }
            SoundEngine.PlaySound(SoundID.Item73);
        }
        public override void OnRingCooldown(int curentCooldown, Player player)
        {
            // Only run the first 12 seconds (720ms / 60 = 12sec)
            //
            if (curentCooldown >= Cooldown * RingPlayer.ringCooldownMulti - 720)
                player.pickSpeed -= .5f;
        }

        public override int MaxAnimationFrames => 30;
        public override void RingUseAnimation(Player player)
        {
            SoundEngine.PlaySound(SoundID.CoinPickup);

            Vector2 dustPos = player.Center + new Vector2(23, 0).RotatedByRandom(MathHelper.ToRadians(360));
            Vector2 diff = player.Center - dustPos;
            diff.Normalize();

            Dust.NewDustPerfect(dustPos, DustID.Gold, diff * 2).noGravity = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.SpelunkerPotion, 2);
            recipe.AddIngredient(ItemID.GoldOre, 4);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.SpelunkerPotion, 2);
            recipe.AddIngredient(ItemID.PlatinumOre, 4);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}