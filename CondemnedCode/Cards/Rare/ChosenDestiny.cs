using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Utils;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace Condemned.CondemnedCode.Cards.Rare;

public class ChosenDestiny : CondemnedCard
{
    public ChosenDestiny() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CondemnedKeywords.Brittle);
        WithCostUpgradeBy(-1);
        WithTip(CardKeyword.Innate);
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
            if (card == this || card.DeckVersion == null) continue;
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
            GD.Print(card);
            GD.Print(card.GetHashCode());
            
            if (!ChosenDestinyFields.HasPermanentInnate.Get(card)) ChosenDestinyFields.HasPermanentInnate.Set(card.DeckVersion, true);
            
            if (card.DeckVersion.Keywords.Contains(CardKeyword.Innate))
            {
                CardCmd.RemoveKeyword(card, CardKeyword.Innate);
                CardCmd.RemoveKeyword(card.DeckVersion, CardKeyword.Innate);
                ChosenDestinyFields.PermanentInnate.Set(card.DeckVersion, false);
            }
            else
            {
                CardCmd.ApplyKeyword(card, CardKeyword.Innate);
                CardCmd.ApplyKeyword(card.DeckVersion, CardKeyword.Innate);
                ChosenDestinyFields.PermanentInnate.Set(card.DeckVersion, true);
            }
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);

            CardCmd.Preview(card.DeckVersion);
        }
    }
}