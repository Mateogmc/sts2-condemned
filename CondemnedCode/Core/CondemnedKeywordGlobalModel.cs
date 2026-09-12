using BaseLib.Abstracts;
using BaseLib.Utils;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Utils;
using Condemned.CondemnedCode.Utils.Interfaces;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace Condemned.CondemnedCode.Core;

public class CondemnedKeywordGlobalModel() : CustomSingletonModel(HookType.Run), IRunLoadedFromSaveListener, IRunInitializedListener
{
    #region CHOSEN DESTINY

    private static RunState runState = null;

    public void AfterRunLoadedFromSave(RunState runState, SerializableRun save)
    {
        CondemnedKeywordGlobalModel.runState = runState;
        
        ReloadPermanentInnate();
    }

    public void AfterRunInitialized(RunState runState)
    {
        CondemnedKeywordGlobalModel.runState = runState;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (runState == null) return base.AfterRoomEntered(room);
        if (runState.Players.Count > 1)
        {
            ReloadPermanentInnate();
        }
        
        return base.AfterRoomEntered(room);
    }

    public override Task BeforeCombatStart()
    {
        if (runState != null)
            ReloadPermanentInnate();
        
        return base.BeforeCombatStart();
    }

    private void ReloadPermanentInnate()
    {
        foreach (Player player in runState.Players)
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

    #endregion
}