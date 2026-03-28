using Microsoft.Xna.Framework;
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
        public override int BaseCooldown => 1800;
        public override int UseTime => 30;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
			base.SetDefaults();
			Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }

        public override void OnUseFrame(Player player, int frame)
        {
            SoundEngine.PlaySound(SoundID.CoinPickup with { Volume = 0.3f });

            Vector2 pos = player.Center + Main.rand.NextVector2Circular(25, 25);
            Dust.NewDustPerfect(pos, DustID.Gold, Vector2.Zero).noGravity = true;
        }

        public override void OnActivate(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item73);

            player.AddBuff(BuffID.Spelunker, 60 * 12);
            player.AddBuff(BuffID.Mining, 60 * 12);

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustDirect(player.position, player.width, player.height, DustID.Gold, Scale: 1.5f).noGravity = true;
            }
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