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
        public override int MaxAnimationFrames => 30;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[base.Type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.Item.width = 16;
            base.Item.height = 16;
            base.Item.rare = ItemRarityID.LightPurple;
            base.Item.value = BuyPrice.RarityLightPurple;
        }

        public override void RingActivate(Player player, float ringPowerMulti)
        {
            player.AddBuff(ModContent.BuffType<ArcaneMight>(), 600, true, false);
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 0, default(Color), 1f);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.5f;
                Main.dust[dust].velocity *= 1.5f;
            }
            SoundEngine.PlaySound(SoundID.Item73, player.position);
        }

        public override void RingUseAnimation(Player player, int frame)
        {
            SoundEngine.PlaySound(SoundID.MaxMana, player.position);
            Vector2 dustPos = player.Center + Utils.RotatedByRandom(new Vector2(23f, 0f), (double)MathHelper.ToRadians(360f));
            Vector2 diff = player.Center - dustPos;
            diff.Normalize();
            Dust.NewDustPerfect(dustPos, 45, new Vector2?(diff * 2f), 0, default(Color), 1.5f).noGravity = true;
        }
    }
}
