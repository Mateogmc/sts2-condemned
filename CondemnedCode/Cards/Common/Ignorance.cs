using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.StaticHoverTips;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Common;

public class Ignorance : CondemnedCard
{
    public Ignorance() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(6, 3);
        WithTip(CondemnedStaticHoverTips.Keywords);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        int amount = 1;
        
        try {

            List<CardModel> cardPileAddResultList;
            if (IsUpgraded)
            {
                var allCards = new CardPile(PileType.Play);
                foreach (var card in Owner.PlayerCombatState.AllCards.Where(c => c.Keywords.Count + (c.BaseReplayCount > 0 ? 1 : 0) > 0))
                {
                    if (card == this) continue;
                    allCards.AddInternal(card);
                }
                
                cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
                        new CardSelectorPrefs(SelectionScreenPrompt, amount))).ToList();
            }
            else
            {
                var allCards = new CardPile(PileType.Play);
                foreach (var card in Owner.PlayerCombatState.Hand.Cards.Where(c => c.Keywords.Count + (c.BaseReplayCount > 0 ? 1 : 0) > 0))
                {
                    if (card == this) continue;
                    allCards.AddInternal(card);
                }
                
                cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
                        new CardSelectorPrefs(SelectionScreenPrompt, amount))).ToList();
            }
                
            foreach (CardModel card in cardPileAddResultList)
            {
                List<CardKeyword> keywords = card.Keywords.ToList();

                foreach (CardKeyword keyword in keywords)
                {
                    if (keyword == CondemnedKeywords.Brittle) continue;
                    card.RemoveKeyword(keyword);
                }

                card.BaseReplayCount = 0;
                
                
                CondemnedKeywordModel.TriggerCardKeywordsModified(this);
                CardCmd.Preview(card, 0.3f);
            }

        }
        catch (NullReferenceException e)
        {
            GD.Print($"[CONDEMNED] No target for Survival");
        }
    }
}