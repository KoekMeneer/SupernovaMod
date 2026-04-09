using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Items.Armor.Verglas
{
    [AutoloadEquip(EquipType.Head)]
    public class VerglasVeil : VerglasHelm
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.defense = 7;
        }

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += .15f;
			player.GetDamage(DamageClass.Summon) += .15f;

            player.maxMinions += 2;
            player.manaCost -= 0.05f;
            player.statManaMax2 += 30;
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
