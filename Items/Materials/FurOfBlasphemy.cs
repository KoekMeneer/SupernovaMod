using Terraria;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class FurOfBlasphemy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fur of Blasphemy");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.rare = Rarity.LigtRed;
            item.value = Item.buyPrice(0, 0, 12, 27);
        }
	}
}
