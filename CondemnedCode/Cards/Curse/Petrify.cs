using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Condemned.CondemnedCode.Cards.Curse;

public class Petrify : CondemnedCard
{
    public Petrify() : base(-1, CardType.Curse, CardRarity.Curse, TargetType.Self)
    {
        WithBlock(8, 2);
        WithKeywords(CardKeyword.Unplayable);
        WithKeywords(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
    }
}