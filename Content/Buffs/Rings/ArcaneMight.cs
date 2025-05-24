using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Rings
{
    class ArcaneMight : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type]            = true;
            Main.pvpBuff[Type]           = true;
            Main.buffNoSave[Type]        = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // During the buff duration, no mana will be used
            player.manaCost = 0f;

            // Preform a dust effect to indicate that the buff is active
            //
            Vector2 dustPos = player.Center + Utils.RotatedByRandom(new Vector2(23f, 0f), (double)MathHelper.ToRadians(360f));
            Vector2 diff = player.Center - dustPos;
            diff.Normalize();
            Dust.NewDustPerfect(dustPos, 45, new Vector2?(diff * 2f), 0, default(Color), 1f).noGravity = true;
        }
    }
}
