using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Curse;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Common;

public class MentalFortress : CondemnedCard
{
    public MentalFortress() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(12, 4);
        WithUpgradingCardTip<Petrify>();
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var curse = CombatState.CreateCard<Petrify>(Owner);
        if (IsUpgraded) CardCmd.Upgrade(curse);
        await CardPileCmd.AddGeneratedCardToCombat(curse, PileType.Hand, Owner);
    }
}