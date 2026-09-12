using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using Condemned.CondemnedCode.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Powers
{
public class ExilesVirtuePower : CondemnedPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (side != CombatSide.Player) return;
            if (!participants.Contains(Owner)) return;

            int jinxCount = 0;
            int cardCount = 0;

            foreach (CardModel card in Owner.Player.PlayerCombatState.Hand.Cards)
            {
                jinxCount += card.Keywords.Count + (card.BaseReplayCount > 0 ? 1 : 0);
                cardCount += card.IsCurse() ? 1 : 0;
            }

            jinxCount *= Amount;
            cardCount *= Amount;

            List<Creature> enemies = CombatState.HittableEnemies.ToList();

            foreach (Creature enemy in enemies)
            {
                await CommonActions.Apply<JinxPower>(new ThrowingPlayerChoiceContext(), enemy, null, jinxCount);
            }

            for (int i = 0; i < cardCount; i++)
            {
                await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), Owner.Player);
            }
        }
    }
}