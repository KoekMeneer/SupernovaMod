using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using SupernovaMod.Core.Effects;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class BoneCrusher : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        protected Color[] DustColors { get; } = new Color[]
        {
            new Color(63, 0, 123),
            new Color(123, 29, 220),
            new Color(224, 141, 255)
        };
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 58;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 7;
            Item.damage = 68;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 12, 30, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BoneSword);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ModContent.ItemType<Materials.BoneFragment>(), 7);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Buffs.StatDebuffs.CursedDebuff>(), 240);
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            // +50% damage against the undead
            //
            if (NPCID.Sets.Zombies[target.type] || NPCID.Sets.Skeletons[target.type])
            {
                modifiers.FinalDamage *= 1.5f;
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(2))
            {
                // Emit dusts when the sword is swung 
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Bone, Scale: .3f);
            }
            if (Main.rand.NextBool(4))
            {
                // Emit dusts when the sword is swung 
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Shadowflame, newColor: Main.rand.NextFromList(DustColors), Scale: Main.rand.NextFloat(.5f, 1.5f));
            }
        }
    }
}
