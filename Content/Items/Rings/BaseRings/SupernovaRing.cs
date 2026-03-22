using SupernovaMod.Common.Players;
using SupernovaMod.Common.Systems;
using SupernovaMod.Content.Prefixes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SupernovaMod.Content.Items.Rings.BaseRings
{
    public enum RingType
    {
        Misc = -1,
        Projectile,
    }

    /// <summary>
    /// This class provides methods, properties, etc. for Ring item types.
    /// </summary>
    public abstract class SupernovaRingItem : ModItem
    {
        /// <summary>
        /// The cooldown in ticks stat for our ring item.
        /// </summary>
        public abstract int BaseCooldown { get; }
        /// <summary>
        /// Damage for <see cref="RingType.Projectile"/> type rings.
        /// </summary>
        public virtual int Damage { get; protected set; }

        /// <summary>
        /// The type this ring belongs to.
        /// </summary>
        public virtual RingType RingType { get; } = RingType.Misc;

        public float damageBonusMulti = 1;
        /// <summary>
        /// Cooldown regen multiplier
        /// </summary>
        public float coolRegen = 1;

        /// <summary>
        /// The use animation duration.
        /// </summary>
        public virtual int UseTime => 0;

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            // Only allow equip only for our ring slot
            return slot == ModContent.GetInstance<SupernovaRingSlot>().Type;
        }

        public override void UpdateInventory(Player player)
        {
            Item.RebuildTooltip();
        }

        public override void UpdateEquip(Player player)
        {
            Item.RebuildTooltip();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            var rp = player.GetModPlayer<ResourcePlayer>();

            int insertIndex = 1;

            // Add cooldown stat
            //
            int cooldown = GetScaledCooldown(player);
            var line = new TooltipLine(Mod, "Cooldown", $"{FormatTime(cooldown)} cooldown");
            tooltips.Insert(insertIndex++, line);

            // Add damage stat
            //
            if (RingType == RingType.Projectile)
            {
                line = new TooltipLine(Mod, "Damage", $"{GetScaledDamage(player)} damage");
                tooltips.Insert(insertIndex++, line);
            }

            // Add hint to inform the user
            tooltips.Insert(tooltips.Count - 1, new TooltipLine(Mod, "Hint", $"Press [{SupernovaKeybinds.RingAbilityButton.GetAssignedKeys().FirstOrDefault(defaultValue: "<unset>")}] to activate"));

            // Disabled slot warning
            //
            if (!ModContent.GetInstance<Common.Configs.CommonConfig>().enableRingSlot)
            {
                var warn = new TooltipLine(Mod, "Disabled", "Not equipable (enable the ring slot in config)")
                {
                    IsModifier = true,
                    IsModifierBad = true
                };
                tooltips.Add(warn);
            }

            // Add cooldown regen modifier
            //
            if (coolRegen != 1)
            {
                line = new TooltipLine(Mod, "RingCoolRegenBonus", string.Empty);
                line.IsModifier = true;

                // Get positive or negative bonus value
                double bonus;
                if (coolRegen > 1)
                {
                    bonus = Math.Round(coolRegen * 100) - 100;
                    line.Text = "+";
                    line.IsModifierBad = true;
                }
                else
                {
                    bonus = 100 - Math.Round(coolRegen * 100);
                    line.Text = "-";
                }
                line.Text += $"{bonus}% cooldown";
                tooltips.Add(line);
            }

            // Add damage modifier
            //
            if (damageBonusMulti != 1)
            {
                line = new TooltipLine(Mod, "RingDamageBonus", string.Empty);
                line.IsModifier = true;

                // Get positive or negative bonus value
                double bonus;
                if (damageBonusMulti > 1)
                {
                    bonus = Math.Round(damageBonusMulti * 100) - 100;
                    line.Text = "+";
                }
                else
                {
                    bonus = 100 - Math.Round(damageBonusMulti * 100);
                    line.Text = "-";
                    line.IsModifierBad = true;
                }
                line.Text += $"{bonus}% damage";
                tooltips.Add(line);
            }
        }

        #region Reforge Methods

        public override void PreReforge()
        {
            coolRegen = 1;
            damageBonusMulti = 1;
        }

        // Make rings only be able to get ring prefixes
        //
        private int[]? _ringPrefixCache = null;
        protected virtual int[] GetPrefixes()
        {
            if (_ringPrefixCache == null)
            {
                _ringPrefixCache = RingPrefix.LoadPrefixes(this);
            }
            return _ringPrefixCache;
        }
        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return Main.rand.NextFromList(GetPrefixes());
        }
        public override bool AllowPrefix(int pre)
        {
            return GetPrefixes()
                .Contains(pre);
        }

        #endregion

        // --- Activation flow ---

        /// <summary>
        /// Returns whether or not this ring can be activated. By default returns true.
        /// </summary>
        /// <param name="player">The player using the item.</param>
        public virtual bool CanRingActivate(Player player) => true;

        /// <summary>
        /// Called when the 'ring ability key' is pressed, before activating the ring ability.
        /// </summary>
        /// <param name="player"></param>
        public virtual void PreActivate(Player player) { }

        /// <summary>
        /// Called when the ring is activated.
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnActivate(Player player) { }

        /// <summary>
        /// Called after ring activation. Use for cleanup and extra visuals.
        /// </summary>
        /// <param name="player"></param>
        public virtual void PostActivate(Player player) { }

        /// <summary>
        /// Called on the first use animation frame.
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnStartUse(Player player) { }

        /// <summary>
        /// Called every frame during the use animation.
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnUseFrame(Player player, int frame) { }

        // --- Failure / feedback ---
        /// <summary>
        /// Called after the ring fails to be activated.
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnActivateFailed(Player player) { }

        // --- Helper methods ---
        
        public int GetScaledCooldown(Player player)
        {
            var rp = player.GetModPlayer<ResourcePlayer>();

            int cooldown = BaseCooldown;
            cooldown = (int)(cooldown * coolRegen); // From reforge
            cooldown -= rp.ringFlatCooldownReduction;
            cooldown = (int)(cooldown * rp.ringCooldownMult);

            return Math.Max(1, cooldown);
        }

        public int GetScaledDamage(Player player, bool useRingPower = true)
        {
            var rp = player.GetModPlayer<ResourcePlayer>();

            int damage = Damage;
            damage = (int)(damage * rp.ringPower);

            return Math.Max(1, damage);
        }

        protected string FormatTime(int ticks)
        {
            int totalSeconds = ticks / 60;
            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;

            if (minutes > 0)
            {
                if (seconds == 0)
                {
                    return $"{minutes}s";
                }

                return $"{minutes}m {seconds}s";
            }

            return $"{seconds}s";
        }
    }
}
