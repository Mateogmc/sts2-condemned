using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Vfx;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Transfusion : CondemnedCard
{

    public Transfusion() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVar(new CalculationBaseVar(0));
        WithVar(new ExtraDamageVar(2).WithUpgrade(1));
        WithVar(new CalculatedDamageVar(ValueProp.Move)
            .WithMultiplier((card, target) =>
                target?.GetPowerAmount<JinxPower>() ?? 0
            ));
        WithTip(typeof(JinxPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        int amount = play.Target.GetPowerAmount<JinxPower>();

        var attackCommand = await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, play)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_bloody_impact")
            .Execute(choiceContext);

        if (attackCommand.Results.SelectMany(r => r).Any(r => r.WasTargetKilled))
        {

            if (amount > 0)
            {
                try
                {
                    Creature newTarget = CombatState.HittableEnemies.Where(c => c != play.Target)
                        .TakeRandom(1, Owner.RunState.Rng.CombatTargets).First();
                    await CommonActions.Apply<JinxPower>(choiceContext, newTarget, this, amount);
                }
                catch (ArgumentNullException ex)
                {
                    GD.Print($"[CONDEMNED] {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    GD.Print($"[CONDEMNED] {ex.Message}");
                }
                
            }
        }
    }
}