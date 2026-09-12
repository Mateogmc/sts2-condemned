using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Condemned.CondemnedCode.Powers;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BaseLib.Extensions;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Patches
{
    [HarmonyPatch(typeof(NHealthBar))]
    public static class NHealthBarScathingPatch
    {
        private static readonly ConditionalWeakTable<NHealthBar, Control> _scathingControls = new();

        [HarmonyPatch("_Ready")]
        [HarmonyPostfix]
        public static void ReadyPostfix(NHealthBar __instance)
        {
            var poison = __instance.GetNode<NinePatchRect>("%PoisonForeground");
            if (poison == null)
                return;

            var mask = poison.GetParent();
            if (mask == null)
                return;

            var scathing = poison.Duplicate() as NinePatchRect;
            if (scathing == null)
                return;

            scathing.Name = "ScathingForeground";
            scathing.Visible = false;
            scathing.Modulate = Colors.White;
            scathing.SelfModulate = new Color(0.2f, 0.2f, 0.2f);
            scathing.ZIndex = poison.ZIndex;

            mask.AddChild(scathing);

            mask.MoveChild(scathing, mask.GetChildCount() - 1);

            _scathingControls.Remove(__instance);
            _scathingControls.Add(__instance, scathing);
            
            GD.Print($"Scathing visible: {scathing.Visible}");
            GD.Print($"Parent: {scathing.GetParent().Name}");
            GD.Print($"ZIndex: {scathing.ZIndex}");
        }

        [HarmonyPatch("RefreshForeground")]
        [HarmonyPostfix]
        public static void RefreshForegroundPostfix(NHealthBar __instance)
        {
            if (!_scathingControls.TryGetValue(__instance, out var scathing))
                return;

            var creature = __instance.GetType()
                .GetField("_creature", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(__instance) as Creature;

            if (creature == null || creature.CombatState == null)
                return;

            var player = creature.CombatState.Players
                .FirstOrDefault(c => c != null && c.HasPower<ScathingPower>(), null);

            if (player == null || player.Creature.GetPower<ScathingPower>() == null)
            {
                scathing.Visible = false;
                return;
            }

            if (creature.HpDisplay.IsInfinite())
            {
                scathing.Visible = false;
                return;
            }

            int jinxPower = creature.HasPower<JinxPower>() ? creature.GetPowerAmount<JinxPower>() : 0;
            if (jinxPower <= 0 || creature.CurrentHp <= 0)
            {
                scathing.Visible = false;
                return;
            }

            int poisonDamage = creature.HasPower<PoisonPower>() ? creature.GetPower<PoisonPower>().CalculateTotalDamageNextTurn() : 0;
            int doomAmount = creature.HasPower<DoomPower>() ? creature.GetPowerAmount<DoomPower>() : 0;

            if (IsPoisonLethal(poisonDamage, creature) || IsDoomLethal(doomAmount, poisonDamage, creature))
            {
                scathing.Visible = false;
                return;
            }

            var maxFgWidthProp = typeof(NHealthBar)
                .GetProperty("MaxFgWidth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            float maxFgWidth = maxFgWidthProp != null
                ? (float)maxFgWidthProp.GetValue(__instance)
                : 0f;

            if (maxFgWidth <= 0)
                return;

            int currentHp = creature.CurrentHp;

            scathing.Visible = true;
            
            float hpWidth = maxFgWidth * currentHp / creature.MaxHp;

            if (IsScathingLethal(jinxPower, poisonDamage, creature))
            {
                scathing.OffsetLeft = 0f;
                scathing.OffsetRight = hpWidth - maxFgWidth;

                var hpForeground = __instance.GetType()
                    .GetField("_hpForeground", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(__instance) as Control;

                if (hpForeground != null)
                    hpForeground.Visible = false;
            }
            else
            {
                float safeWidth =
                    maxFgWidth * (currentHp - jinxPower - poisonDamage) / creature.MaxHp;

                float poisonEndWidth =
                    maxFgWidth * (currentHp - poisonDamage) / creature.MaxHp;

                scathing.OffsetLeft = safeWidth;
                scathing.OffsetRight = poisonEndWidth - maxFgWidth;
            }
        }

        [HarmonyPatch("RefreshText")]
        [HarmonyPostfix]
        public static void RefreshTextPostfix(NHealthBar __instance)
        {
            var creature = __instance.GetType()
                .GetField("_creature", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(__instance) as Creature;

            if (creature == null || creature.CombatState == null || creature.CurrentHp <= 0)
                return;

            var player = creature.CombatState.Players
                .FirstOrDefault(c => c != null && c.HasPower<ScathingPower>(), null);

            if (player == null || player.Creature.GetPower<ScathingPower>() == null)
                return;

            int jinx = creature.GetPowerAmount<JinxPower>();
            if (jinx <= 0)
                return;
            
            int poisonDamage = creature.HasPower<PoisonPower>() ? creature.GetPower<PoisonPower>().CalculateTotalDamageNextTurn() : 0;
            int doomAmount = creature.HasPower<DoomPower>() ? creature.GetPowerAmount<DoomPower>() : 0;

            if (IsPoisonLethal(poisonDamage, creature) || IsDoomLethal(doomAmount, poisonDamage, creature))
            {
                return;
            }

            int jinxPower = creature.HasPower<JinxPower>() ? creature.GetPowerAmount<JinxPower>() : 0;

            if (IsScathingLethal(jinxPower, poisonDamage, creature))
            {
                var label = __instance.GetType()
                    .GetField("_hpLabel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(__instance) as MegaLabel;

                if (label != null)
                {
                    label.AddThemeColorOverride(ThemeConstants.Label.FontColor, new Color(0.2f, 0.2f, 0.2f));
                    label.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, new Color(0.6f, 0.6f, 0.6f));
                }
            }
        }

        private static bool IsPoisonLethal(int poisonDamage, Creature creature)
        {
            return poisonDamage > 0 && creature.HasPower<PoisonPower>() && poisonDamage >= creature.CurrentHp;
        }

        private static bool IsDoomLethal(int doomAmount, int poisonDamage, Creature creature)
        {
            return doomAmount > 0 && creature.HasPower<DoomPower>() && doomAmount >= creature.CurrentHp - poisonDamage;
        }

        private static bool IsScathingLethal(int scathingDamage, int poisonDamage, Creature creature)
        {
            return scathingDamage > 0 && creature.HasPower<JinxPower>() && scathingDamage >= creature.CurrentHp - poisonDamage;
        }
    }
}