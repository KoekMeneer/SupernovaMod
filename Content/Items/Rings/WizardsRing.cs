using Microsoft.Xna.Framework;
using SupernovaMod.Content.Buffs.Rings;
using SupernovaMod.Content.Items.Rings.BaseRings;
using SupernovaMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Rings
{
    public class WizardsRing : SupernovaRingItem
    {
        public override int BaseCooldown => 7200;
        public override int Damage { get; protected set; } = 0;
        public override int UseTime => 30;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[base.Type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.Item.width = 30;
            base.Item.height = 22;
            base.Item.rare = ItemRarityID.LightPurple;
            base.Item.value = BuyPrice.RarityLightPurple;
        }

        public override void OnUseFrame(Player player, int frame)
        {
            SoundEngine.PlaySound(SoundID.MaxMana with { Volume = 0.3f });

            Vector2 pos = player.Center + Main.rand.NextVector2Circular(25, 25);
            Dust.NewDustPerfect(pos, DustID.ManaRegeneration, Vector2.Zero).noGravity = true;
        }

        public override void OnActivate(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item73);

            player.AddBuff(ModContent.BuffType<ArcaneMight>(), 600);

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustDirect(player.position, player.width, player.height, DustID.ManaRegeneration, Scale: 1.5f).noGravity = true;
            }
        }
    }
}
