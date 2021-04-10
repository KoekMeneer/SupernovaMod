using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Items.Weapons.Hardmode
{
    public class DriveSaber : ModItem
	{
        private int i = 0;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drive Saber");
        }
        public override void SetDefaults()
		{
            item.width = 40;
			item.height = 40;
			item.useStyle = 1;
            item.damage = 35;
            item.crit = 3;
            item.value = Item.buyPrice(0, 40, 0, 0);
            item.rare = Rarity.Pink;
			item.UseSound = SoundID.Item1;
            item.useTurn = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {

            if (player.altFunctionUse == 2) // Right Click function
            {
                item.shoot = ProjectileID.MagnetSphereBolt;

                if (i >= 16)
                {
                    item.damage = 78;  //The damage stat for the Weapon.
                    item.useTime = 58;
                    item.useAnimation = 54;
                    item.autoReuse = false;
                    item.shootSpeed = 2.3f;
                    item.shoot = ProjectileID.Electrosphere;
                    i -= 16;
                }
                else
                {
                    item.damage = 35;
                    item.useTime = 14;
                    item.useAnimation = 9;
                    item.autoReuse = true;
                    item.shootSpeed = 0.001f;
                    item.shoot = ProjectileID.MagnetSphereBolt;
                }
            }
            else // Default Left Click
            {
                item.damage = 35;
                item.useTime = 8;
                item.useAnimation = 8;
                item.autoReuse = true;
                item.shoot = ProjectileID.MagnetSphereBolt;
                i++;
                if (i >= 64)
                {
                    i--;
                }
                item.shootSpeed = (i * .07f);
            }
            return base.CanUseItem(player);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                //Emit dusts when the sword is swung
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Electric);
                Main.dust[dust].noGravity = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MechroDrive"), 7);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(mod.GetItem("BrokenSwordShards"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
