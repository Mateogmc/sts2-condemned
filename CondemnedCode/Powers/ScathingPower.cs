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

namespace Condemned.CondemnedCode.Powers
{
    public class ScathingPower : CondemnedPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;

        public int CalculateTotalDamageNextTurn(Creature target)
        {
            int jinxAmount = target.GetPowerAmount<JinxPower>();
            return jinxAmount > 0 ? jinxAmount : 0;
        }

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (side != CombatSide.Player) return;
            if (!participants.Contains(Owner)) return;

            foreach (Creature creature in combatState.HittableEnemies)
            {
                int jinxAmount = creature.GetPowerAmount<JinxPower>();
                
                if (jinxAmount <= 0) continue;
                
                await CreatureCmd.Damage(
                    new ThrowingPlayerChoiceContext(),
                    creature,
                    jinxAmount,
                    ValueProp.Unpowered,
                    null,
                    null
                );
            }
        }
    }
}