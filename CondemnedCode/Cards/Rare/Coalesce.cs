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

namespace Condemned.CondemnedCode.Cards.Rare;

public class Coalesce : CondemnedCard
{
    public Coalesce() : base(1, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        WithTip(CondemnedStaticHoverTips.Keywords);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int amount = 2;

        var allCards = new CardPile(PileType.Play);

        if (IsUpgraded)
        {
            foreach (var card in Owner.PlayerCombatState.AllCards)
            {
                if (card == this || card.Pile.Type == PileType.Exhaust) continue;
                allCards.AddInternal(card);
            }
        }
        else
        {
            foreach (var card in Owner.PlayerCombatState.Hand.Cards)
            {
                if (card == this || card.Pile.Type == PileType.Exhaust) continue;
                allCards.AddInternal(card);
            }
        }
        

        var selectedCards = await CardSelectCmd.FromCombatPile(
            choiceContext,
            allCards,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, amount)
        );

        var cards = selectedCards.ToList();
        
        List<CardKeyword> keywords = cards[0].Keywords.ToList().Union(cards[1].Keywords.ToList()).ToList();

        bool aChanged = false;
        bool bChanged = false;

        foreach (CardKeyword keyword in keywords)
        {
            if (!cards[0].Keywords.Contains(keyword))
            {
                cards[0].AddKeyword(keyword);
                aChanged = true;
            }

            if (!cards[1].Keywords.Contains(keyword))
            {
                cards[1].AddKeyword(keyword);
                bChanged = true;
            }
        }
        
        int totalReplay = Math.Max(cards[0].BaseReplayCount, cards[1].BaseReplayCount);

        if (cards[0].BaseReplayCount != totalReplay)
        {
            cards[0].BaseReplayCount = totalReplay;
            aChanged = true;
        }

        if (cards[1].BaseReplayCount != totalReplay)
        {
            cards[1].BaseReplayCount = totalReplay;
            bChanged = true;
        }
        
        if (aChanged)
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
        if (bChanged)
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}