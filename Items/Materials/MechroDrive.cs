using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class MechroDrive : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechro Drive");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = Item.buyPrice(0, 0, 12, 65);
            item.rare = Rarity.Orange;
            item.UseSound = SoundID.Item1;
            item.maxStack = 999;
        }
    }
}