using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Rare;

public class MindsEye : CondemnedCard
{
    public MindsEye() : base(0, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        WithVar(new MindsEyeVar(0));
        WithKeywords(CondemnedKeywords.Brittle);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int amount = (int) DynamicVars["MindsEye"].PreviewValue;

        var allCards = new CardPile(PileType.Play);
        foreach (var card in Owner.PlayerCombatState.AllCards)
        {
            if (card == this || card.Pile.Type == PileType.Exhaust) continue;
            allCards.AddInternal(card);
        }

        var selectedCards = await CardSelectCmd.FromCombatPile(
            choiceContext,
            allCards,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, amount)
        );

        var toDraw = selectedCards.ToList();

        List<CardModel> toDiscard = Owner.PlayerCombatState.Hand.Cards.ToList();

        foreach (CardModel card in toDiscard)
        {
            await CardPileCmd.Add(card, Owner.PlayerCombatState.DiscardPile);
        }

        foreach (CardModel card in toDraw)
        {
            await CardPileCmd.Add(card, Owner.PlayerCombatState.Hand);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}