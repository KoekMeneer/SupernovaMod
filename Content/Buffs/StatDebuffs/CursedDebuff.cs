using Microsoft.Xna.Framework;
using SupernovaMod.Common.GlobalNPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.StatDebuffs
{
    public class CursedDebuff : ModBuff
    {
        // NPC only buff so we'll just assign it a useless buff icon.
        public override string Texture => "SupernovaMod/Assets/Textures/DebuffTemplate";

        protected Color[] DustColors { get; } = new Color[]
        {
            new Color(63, 0, 123),
            new Color(123, 29, 220),
            new Color(224, 141, 255)
        };

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            npc.GetGlobalNPC<DebuffNPC>().cursed = time;
            return base.ReApply(npc, time, buffIndex);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.Next(5) < 3)
            {
                int dust13 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, Main.rand.NextFromList(DustColors), 1.5f);
                Main.dust[dust13].noGravity = true;
                Main.dust[dust13].velocity *= 1.1f;
                Dust dust27 = Main.dust[dust13];
                dust27.velocity.Y = dust27.velocity.Y + 0.25f;
                if (Utils.NextBool(Main.rand, 2))
                {
                    Main.dust[dust13].noGravity = false;
                    Main.dust[dust13].scale *= 0.5f;
                }
            }
            if (npc.GetGlobalNPC<DebuffNPC>().cursed == 0)
            {
                npc.GetGlobalNPC<DebuffNPC>().cursed = npc.buffTime[buffIndex];
            }
        }
    }
}