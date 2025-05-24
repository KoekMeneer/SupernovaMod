using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Items.Materials
{
	public class EldritchEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			//ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 0, 80);
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (Utils.NextBool(Main.rand, 10))
            {
                Vector2 velocity = Utils.RotatedByRandom(Vector2.One, 6.28000020980835) * Utils.NextFloat(Main.rand, 0.7f, 1.25f);
                float num = Utils.NextFloat(Main.rand, 0.5f, 3f);
                int dust = Dust.NewDust(Item.position, Item.width, Item.height, 296, velocity.X, velocity.Y, 0, default(Color), num);
                Main.dust[dust].noGravity = false;
            }
        }
    }
}
