using Condemned.CondemnedCode.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace Condemned.CondemnedCode.Patches;

[HarmonyPatch(typeof(RunState))]
internal static class RunStateLoadPatches
{
    [HarmonyPatch("FromSerializable")]
    [HarmonyPostfix]
    public static RunState Postfix(RunState __result, SerializableRun save)
    {
        GD.Print("[CONDEMNED] RunStateLoadPatches.Postfix FromSerializable");
        HookUtils.OnAfterRunInitialized(__result);
        HookUtils.OnAfterRunLoadedFromSave(__result, save);
        return __result;
    }

    [HarmonyPatch("CreateForNewRun")]
    [HarmonyPostfix]
    public static RunState CreateForNewRunPostfix(RunState __result)
    {
        GD.Print("[CONDEMNED] RunStateLoadPatches.Postfix CreateForNewRun");
        HookUtils.OnAfterRunInitialized(__result);
        return __result;
    }
}