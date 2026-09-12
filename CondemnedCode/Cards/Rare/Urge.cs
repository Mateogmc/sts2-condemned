using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Curse;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class Urge : CondemnedCard
{
    public Urge() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<UrgePower>(1);
        WithPower<UrgePlusPower>(1);
        WithUpgradingCardTip<Craving>();
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (IsUpgraded)
        {
            await CommonActions.Apply<UrgePlusPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<UrgePlusPower>().BaseValue);
        }
        else
        {
            await CommonActions.Apply<UrgePower>(choiceContext, Owner.Creature, this, DynamicVars.Power<UrgePower>().BaseValue);
        }
    }
}