using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class Rot : CondemnedCard
{
    public Rot() : base(4, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithPower<JinxPower>(25, 10);
        WithTip(typeof(JinxPower));
        WithKeyword(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await CommonActions.Apply<JinxPower>(choiceContext, play.Target, this, DynamicVars.Power<JinxPower>().BaseValue);
    }
}