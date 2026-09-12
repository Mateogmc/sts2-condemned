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
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Powers
{
public class ConsumeFromWithinPower : CondemnedPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        
        private CardModel? _source;
        
        public override Task AfterApplied(Creature? applier, CardModel? cardSource)
        {
            _source = cardSource;
            
            return Task.CompletedTask;
        }

        public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side != CombatSide.Player) return;
            if (!participants.Contains(Owner)) return;

            foreach (Creature creature in Owner.CombatState.HittableEnemies)
            {
                int jinxAmount = creature.GetPowerAmount<JinxPower>();
                
                if (jinxAmount <= 0) continue;

                await CommonActions.Apply<JinxPower>(choiceContext, creature, _source, -1);
                
                await CreatureCmd.Damage(
                    new ThrowingPlayerChoiceContext(),
                    creature,
                    Amount,
                    ValueProp.Unpowered,
                    _source,
                    null
                );
            }
        }
    }
}