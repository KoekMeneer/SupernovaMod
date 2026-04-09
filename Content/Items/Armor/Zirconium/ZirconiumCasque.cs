using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Items.Armor.Zirconium
{
    [AutoloadEquip(EquipType.Head)]
    public class ZirconiumCasque : ZirconiumHelmet
	{
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.defense = 3;
        }

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Ranged) += .05f;
		}
    }
}
