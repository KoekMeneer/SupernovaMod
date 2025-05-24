using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Rings
{
    public class ThornedRingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }
    }
}
