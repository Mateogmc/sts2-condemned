using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Cards.Ancient;

public class Ostracism : CondemnedCard
{
    public Ostracism() : base(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
    {
        WithBlock(10, 5);
        WithCards(1, 1);
        WithVar("CursedCards", 2);
        WithTip(CondemnedKeywords.Cursed);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CommonActions.Draw(this, choiceContext);
        
        var allCards = new CardPile(PileType.Play);

        if (IsUpgraded)
        {
            foreach (var card in Owner.PlayerCombatState.AllCards.Where(c => !c.IsCurse()))
            {
                if (card == this || card.Pile.Type == PileType.Exhaust) continue;
                allCards.AddInternal(card);
            }
        }
        else
        {
            foreach (var card in Owner.PlayerCombatState.Hand.Cards.Where(c => !c.IsCurse()))
            {
                if (card == this || card.Pile.Type == PileType.Exhaust) continue;
                allCards.AddInternal(card);
            }
        }

        IEnumerable<CardModel> cardPileAddResultList = await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars["CursedCards"].IntValue));
        foreach (CardModel card in cardPileAddResultList)
        {
            card.AddKeyword(CondemnedKeywords.Cursed);
            CardCmd.Preview(card, 0.5f);
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
        }
    }
}