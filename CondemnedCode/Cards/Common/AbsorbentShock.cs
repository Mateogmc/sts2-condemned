using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Common;

public class AbsorbentShock : CondemnedCard
{
    public AbsorbentShock() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(5, 3);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);

        int jinxPower = play.Target.GetPowerAmount<JinxPower>();
        
        if (jinxPower == 1)
        {
            await CommonActions.Apply<JinxPower>(play.Target, this, -1m);
            await PlayerCmd.GainEnergy(1m, Owner);
        }
        else if (jinxPower >= 2)
        {
            await CommonActions.Apply<JinxPower>(play.Target, this, -2m);
            await PlayerCmd.GainEnergy(2m, Owner);
        }
    }
}