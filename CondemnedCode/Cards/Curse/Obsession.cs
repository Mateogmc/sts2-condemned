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

public class Obsession : CondemnedCard
{
    public static decimal stacks = 1m;

    public Obsession() : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
    {
        WithCards(1, 1);
        WithKeywords(CardKeyword.Unplayable);
    }

    /*
    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var curse = CombatState.CreateCard<Clumsy>(Owner);
        curse.RemoveKeyword(CardKeyword.Unplayable);
        return CardPileCmd.AddGeneratedCardToCombat(curse, PileType.Hand, Owner);
    }*/

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card == this)
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }
}