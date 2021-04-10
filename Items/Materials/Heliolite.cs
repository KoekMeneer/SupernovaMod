using Terraria;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class Heliolite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heliolite");
            Tooltip.SetDefault("Droped by Hellion enemies");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.rare = 5;
            item.value = Item.buyPrice(0, 0, 50, 50);
        }
    }
}
