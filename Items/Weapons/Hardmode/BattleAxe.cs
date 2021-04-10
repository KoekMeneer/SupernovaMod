using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Items.Weapons.Hardmode
{
    public class BattleAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Battle Axe");
            Tooltip.SetDefault("To War!");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, -5);
        }
        public override void SetDefaults()
		{
			item.damage = 72;
			item.melee = true;
            item.crit = 18;
            item.width = 40;
			item.height = 40;
			item.useTime = 26;
			item.useAnimation = 26;
			item.useStyle = 1;
			item.knockBack = 24;
            item.value = Item.buyPrice(0, 12, 0, 0); // Another way to handle value of item.
            item.rare = Rarity.LigtRed;
			item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			target.AddBuff(BuffID.Frostburn, 666);
        }
    }
}
