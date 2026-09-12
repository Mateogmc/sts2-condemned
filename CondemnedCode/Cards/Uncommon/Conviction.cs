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

public class Conviction : CondemnedCard
{
    public Conviction() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        WithVar("ConvictionVar", 4, 2);
        WithTip(CardKeyword.Exhaust);
        WithTip(CardKeyword.Ethereal);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<ConvictionPower>(choiceContext, hittableEnemy, DynamicVars["ConvictionVar"].BaseValue,
                Owner.Creature, this);
        }
        
        int amount = 1;
        
        try {

            List<CardModel> cardPileAddResultList;
            if (Owner.PlayerCombatState.Hand.Cards.Count <= 0) return;
            
            if (IsUpgraded)
            {
                var allCards = new CardPile(PileType.Play);
                foreach (var card in Owner.PlayerCombatState.Hand.Cards.Where(c => !(c.Keywords.Contains(CardKeyword.Ethereal) && c.Keywords.Contains(CardKeyword.Ethereal))).ToList())
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
                allCards.AddInternal(Owner.PlayerCombatState.Hand.Cards.ToList().FindAll(c => !(c.Keywords.Contains(CardKeyword.Ethereal) && c.Keywords.Contains(CardKeyword.Ethereal))).TakeRandom(1, Owner.RunState.Rng.CombatCardSelection).First());
                
                cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
                    new CardSelectorPrefs(SelectionScreenPrompt, amount))).ToList();
            }
                
            foreach (CardModel card in cardPileAddResultList)
            {
                card.AddKeyword(CardKeyword.Exhaust);
                card.AddKeyword(CardKeyword.Ethereal);
                CondemnedKeywordModel.TriggerCardKeywordsModified(this);
            }

        }
        catch (NullReferenceException e)
        {
            GD.Print($"[CONDEMNED] No target for Conviction");
        }
    }
}