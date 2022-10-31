﻿using Microsoft.Xna.Framework;
using Supernova.Api;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Rings
{
	public class GemRing : SupernovaRing
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Gem Ring");
            Tooltip.SetDefault("Shoots gem staff projeciltes when the 'Ring Ability button' is pressed.");
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
		public override int Cooldown => 1700;
		public override void OnRingActivate(Player player)
		{
            for (int i = 0; i < 3; i++)
			{
                // Add dust effect
                for (int j = 0; j < 5; j++)
                {
                    int dust = Dust.NewDust(player.position, player.width, player.height, DustID.GemDiamond);
                    Main.dust[dust].scale = 1.5f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.5f;
                    Main.dust[dust].velocity *= 1.5f;
                }

                Vector2 position = player.Center;
				SoundEngine.PlaySound(SoundID.Item14, position);
				int ShootDamage = 17;
				int Type = 121;
				float ShootKnockback = 3.6f;

				Vector2 diff = Main.MouseWorld - player.Center;

				diff.Normalize();

				Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 10, diff.Y * 10, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 9.5f, diff.Y * 9.5f, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type1 = 122;
				Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 9, diff.Y * 9, Type1, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type2 = 123;
				Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 8.5f, diff.Y * 8.5f, Type2, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 8, diff.Y * 8, Type2, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type3 = 124;
				Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 7.5f, diff.Y * 7.5f, Type3, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type4 = 125;
				Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 7, diff.Y * 7, Type4, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
				int Type5 = 126;
				Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 6.5f, diff.Y * 6.5f, Type5, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
			}
		}
		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddTile(ModContent.TileType<Global.Tiles.RingForge>());
            recipe.Register();
        }
    }
}