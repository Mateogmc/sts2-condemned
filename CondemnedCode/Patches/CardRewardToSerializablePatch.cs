using Condemned.CondemnedCode.Vfx;
using HarmonyLib;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Saves.Runs;
using RewardTypeExtensions = Condemned.CondemnedCode.Extensions.RewardTypeExtensions;

namespace Condemned.CondemnedCode.Patches;

[HarmonyPatch(typeof(CardReward), nameof(CardReward.ToSerializable))]
public static class CardRewardToSerializablePatch
{
    public static void Postfix(
        CardReward __instance,
        SerializableReward __result)
    {
        if (__instance is CurseCardReward)
            __result.RewardType = RewardTypeExtensions.Curse;
    }
}