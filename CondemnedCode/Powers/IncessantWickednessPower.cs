using BaseLib.Utils;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Powers;

public class IncessantWickednessPower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override bool ShouldReceiveCombatHooks => true;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    
    private int _currentStack;

    public override int DisplayAmount => _currentStack;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        _currentStack = Amount;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power is JinxPower && amount > 0 && applier == Owner)
        {
            if (_currentStack == 1)
            {
                _currentStack = Amount;
                await CardPileCmd.Draw(choiceContext, Owner.Player);
            }
            else
                _currentStack--;
            
            InvokeDisplayAmountChanged();
        }
    }
}