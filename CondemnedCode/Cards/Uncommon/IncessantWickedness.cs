using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class IncessantWickedness : CondemnedCard
{
    public IncessantWickedness() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<IncessantWickednessPower>(3);
        WithCostUpgradeBy(-1);
        WithCards(1);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<IncessantWickednessPower>(choiceContext, this, DynamicVars.Power<IncessantWickednessPower>().BaseValue);
    }
}