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
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Powers
{
public class TarnishPower : CondemnedPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side != CombatSide.Player) return;
            if (!participants.Contains(Owner)) return;

            foreach (Creature creature in Owner.CombatState.HittableEnemies)
            {
                if (creature.HasPower<JinxPower>())
                {
                    int amount = Math.Min(Amount, creature.GetPowerAmount<JinxPower>());
                    
                    await CommonActions.Apply<JinxPower>(new ThrowingPlayerChoiceContext(), creature, null, -amount);
                    await CommonActions.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), creature, null, amount);
                }
            }
        }
    }
}