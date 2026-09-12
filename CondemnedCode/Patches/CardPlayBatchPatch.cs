using Condemned.CondemnedCode.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
public static class CardPlayBatchPatch
{
    [HarmonyPrefix]
    public static void Prefix(CardModel __instance, out BatchState? __state)
    {
        GD.Print($"[CONDEMNED] Playing card {__instance.GetType()}");
        __state = KeywordModificationBatcher.Current;
        KeywordModificationBatcher.Enter();
    }

    [HarmonyPostfix]
    public static void Postfix(ref Task __result, BatchState? __state)
    {
        var myState = KeywordModificationBatcher.Current;

        KeywordModificationBatcher.SetCurrent(__state);

        __result = AwaitAndExit(__result, myState);
    }

    private static async Task AwaitAndExit(Task inner, BatchState? state)
    {
        try { await inner; }
        finally { KeywordModificationBatcher.Exit(state); }
    }
}