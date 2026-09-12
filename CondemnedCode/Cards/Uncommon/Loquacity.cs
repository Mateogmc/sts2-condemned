using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.StaticHoverTips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Loquacity : CondemnedCard
{
    public Loquacity() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<LoquacityPower>(6, 2);
        WithTip(CondemnedStaticHoverTips.Keywords);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<LoquacityPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<LoquacityPower>().BaseValue);
    }
}