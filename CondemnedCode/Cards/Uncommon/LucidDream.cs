using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class LucidDream : CondemnedCard
{
    public LucidDream() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        WithKeywords(CardKeyword.Ethereal);
        WithCostUpgradeBy(-1);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int amount = 1;

        var allCards = new CardPile(PileType.Play);
        foreach (var card in Owner.PlayerCombatState.ExhaustPile.Cards)
        {
            if (card == this) continue;
            allCards.AddInternal(card);
        }

        var selectedCards = await CardSelectCmd.FromCombatPile(
            choiceContext,
            allCards,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, amount)
        );

        var cards = selectedCards.ToList();

        foreach (CardModel card in cards)
        {
            await CardCmd.AutoPlay(choiceContext, card, null);
            await CardPileCmd.Add(card, PileType.Discard.GetPile(Owner));
        }
    }
}