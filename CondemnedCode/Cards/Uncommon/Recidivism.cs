using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Recidivism : CondemnedCard
{
    public Recidivism() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(4, 3);
        WithKeyword(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        int amount = 1;

        List<CardModel> cardPileAddResultList;
        if (IsUpgraded)
        {
            var allCards = new CardPile(PileType.Play);
            foreach (var card in Owner.PlayerCombatState.AllCards.ToList().FindAll(c => c.Keywords.Contains(CardKeyword.Unplayable)))
            {
                if (card == this || card.Pile.Type == PileType.Exhaust) continue;
                allCards.AddInternal(card);
            }
            
            cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
                    new CardSelectorPrefs(SelectionScreenPrompt, amount))).ToList();
        }
        else
        {
            var allCards = new CardPile(PileType.Play);
            foreach (var card in Owner.PlayerCombatState.Hand.Cards.ToList().FindAll(c => c.Keywords.Contains(CardKeyword.Unplayable)))
            {
                if (card == this || card.Pile.Type == PileType.Exhaust) continue;
                allCards.AddInternal(card);
            }
            
            cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
                    new CardSelectorPrefs(SelectionScreenPrompt, amount))).ToList();
        }
            
        foreach (CardModel card in cardPileAddResultList)
        {
            card.RemoveKeyword(CardKeyword.Unplayable);
            await CardCmd.AutoPlay(choiceContext, card, null);
            card.RemoveKeyword(CardKeyword.Unplayable);
            await CardCmd.AutoPlay(choiceContext, card, null);
            card.AddKeyword(CardKeyword.Unplayable);
            
            await CardPileCmd.Add(card, PileType.Exhaust.GetPile(Owner));
        }
    }
}