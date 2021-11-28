using Terraria;
using Terraria.ModLoader;

namespace Supernova.Buffs.Minion
{
    public class RedDevilBuff : ModBuff
    {

        public override void SetDefaults()
        {
            DisplayName.SetDefault("RedDevilBuff");
            Description.SetDefault("A Red Devil spawn that will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            SupernovaPlayer modPlayer = (SupernovaPlayer)player.GetModPlayer(mod, "SupernovaPlayer");

            if (player.ownedProjectileCounts[mod.ProjectileType("RedDevilMinion")] > 0)
                modPlayer.minionRedDevil = true;
            if (!modPlayer.minionRedDevil)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}