using BaseLib.Abstracts;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Vfx;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Core;

public class CondemnedVfxModel : CustomSingletonModel
{
    public CondemnedVfxModel() : base(HookType.Combat) { }

    public override bool ShouldReceiveCombatHooks => true;

    public override Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        GD.Print($"[VfxModel] AfterPowerAmountChanged called. Power: {power?.GetType().Name}, Amount: {amount}");
        if (power is JinxPower jinxPower)
        {
            GD.Print($"[VfxModel] JinxPower detected! Amount: {amount}, Owner: {jinxPower.Owner?.Name}");
            if (amount > 0)
            {
                GD.Print("[VfxModel] → Applying Jinx VFX");
                CondemnedVfx.ApplyJinx(jinxPower.Owner);
            }
            else if (amount < 0)
            {
                GD.Print("[VfxModel] → Consuming Jinx VFX");
                CondemnedVfx.ConsumeJinx(jinxPower.Owner, applier);
            }
        }

        return Task.CompletedTask;
    }
}