using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Supernova;

namespace Supernova.Npcs.Bosses.CosmicCollective
{
	public class CosmofleshCutter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmo FleshCutter");
        }
		public override void SetDefaults()
		{
			item.damage = 52;
			item.melee = true;
			item.channel = true;
            item.crit = 4;
            item.width = 40;
			item.height = 40;
			item.useTime = 29;
			item.useAnimation = 29;
			item.knockBack = 5.5f;
			item.value = Item.buyPrice(0, 12, 46, 82);
			item.rare = Rarity.LigtRed;
			item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useStyle = 1;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			target.AddBuff(BuffID.ShadowFlame, 80);
			base.OnHitNPC(player, target, damage, knockBack, crit);
		}
	}
}
