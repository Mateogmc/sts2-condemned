using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Punishment : CondemnedCard
{
    public Punishment() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(3, 2);
        WithVar(new CalculationBaseVar(0));
        WithVar(new ExtraDamageVar(3).WithUpgrade(2));

        WithVar(
            new CalculatedDamageVar(ValueProp.Move)
                .WithMultiplier((card, target) =>
                    target?.GetPowerAmount<JinxPower>() ?? 0)
        );
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        int jinxAmount = play.Target.GetPowerAmount<JinxPower>();

        if (play.Target.HasPower<JinxPower>())
        {
            await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, play)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);

        }
    }
}