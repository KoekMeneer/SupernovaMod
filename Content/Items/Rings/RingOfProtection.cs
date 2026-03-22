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
    public class RingOfProtection : SupernovaRingItem
    {
        public override int BaseCooldown => 35 * 60;

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
            float rot = MathHelper.ToRadians(frame * 10);
            Vector2 pos = player.Center + new Vector2(20f, 0).RotatedBy(rot);

            Dust.NewDustPerfect(pos, DustID.Lead, Vector2.Zero, 0, default, 1.1f).noGravity = true;
            Dust.NewDustPerfect(pos, DustID.SilverCoin, Vector2.Zero, 0, default, 0.9f).noGravity = true;

            RingVFX.PlayChargeSound(player.Center);
        }

        public override void OnActivate(Player player)
        {
            // TODO: Add some kind of shield effect instead

            SoundEngine.PlaySound(SoundID.Item29);

            player.AddBuff(BuffID.ShadowDodge, 60 * 30);
            player.ShadowDodge();

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustDirect(player.position, player.width, player.height, DustID.SilverCoin, Scale: 1.4f).noGravity = true;
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ResourcePlayer resourcePlayer = player.GetModPlayer<ResourcePlayer>();
            player.statDefense += (int)(2 * resourcePlayer.ringPower);
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.IronskinPotion);
            recipe.AddIngredient(ItemID.SilverBar, 4);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.IronskinPotion);
            recipe.AddIngredient(ItemID.TungstenBar, 4);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}