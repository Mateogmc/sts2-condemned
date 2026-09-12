using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Exorcist : CondemnedCard
{
    public Exorcist() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ExorcistPower>(6, 3);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<ExorcistPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<ExorcistPower>().BaseValue);
    }
}