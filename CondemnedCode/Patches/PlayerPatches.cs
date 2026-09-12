using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Patches;

[HarmonyPatch(typeof(Player))]
public class PlayerPatches
{
    [HarmonyPatch("PopulateCombatState")]
    [HarmonyPrefix]
    public static void prefix(CombatState state)
    {
        GD.Print("[CONDEMNED] Hook before combat state population");
        ReloadPermanentInnateBeforeCombat(state);
    }
    
    public static void ReloadPermanentInnateBeforeCombat(CombatState state)
    {
        foreach (Player player in state.Players)
        {
            foreach (CardModel card in player.Deck.Cards)
            {
                if (ChosenDestinyFields.HasPermanentInnate.Get(card))
                {
                    if (ChosenDestinyFields.PermanentInnate.Get(card))
                    {
                        CardCmd.ApplyKeyword(card, CardKeyword.Innate);
                    }
                    else
                    {
                        CardCmd.RemoveKeyword(card, CardKeyword.Innate);
                    }
                }
            }
        }
    }

}