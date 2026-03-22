using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Rings
{
    public class RingOfHellfire : SupernovaRingItem
    {
        public override int BaseCooldown => 60 * 200;
        public override int UseTime => 40;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
			base.SetDefaults();
			Item.width = 32;
            Item.height = 22;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 6, 0, 0);
        }

        public override void OnUseFrame(Player player, int frame)
        {
            Vector2 pos = player.Center + Main.rand.NextVector2Circular(30, 30);
            Dust.NewDustPerfect(pos, DustID.Lava, (player.Center - pos) * 0.2f).noGravity = true;

            RingVFX.PlayChargeSound(player.Center);
        }

        public override void OnActivate(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item74);

            int duration = 60 * 30;

            player.AddBuff(BuffID.Inferno, duration);
            player.AddBuff(ModContent.BuffType<Buffs.Rings.HellfireRingBuff>(), duration);

            for (int i = 0; i < 25; i++)
            {
                Dust.NewDustDirect(player.position, player.width, player.height, DustID.Torch, Scale: 2f).noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}
