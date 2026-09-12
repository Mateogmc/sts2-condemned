using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Powers;

public class ParanoiaPower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;
    
    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override bool ShouldReceiveCombatHooks => true;

    public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            CardPile hand = player.PlayerCombatState.Hand;

            for (int i = 0; i < Amount; i++)
            {
                List<CardModel> cardList = hand.Cards.Where(c => c.Type == CardType.Skill && (c.CanPlay(out var reason, out _) || (reason != UnplayableReason.NoLivingAllies && reason != UnplayableReason.BlockedByCardLogic)))
                    .TakeRandom(1, Owner.Player.RunState.Rng.CombatCardSelection).ToList();

                foreach (CardModel card in cardList)
                {
                    await CardCmd.AutoPlay(choiceContext, card, null);
                }
            }
        }
    }
}