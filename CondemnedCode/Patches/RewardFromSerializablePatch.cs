using Condemned.CondemnedCode.Vfx;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using RewardTypeExtensions = Condemned.CondemnedCode.Extensions.RewardTypeExtensions;

namespace Condemned.CondemnedCode.Patches;

[HarmonyPatch(typeof(Reward), nameof(Reward.FromSerializable))]
public static class RewardFromSerializablePatch
{
    public static bool Prefix(
        SerializableReward save,
        Player player,
        ref Reward __result)
    {
        if (save.RewardType != RewardTypeExtensions.Curse)
            return true;

        var options = new CardCreationOptions(
            save.CardPoolIds
                .Select(ModelDb.GetById<CardPoolModel>),
            save.Source,
            save.RarityOdds);

        __result = new CurseCardReward(
            options,
            save.OptionCount,
            player);

        return false;
    }
}