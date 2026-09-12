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
using Condemned.CondemnedCode.Cards.Curse;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Condemned.CondemnedCode.Powers
{
public class UrgePower : CondemnedPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (side != CombatSide.Player) return;
            if (!participants.Contains(Owner)) return;

            var curse = CombatState.CreateCard<Craving>(Owner.Player);
            await CardPileCmd.AddGeneratedCardToCombat(curse, PileType.Hand, Owner.Player);
        }
    }
}