using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Curse;

public class Craving : CondemnedCard
{
    public Craving() : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
    {
        WithCards(1, 1);
        WithKeywords(CardKeyword.Unplayable);
        WithKeywords(CardKeyword.Exhaust);
    }
    
    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        return CommonActions.Draw(this, choiceContext);
    }
}