using Terraria;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class BloodRose : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Rose");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 0, 30, 0);
        }
    }
}
