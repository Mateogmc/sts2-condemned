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

namespace Condemned.CondemnedCode.Powers;

public class CursedFormPower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override bool ShouldReceiveCombatHooks => true;
    
    
    private NCursedFormVfx? _vfx;

    private NCursedFormVfx? Vfx
    {
        get => this._vfx == null || this._vfx.IsValid() ? this._vfx : (NCursedFormVfx) null;
        set
        {
            this.AssertMutable();
            this._vfx = value;
        }
    }
    
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        Vfx = NCursedFormVfx.Create(this.Owner);
        return Task.CompletedTask;
    }

    public override Task AfterRemoved(Creature oldOwner)
    {
        Vfx?.SetActive(false);
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.IsCurse())
        {
            Flash();
            foreach (Creature enemy in CombatState.HittableEnemies)
            {
                CommonActions.Apply<JinxPower>(choiceContext, enemy, cardPlay.Card, Amount);
            }
        }
        
        return Task.CompletedTask;
    }
}