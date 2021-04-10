using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class HelixStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helix Stone");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.rare = Rarity.LigtRed;
            item.value = Item.buyPrice(0, 0, 70, 50);
        }
    }
}
