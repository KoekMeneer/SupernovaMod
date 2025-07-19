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
        public override int MaxAnimationFrames => 40;
        public override int BaseCooldown => 2220;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Green;
            Item.value = BuyPrice.RarityGreen;
            Item.damage = 12;
            damage = 15;
        }

        public override void RingActivate(Player player, float ringPowerMulti)
        {
            for (int i = 0; i < 7; i++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.JunglePlants, 0f, 0f, 0, default(Color), 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.5f;
                Main.dust[dust].velocity *= 1.5f;
            }
            int projectileDamage = (int)((float)damage * ringPowerMulti);
            //ProjectileHelper.ShootCrossPattern(player.GetSource_Accessory(base.Item, null), player.Center, 4, 3f, 484, ShootDamage, 0.4f, player.whoAmI, 0f, 0f, 0f);
            //ProjectileHelper.ShootPlusPattern(player.GetSource_Accessory(base.Item, null), player.Center, 4, 3f, 484, ShootDamage, 0.4f, player.whoAmI, 0f, 0f, 0f);
            player.AddBuff(Projectiles.Typeless.ThornedRingProj.BuffType, 60 * 20);
            Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Typeless.ThornedRingProj>(), projectileDamage, 3, player.whoAmI);
        }

        public override void RingUseAnimation(Player player, int frame)
        {
            SoundEngine.PlaySound(SoundID.Item15, default(Vector2?), null);
            Vector2 dustPos = player.Center + Utils.RotatedByRandom(new Vector2(30f, 0f), (double)MathHelper.ToRadians(360f));
            Vector2 diff = player.Center - dustPos;
            diff.Normalize();
            Dust.NewDustPerfect(dustPos, 40, new Vector2?(diff * 2f), 0, default(Color), 1f).noGravity = true;
            Dust.NewDustPerfect(dustPos, 44, new Vector2?(diff), 0, default(Color), 0.5f).noGravity = true;
        }
    }
}
