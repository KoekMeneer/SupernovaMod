using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.CosmicCollective
{
    public class Cosmorang : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmorang");
        }

        public override void SetDefaults()
        {
            item.damage = 40;
            item.crit = 3;
            item.thrown = true;
            item.noMelee = true;
            item.maxStack = 1;
            item.width = 30;
            item.height = 30;
            item.useTime = 8;
            item.useAnimation = 8;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 2.2f;
            item.value = Item.buyPrice(0, 12, 46, 82);
            item.rare = Rarity.LigtRed;
            item.shootSpeed = 13f;
            item.shoot = mod.ProjectileType("Cosmorang");
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
    }
}