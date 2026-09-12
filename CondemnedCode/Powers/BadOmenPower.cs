using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Powers;

public class BadOmenPower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    
    public override bool ShouldReceiveCombatHooks => true;
    
    private int _currentStack;
    public override int DisplayAmount => _currentStack;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        _currentStack = Amount;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner.Player && cardPlay.Card.IsCurse())
        {
            if (_currentStack == 1)
            {
                await CardPileCmd.Draw(choiceContext, 1M, cardPlay.Card.Owner);
                _currentStack = Amount;
            }
            else
            {
                _currentStack--;
            }
            
            InvokeDisplayAmountChanged();
        }
    }
}