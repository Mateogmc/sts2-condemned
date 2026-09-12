using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Common;

public class LesserEvil : CondemnedCard
{
    public LesserEvil() : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithBlock(4, 2);
        WithPower<JinxPower>(2, 2);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        await CommonActions.Apply<JinxPower>(choiceContext, play.Target, this, DynamicVars.Power<JinxPower>().BaseValue);
    }
}