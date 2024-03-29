using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Bosses.StoneMantaRay
{
    public class StoneRepeater : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surgestone Repeater");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 0);
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.width = 40;
            Item.crit = 4;
            Item.height = 20;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 8.4f;
            Item.value = 50000;
            Item.autoReuse = false;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
            Item.shootSpeed = 11f;
            Item.useAmmo = ItemUseStyleID.Swing;

            Item.DamageType = DamageClass.Ranged;
        }
    }
}