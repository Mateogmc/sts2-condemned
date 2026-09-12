using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.StaticHoverTips;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Event;

public class Wax : CondemnedCard
{
    public Wax() : base(2, CardType.Skill, CardRarity.Event, TargetType.Self)
    {
        WithKeyword(CondemnedKeywords.Brittle);
        WithCostUpgradeBy(-1);
        WithTip(CardKeyword.Innate);
        WithTip(CondemnedStaticHoverTips.Keywords);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int amount = 1;

        List<CardModel> cardPileAddResultList;
        var allCards = new CardPile(PileType.Play);
        foreach (var card in Owner.PlayerCombatState.Hand.Cards.ToList())
        {
            if (card == this) continue;
            allCards.AddInternal(card);
        }
            
        cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, amount))).ToList();
        
        foreach (var deckCard in Owner.Deck.Cards)
        {
            GD.Print(deckCard);
            GD.Print(deckCard.GetHashCode());
        }

        foreach (CardModel card in cardPileAddResultList)
        {
            foreach (CardKeyword keyword in card.DeckVersion.Keywords.Except(card.Keywords))
            {
                card.DeckVersion.RemoveKeyword(keyword);
            }

            foreach (CardKeyword keyword in card.Keywords.Except(card.DeckVersion.Keywords))
            {
                card.DeckVersion.AddKeyword(keyword);
            }

            card.DeckVersion.BaseReplayCount = card.BaseReplayCount;

            CardCmd.Preview(card.DeckVersion);
        }
    }
}