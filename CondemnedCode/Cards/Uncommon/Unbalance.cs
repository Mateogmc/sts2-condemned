using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Cards.Common;

public class Unbalance : CondemnedCard
{
    public Unbalance() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6, 3);
        WithPower<FrailPower>(1);
        WithTip(typeof(FrailPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_blunt") .Execute(choiceContext);
        if (play.Target.GetPowerAmount<JinxPower>() > 0)
        {
            await CommonActions.Apply<JinxPower>(play.Target, this, -1m);
            await CommonActions.Apply<FrailPower>(play.Target, this, DynamicVars.Power<FrailPower>().BaseValue);
        }
    }
}