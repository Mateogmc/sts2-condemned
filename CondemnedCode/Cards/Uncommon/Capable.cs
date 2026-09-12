using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Odds;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Capable : CondemnedCard
{
    public Capable() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<CapablePower>(2);
        WithCostUpgradeBy(-1);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<CapablePower>(choiceContext, Owner.Creature, this, DynamicVars.Power<CapablePower>().BaseValue);
    }
}