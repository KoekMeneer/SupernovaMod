using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Rings
{
    public class GemRing : SupernovaRingItem
    {
        public override RingType RingType => RingType.Projectile;
        public override int BaseCooldown => 4800;
        public override int Damage { get; protected set; } = 17;
        public override int UseTime => 60;

        private float _rot;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.accessory = true;

            Damage = 17;
        }

        public override void OnUseFrame(Player player, int frame)
        {
            RingVFX.PlayChargeSound(player.Center);

            // rotating gem dust
            for (int i = 0; i < 3; i++)
            {
                //_rot += MathHelper.ToRadians(10);
                Vector2 pos = player.Center + new Vector2(20, 0).RotatedBy(_rot);

                int randDustId = Main.rand.NextFromList(DustID.GemRuby, DustID.GemSapphire, DustID.GemEmerald, DustID.GemTopaz, DustID.GemDiamond);
                RingVFX.DustOrbit(
                    ref _rot,
                    pos,
                    randDustId,
                    radius: .2f,
                    speed: MathHelper.ToRadians(10)
                );
            }
        }

        public override void OnActivate(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item14, player.Center);

            Vector2 position = player.Center;
            int damage = GetScaledDamage(player);

            Vector2 direction = Main.MouseWorld - player.Center;

            direction.Normalize();

            for (int i = -2; i <= 2; i++)
            {
                float speed = 7f + i;

                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    direction * speed,
                    121 + (i + 2), // Get gem project types
                    damage,
                    3f,
                    player.whoAmI
                );
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}