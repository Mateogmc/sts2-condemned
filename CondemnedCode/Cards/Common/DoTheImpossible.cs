using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Common;

public class DoTheImpossible : CondemnedCard
{
    public DoTheImpossible() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(5);
        WithCostUpgradeBy(-1);
        WithPower<DoTheImpossiblePower>(1);
        WithTip(CardKeyword.Unplayable);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CommonActions.Apply<DoTheImpossiblePower>(choiceContext, Owner.Creature, this);
    }
}