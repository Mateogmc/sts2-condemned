using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using Condemned.CondemnedCode.Utils;

[HarmonyPatch(typeof(AssetSets), nameof(AssetSets.CommonAssets), MethodType.Getter)]
internal static class AssetSetsCommonAssetsPatch
{
    [HarmonyPostfix]
    private static void Postfix(ref IReadOnlySet<string> __result)
    {
        __result = __result
            .Concat(CondemnedResources.AssetPaths)
            .ToHashSet();
    }
}