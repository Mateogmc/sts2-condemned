using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Curse;
using Condemned.CondemnedCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class RecurringNightmare : CondemnedCard
{
    public RecurringNightmare() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithUpgradingCardTip<Reiteration>();
        WithKeywords(CardKeyword.Exhaust);
    }
    
    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var curse = CombatState.CreateCard<Reiteration>(Owner);
        return CardPileCmd.AddGeneratedCardToCombat(curse, PileType.Hand, Owner);
    }
}