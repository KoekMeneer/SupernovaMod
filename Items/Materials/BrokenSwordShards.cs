using Terraria;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class BrokenSwordShards : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Sword Shards");
            Tooltip.SetDefault("You can buy this at the Ronin npc during a HardMode Bloodmoon.");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 20, 0, 0);
        }
    }
}
