using System.Reflection;
using BaseLib.Extensions;
using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Patches;

[HarmonyPatch]
public static class CardModelPersuasivePatch
{
    static MethodBase TargetMethod()
    {
        return AccessTools.FirstMethod(
            typeof(CardModel),
            m => m.Name == "GetDescriptionForPile");
    }

    [HarmonyPostfix]
    static void Postfix(CardModel __instance, ref string __result)
    {
        if (!__instance.IsMutable)
            return;

        var owner = __instance.Owner;
        if (owner == null)
            return;
        if (!owner.HasPower<PersuasivePower>())
            return;
        decimal amount = owner.Creature.GetPowerAmount<PersuasivePower>() * (__instance.Keywords.Count + (__instance.BaseReplayCount > 0 ? 1 : 0));

        if (__instance.Type == CardType.Skill)
        {
            if (__instance.DynamicVars.ContainsKey("Block"))
                return;

            LocString locString = new LocString("powers", "CONDEMNED-PERSUASIVE_POWER.blockExtra");
            locString.Add("Amount", amount);

            __result += "\n" + locString.GetFormattedText();
        }
        
        if (__instance.IsCurse())
        {
            var jinxLoc = new LocString(
                "powers",
                "CONDEMNED-PERSUASIVE_POWER.jinxExtra"
            );

            jinxLoc.Add("Amount", amount);

            __result += "\n" + jinxLoc.GetFormattedText();
        }
    }
}