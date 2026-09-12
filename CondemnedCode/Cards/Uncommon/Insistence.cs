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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Insistence : CondemnedCard
{
    public Insistence() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        WithBlock(8, 2);
        WithCostUpgradeBy(-1);
        WithTip(CardKeyword.Exhaust);
        WithTip(StaticHoverTip.ReplayStatic);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        int amount = 1;
        
        try {

            List<CardModel> cardPileAddResultList;
            var allCards = new CardPile(PileType.Play);
            foreach (var card in Owner.PlayerCombatState.Hand.Cards.ToList())
            {
                if (card == this) continue;
                allCards.AddInternal(card);
            }
                
            cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, amount))).ToList();
                
            foreach (CardModel card in cardPileAddResultList)
            {
                card.AddKeyword(CardKeyword.Exhaust);
                card.BaseReplayCount++;
                CondemnedKeywordModel.TriggerCardKeywordsModified(this);
            }

        }
        catch (NullReferenceException e)
        {
            GD.Print($"[CONDEMNED] No target for Insistence");
        }
    }
}