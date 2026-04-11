using Microsoft.Xna.Framework;
using SupernovaMod.Core;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Tools
{
    public class VerglasHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
			Item.knockBack = 7;
			Item.width = 24;
            Item.height = 24;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = SellPrice.VerglasItem;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
			Item.useTurn = true;

			Item.hammer = 70;
            Item.axe = 30;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(4))
			{
				// Emit dusts when the sword is swung 
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost);
			}
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 80);
		}
		public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
		{
			if (hurtInfo.PvP)
			{
				target.AddBuff(BuffID.Frostburn, 80);
			}
		}
	}
}
