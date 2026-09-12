using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.StaticHoverTips;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Transfer : CondemnedCard
{
    public Transfer() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        WithBlock(7, 3);
        WithKeywords(CondemnedKeywords.Brittle);
        WithTip(CondemnedStaticHoverTips.Keywords);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        
        int amount = 2;

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

        var cards = selectedCards.ToList();

        if (cards.Count >= 2)
        {
            var cardA = cards[0];
            var cardB = cards[1];

            var exclude = new[] { CondemnedKeywords.Brittle };

            var keywordsA = cardA.Keywords.Where(k => !exclude.Contains(k)).ToList();
            var keywordsB = cardB.Keywords.Where(k => !exclude.Contains(k)).ToList();

            int repeatCountA = cardA.BaseReplayCount;
            int repeatCountB = cardB.BaseReplayCount;

            foreach (var kw in keywordsA) cardA.RemoveKeyword(kw);
            foreach (var kw in keywordsB) cardB.RemoveKeyword(kw);

            foreach (var kw in keywordsB) cardA.AddKeyword(kw);
            foreach (var kw in keywordsA) cardB.AddKeyword(kw);
            
            cardA.BaseReplayCount = repeatCountB;
            cardB.BaseReplayCount = repeatCountA;

            /*
            if (keywordsA != keywordsB || repeatCountA != repeatCountB)
            {
                CondemnedKeywordModel.TriggerCardKeywordsModified(this);
                CondemnedKeywordModel.TriggerCardKeywordsModified(this);
            }*/
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}