using BaseLib.Utils;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Powers;

public class CapablePower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override int DisplayAmount => _currentAmount;

    private int _currentAmount;
    public int CurrentAmount => _currentAmount;

    public override bool ShouldReceiveCombatHooks => true;

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.Keywords.Contains(CardKeyword.Unplayable) && _currentAmount > 0)
        {
            _currentAmount--;
            InvokeDisplayAmountChanged();
        }
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == CombatSide.Player)
        {
            _currentAmount = Amount;
            InvokeDisplayAmountChanged();
        }
        
        return Task.CompletedTask;
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power == this && applier == Owner)
        {
            _currentAmount += (int) amount;
            InvokeDisplayAmountChanged();
        }
        return Task.CompletedTask;
    }
}