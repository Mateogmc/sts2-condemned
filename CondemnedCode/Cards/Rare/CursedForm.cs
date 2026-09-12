using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class CursedForm : CondemnedCard
{
    public CursedForm() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<CursedFormPower>(1, 1);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await CommonActions.Apply<CursedFormPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<CursedFormPower>().BaseValue);
    }
}