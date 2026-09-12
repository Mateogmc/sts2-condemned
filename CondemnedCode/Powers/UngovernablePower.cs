using BaseLib.Utils;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Powers;

public class UngovernablePower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override bool ShouldReceiveCombatHooks => true;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.IsCurse())
        {
            CardPile hand = Owner.Player.PlayerCombatState.Hand;

            for (int i = 0; i < 1; i++)
            {
                List<CardModel> cardList = hand.Cards.Where(c => (c.CanPlay(out var reason, out _) || (reason != UnplayableReason.NoLivingAllies && reason != UnplayableReason.BlockedByCardLogic)))
                    .TakeRandom(1, Owner.Player.RunState.Rng.CombatCardSelection).ToList();

                foreach (CardModel card in cardList)
                {
                    await CardCmd.AutoPlay(choiceContext, card, null);
                }
            }

            for (int i = 0; i < Amount; i++)
            {
                await CardPileCmd.Draw(choiceContext, Owner.Player);
            }
        }
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            PowerCmd.Remove<UngovernablePower>(Owner);
        }
        
        return Task.CompletedTask;
    }
}