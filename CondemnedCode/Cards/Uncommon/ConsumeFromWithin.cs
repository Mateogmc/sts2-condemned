using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class ConsumeFromWithin : CondemnedCard
{
    public ConsumeFromWithin() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ConsumeFromWithinPower>(6, 2);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<ConsumeFromWithinPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<ConsumeFromWithinPower>().BaseValue);
    }
}