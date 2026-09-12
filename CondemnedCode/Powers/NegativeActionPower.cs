using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Powers;

public class NegativeActionPower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override bool ShouldReceiveCombatHooks => true;

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power == this)
        {
            foreach (CardModel card in cardSource.Owner.PlayerCombatState.AllCards)
            {
                if (card.IsCurse() && card.Keywords.Contains(CardKeyword.Unplayable))
                {
                    card.RemoveKeyword(CardKeyword.Unplayable);
                    CondemnedKeywordModel.TriggerCardKeywordsModified(card);
                }
            }
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Owner.HasPower<NegativeActionPower>() && card.IsCurse() && card.Keywords.Contains(CardKeyword.Unplayable))
        {
            card.RemoveKeyword(CardKeyword.Unplayable);
            CondemnedKeywordModel.TriggerCardKeywordsModified(card);
        }
        
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (CardModel card in Owner.Player.PlayerCombatState.AllCards)
        {
            if (card.IsCurse() && card.Keywords.Contains(CardKeyword.Unplayable))
            {
                card.RemoveKeyword(CardKeyword.Unplayable);
                CondemnedKeywordModel.TriggerCardKeywordsModified(card);
            }
        }

        return Task.CompletedTask;
    }
}