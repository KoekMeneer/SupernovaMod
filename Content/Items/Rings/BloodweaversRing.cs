using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace SupernovaMod.Content.Items.Rings
{
    public class BloodweaversRing : SupernovaRingItem
    {
        public override RingType RingType => RingType.Projectile;
        public override int BaseCooldown => 60 * 140;
        public override int Damage { get; protected set; } = 24;
        public override int UseTime => 75;

        private float _rot;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
			base.SetDefaults();
			Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 6, 0, 0);
		}

        public override void OnUseFrame(Player player, int frame)
        {
            SoundEngine.PlaySound(SoundID.Item15 with { Volume = 0.25f }, player.Center);

            // Rotating blood tendrils around the player
            float rot = MathHelper.ToRadians(frame * 9);
            Vector2 pos = player.Center + new Vector2(30f, 0).RotatedBy(rot);

            int dustId = Main.rand.NextBool(2) ? DustID.CrimsonTorch : DustID.Blood;
            RingVFX.DustOrbit(
                ref rot,
                pos,
                dustId,
                radius: .2f,
                speed: MathHelper.ToRadians(frame * 9)
            );

            Vector2 pos2 = player.Center + new Vector2(30f, 0).RotatedBy(rot);
            RingVFX.DustOrbit(
                ref rot,
                pos2,
                dustId,
                radius: -.2f,
                speed: MathHelper.ToRadians(frame * 9)
            );
        }

        public override void OnActivate(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item14, player.Center);

			int dmg = GetScaledDamage(player);
            int hits = 0;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.CanBeChasedBy()) continue;

                float dist = Vector2.Distance(player.Center, npc.Center);
                if (dist > 800) continue;

                Projectile.NewProjectile(player.GetSource_ItemUse(Item),
                    npc.Center, Vector2.Zero,
                    ProjectileID.SoulDrain, dmg, 0, player.whoAmI
                );

                Projectile.NewProjectile(player.GetSource_ItemUse(Item),
                    npc.Center,
                    Main.rand.NextVector2Circular(3, 3),
                    ProjectileID.VampireHeal, 1, 0,
                    player.whoAmI, 0, (int)(dmg * 0.4f)
                );

                hits++;
                if (hits >= 10) break;
            }

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustDirect(player.position, player.width, player.height, DustID.Blood, Scale: 2f).noGravity = true;
            }
        }
    }
}
