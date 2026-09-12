using BaseLib.Utils;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Vfx;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Forms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Powers;

public class VoidShelterPower() : CondemnedPower
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
            Flash();
            foreach (Creature teammate in CombatState.GetTeammatesOf(Owner).Where(c => c is { IsAlive: true, IsPlayer: true }))
            {
                await CreatureCmd.GainBlock(teammate, Amount, ValueProp.Move, null);
            }
        }
    }
}