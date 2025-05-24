using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Summon
{
    public class CosmolingMinionBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.CosmolingMinion>()] > 0)
            {
				player.buffTime[buffIndex] = 18000;
			}
            else
            {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
        }
    }
}