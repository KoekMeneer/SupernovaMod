using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using SupernovaMod.Content.Items.Rings.BaseRings;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Rings
{
    public class RingOfProtection : SupernovaRingItem
    {
        public override int BaseCooldown => 40 * 60;
        public override int UseTime => 10;

        public virtual string? FlavorTextTag => "Mods.SupernovaMod.Items.RingOfProtection.FlavorText";

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
			base.SetDefaults();
			Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.White;
            Item.value = 0;
        }

        public override void OnUseFrame(Player player, int frame)
        {
            float rot = MathHelper.ToRadians(frame * 20);
            Vector2 pos = player.Center + new Vector2(20f, 0).RotatedBy(rot);

            Dust.NewDustPerfect(pos, DustID.Platinum, Vector2.Zero, 0, default, 1.1f).noGravity = true;
            Dust.NewDustPerfect(pos, DustID.SilverCoin, Vector2.Zero, 0, default, 0.9f).noGravity = true;

            RingVFX.PlayChargeSound(player.Center);
        }

        public override void OnActivate(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item29);

            // Prepare shield stats
            int hits = 1;
            int timeLeft = 60 * 10; // 10 seconds
            ModifyShieldStats(ref hits, ref timeLeft);

            //
            SupernovaPlayer supernovaPlayer = player.Supernova();
            supernovaPlayer.algizShieldHits = hits;

            // Spawn the shield
            var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Typeless.AlgizShieldProj>(), 0, 0);
            proj.timeLeft = timeLeft;

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustDirect(player.position, player.width, player.height, DustID.SilverCoin, Scale: 1.4f).noGravity = true;
            }
        }

        public virtual void ModifyShieldStats(ref int hits, ref int timeLeft) { }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!string.IsNullOrEmpty(FlavorTextTag))
            {
                string flavor = Language.GetTextValue(FlavorTextTag);
                if (!string.IsNullOrEmpty(flavor))
                {
                    TooltipLine line = new TooltipLine(Mod, "FlavorText", flavor)
                    {
                        OverrideColor = Microsoft.Xna.Framework.Color.LightBlue
                    };
                    tooltips.Add(line);
                }
            }

            base.ModifyTooltips(tooltips);
        }
    }
    public class LifewardRing : RingOfProtection
    {
        public override string Texture => base.Texture;
        public override string? FlavorTextTag => null;

        public override void ModifyShieldStats(ref int hits, ref int timeLeft)
        {
            hits = 2;
            timeLeft = 60 * 15;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RingOfProtection>());
            recipe.AddIngredient(ItemID.PlatinumBar, 5);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RingOfProtection>());
            recipe.AddIngredient(ItemID.GoldBar, 5);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}