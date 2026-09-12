using Condemned.CondemnedCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Patches;

[HarmonyPatch(typeof(CardModel))]
public class CardModelKeywordModifiedPatch
{
    [HarmonyPatch("AddKeyword")]
    [HarmonyPostfix]
    public static void AddKeywordPostfix(CardModel __instance, CardKeyword keyword)
    {
        if (KeywordModificationBatcher.IsBatching)
        {
            KeywordModificationBatcher.Record(__instance);
            return;
        }
        HookUtils.OnAfterCardKeywordsModified(__instance.RunState, __instance, keyword, true);
    }

    [HarmonyPatch("RemoveKeyword")]
    [HarmonyPostfix]
    public static void RemoveKeywordPostfix(CardModel __instance, CardKeyword keyword)
    {
        if (KeywordModificationBatcher.IsBatching)
        {
            KeywordModificationBatcher.Record(__instance);
            return;
        }
        HookUtils.OnAfterCardKeywordsModified(__instance.RunState, __instance, keyword, false);
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.BaseReplayCount), MethodType.Setter)]
    [HarmonyPrefix]
    public static void BaseReplayCountPrefix(CardModel __instance, out int __state)
        => __state = __instance.BaseReplayCount;

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.BaseReplayCount), MethodType.Setter)]
    [HarmonyPostfix]
    public static void BaseReplayCountPostfix(CardModel __instance, int __state)
    {
        int delta = __instance.BaseReplayCount - __state;
        if (delta == 0) return;

        if (KeywordModificationBatcher.IsBatching)
        {
            KeywordModificationBatcher.Record(__instance);
            return;
        }

        HookUtils.OnAfterCardKeywordsModified(
            __instance.RunState, __instance, CardKeyword.None, delta > 0);
    }
}