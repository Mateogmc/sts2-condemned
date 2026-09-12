using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.StaticHoverTips;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Ancient;

public class ExilesVirtue : CondemnedCard, ITomeCard
{
    public ExilesVirtue() : base(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
        WithPower<ExilesVirtuePower>(1);
        WithTip(CondemnedStaticHoverTips.Keywords);
        WithCostUpgradeBy(-1);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<ExilesVirtuePower>(choiceContext, Owner.Creature, this, DynamicVars.Power<ExilesVirtuePower>().BaseValue);
    }
}