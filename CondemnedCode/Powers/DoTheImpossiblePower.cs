using BaseLib.Utils;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Powers;

public class DoTheImpossiblePower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override bool ShouldReceiveCombatHooks => true;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.Keywords.Contains(CardKeyword.Unplayable) &&
            !(Owner.HasPower<CapablePower>() && Owner.GetPower<CapablePower>().CurrentAmount > 0))
        {
            await CommonActions.Apply<DoTheImpossiblePower>(choiceContext, Owner, cardPlay.Card, -1m);
        }
    }
}