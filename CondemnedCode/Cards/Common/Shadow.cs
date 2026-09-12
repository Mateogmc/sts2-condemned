using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Common;

public class Shadow : CondemnedCard
{
    public Shadow() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithBlock(8, 2);
        WithVar("JinxValue", 1, 1);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await CommonActions.CardBlock(this, play);
        await CommonActions.Apply<JinxPower>(play.Target, this, DynamicVars["JinxValue"].BaseValue);
    }
}