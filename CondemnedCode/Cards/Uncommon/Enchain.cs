using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Enchain : CondemnedCard
{
    public Enchain() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithPower<WeakPower>(2);
        WithPower<FrailPower>(2);
        WithCostUpgradeBy(-1);
        WithTip(typeof(WeakPower));
        WithTip(typeof(FrailPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        
        await CommonActions.Apply<WeakPower>(choiceContext, play.Target, this, DynamicVars.Power<WeakPower>().BaseValue);
        await CommonActions.Apply<FrailPower>(choiceContext, play.Target, this, DynamicVars.Power<FrailPower>().BaseValue);
    }
}