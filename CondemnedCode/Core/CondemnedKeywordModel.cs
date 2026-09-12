using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Utils;
using Condemned.CondemnedCode.Utils.Interfaces;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Core;

public class CondemnedKeywordModel() : CustomSingletonModel(HookType.Combat), ICardKeywordsModifiedListener
{
    public override bool ShouldReceiveCombatHooks => true;

    #region  FORETELL
    
    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        CardPile drawPile = shuffler.PlayerCombatState.DrawPile;
        List<CardModel> foretellable = new List<CardModel>();
        foreach (CardModel card in drawPile.Cards)
        {
            if (card.Keywords.Contains(CondemnedKeywords.Foretell))
            {
                foretellable.Add(card);
            }
        }

        foreach (CardModel card in foretellable)
        {
            if (card.Pile == drawPile)
            {
                await CardPileCmd.Add(card, shuffler.PlayerCombatState.Hand);
            }
            
        }
    }

    #endregion

    #region SLEIGHT
    
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Keywords.Contains(CondemnedKeywords.Sleight))
        {
            await CardPileCmd.Draw(choiceContext, 1, card.Owner);
        }
    }
    
    #endregion
    
    #region BRITTLE

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(CondemnedKeywords.Brittle))
        {
            await CardPileCmd.RemoveFromCombat(cardPlay.Card);
        }
    }

    #endregion

    #region RECHARGE

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(CondemnedKeywords.Recharge))
        {
            await PlayerCmd.GainEnergy(1m, cardPlay.Card.Owner);
        }
    }

    #endregion
    
    #region LOQUACITY

    public async void AfterCardKeywordsModified(CardModel card, CardKeyword keyword, bool added)
    {
        LoquacityDamage(card);
    }

    private static async void LoquacityDamage(CardModel card)
    {
        if (card.Owner.HasPower<LoquacityPower>())
        {
            List<Creature> enemies = card.CombatState.HittableEnemies.ToList();

            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(),
                enemies,
                card.Owner.Creature.GetPowerAmount<LoquacityPower>(), ValueProp.Unpowered, card.Owner.Creature);
        }
    }

    public static void TriggerCardKeywordsModified(CardModel card)
    {
        //LoquacityDamage(card);
    }

    #endregion

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != CombatSide.Player)
            return;

        foreach (Creature participant in participants)
        {
            List<CardModel> loopedCards = new List<CardModel>();

            if (participant.Player == null || participant.Player.PlayerCombatState == null)
                return;
            
            foreach (CardModel card in participant.Player.PlayerCombatState.AllCards.Where(c => c.Keywords.Contains(CondemnedKeywords.Looping) && c.Pile.Type != PileType.Exhaust && c.Pile.Type != PileType.Hand))
            {
                loopedCards.Add(card);
            }

            foreach (CardModel card in loopedCards)
            {
                await CardPileCmd.Add(card, PileType.Hand.GetPile(card.Owner));
            }
        }
    }
}