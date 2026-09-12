using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class Drown : CondemnedCard
{
    public Drown() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<DrownPower>(1);
        WithCostUpgradeBy(-1);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<DrownPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<DrownPower>().BaseValue);
    }
}