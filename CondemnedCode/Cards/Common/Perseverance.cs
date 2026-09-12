using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Condemned.CondemnedCode.Cards.Common;

public class Perseverance : CondemnedCard
{
    public Perseverance() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(10, 3);
        WithTip(typeof(Injury));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        int amount = 1;

        try
        {

            List<CardModel> cardPileAddResultList;
            if (IsUpgraded)
            {
                var allCards = new CardPile(PileType.Play);
                foreach (var card in Owner.PlayerCombatState.Hand.Cards.ToList().FindAll(c => c.Type != CardType.Curse))
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
                allCards.AddInternal(Owner.PlayerCombatState.Hand.Cards.ToList().FindAll(c => c.Type != CardType.Curse)
                    .TakeRandom(1, Owner.RunState.Rng.CombatCardSelection).FirstOrDefault());

                cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
                    new CardSelectorPrefs(SelectionScreenPrompt, amount))).ToList();
            }

            foreach (CardModel card in cardPileAddResultList)
            {
                CardModel injury = CombatState.CreateCard<Injury>(Owner);

                await CardCmd.Transform(card, injury);
            }

        }
        catch (NullReferenceException e)
        {
            GD.Print($"[CONDEMNED] No target for Survival");
        }
    }
}