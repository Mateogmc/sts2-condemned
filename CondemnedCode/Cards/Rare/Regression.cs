using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class Regression : CondemnedCard
{
    public Regression() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<RegressionPower>(1, 1);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await CommonActions.Apply<RegressionPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<RegressionPower>().BaseValue);
    }
}