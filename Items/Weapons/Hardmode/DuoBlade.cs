using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.Hardmode
{
    public class DuoBlade : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duo Blade");
            Tooltip.SetDefault("Burns enemies during daytime and may Confuse enemies during nighttime.");
        }
        public override void SetDefaults()
		{
            item.damage = 70;
            item.melee = true;
            item.crit = 10;
            item.width = 40;
            item.height = 40;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = 1;
            item.knockBack = 7;
            item.scale *= 1.1f;
            item.value = Item.buyPrice(0, 35, 57, 87); // Another way to handle value of item.
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if(!Main.dayTime && Main.rand.NextBool(4))
                target.AddBuff(BuffID.Confused, 160);
            else if(Main.dayTime)
            {
                target.AddBuff(BuffID.OnFire, 120);
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BrokenSwordShards"));
            recipe.AddIngredient(ItemID.DarkShard);
            recipe.AddIngredient(ItemID.LightShard);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
