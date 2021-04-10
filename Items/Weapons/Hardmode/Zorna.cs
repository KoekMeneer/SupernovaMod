using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.Hardmode
{
    public class Zorna : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zorna");
        }
        public override void SetDefaults()
		{
            item.damage = 25;
            item.melee = true;
            item.crit = 32;
            item.width = 40;
            item.height = 40;
            item.useTime = 7;
            item.useAnimation = 7;
            item.useStyle = 1;
            item.knockBack = 3.3f;
            item.value = Item.buyPrice(1, 0, 0, 0); // Another way to handle value of item.
            item.rare = 10;
            item.UseSound = SoundID.Item1;
            item.useTurn = false;
            item.autoReuse = true;
        }
    }
}
