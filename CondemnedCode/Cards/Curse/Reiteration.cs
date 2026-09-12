using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Curse;

public class Reiteration : CondemnedCard
{
    public Reiteration() : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
    {
        WithTip(typeof(Reiteration));
        WithVar("CloneCount", 1, 1);
        WithKeywords(CardKeyword.Unplayable);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < DynamicVars["CloneCount"].IntValue; i++)
        {
            var curse = CreateClone();
            await CardPileCmd.AddGeneratedCardToCombat(curse, PileType.Discard, Owner);
        }
    }
}