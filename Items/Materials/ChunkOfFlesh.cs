using Terraria;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class ChunkOfFlesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunk of Flesh");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.rare = Rarity.LigtRed;
            item.value = Item.buyPrice(0, 0, 25, 0);
        }
    }
}
