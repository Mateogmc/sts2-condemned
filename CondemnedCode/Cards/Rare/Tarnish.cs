using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class Tarnish : CondemnedCard
{
    public Tarnish() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<TarnishPower>(1);
        WithTip(typeof(JinxPower));
        WithKeyword(CardKeyword.Ethereal);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<TarnishPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<TarnishPower>().BaseValue);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }
}