using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Rings.BaseRings;
using SupernovaMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Rings
{
    public class ThornedRing : SupernovaRingItem
    {
        public override int BaseCooldown => 2220;
        public override int Damage { get; protected set; } = 15;
        public override int UseTime => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Green;
            Item.value = BuyPrice.RarityGreen;
            Item.damage = 12;
        }

        public override void OnUseFrame(Player player, int frame)
        {
            SoundEngine.PlaySound(SoundID.Item15, player.Center);

            Vector2 pos = player.Center + Main.rand.NextVector2Circular(30, 30);
            Dust.NewDustPerfect(pos, DustID.JunglePlants, (player.Center - pos) * 0.2f).noGravity = true;
        }

        public override void OnActivate(Player player)
        {
            int dmg = GetScaledDamage(player);

            player.AddBuff(Projectiles.Typeless.ThornedRingProj.BuffType, 60 * 20);

            Projectile.NewProjectile(
                player.GetSource_Accessory(Item),
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<Projectiles.Typeless.ThornedRingProj>(),
                dmg,
                3,
                player.whoAmI
            );
        }
    }
}
