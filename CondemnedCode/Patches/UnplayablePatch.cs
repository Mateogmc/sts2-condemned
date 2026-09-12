using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using System.Reflection;
using BaseLib.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Condemned.CondemnedCode.Patches;

[HarmonyPatch]
public class UnplayablePatch
{
    static MethodBase TargetMethod()
    {
        return AccessTools.Method(
            typeof(CardModel),
            "CanPlay",
            new[]
            {
                typeof(UnplayableReason).MakeByRefType(),
                typeof(AbstractModel).MakeByRefType()
            });
    }

    static void Postfix(
        CardModel __instance,
        ref bool __result,
        ref UnplayableReason reason,
        ref AbstractModel preventer)
    {
        if ((reason & UnplayableReason.HasUnplayableKeyword) != 0)
        {
            var capable = __instance.Owner.Creature.GetPower<CapablePower>();

            if (__instance.Owner.HasPower<DoTheImpossiblePower>() ||
                (capable != null && capable.CurrentAmount > 0))
            {
                reason &= ~UnplayableReason.HasUnplayableKeyword;
            }
        }

        if ((reason & UnplayableReason.BlockedByHook) != 0)
        {
            if (__instance is Normality)
            {
                reason &= ~UnplayableReason.BlockedByHook;
            }
        }

        __result = reason == UnplayableReason.None;
    }
}