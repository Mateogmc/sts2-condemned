using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class Cheating : CondemnedCard
{
    public Cheating() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<CheatingPower>(1);
        WithCostUpgradeBy(-1);
        WithTip(CardKeyword.Unplayable);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<CheatingPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<CheatingPower>().BaseValue);
    }
}