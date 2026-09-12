using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Cards.Common;

public class MorallySuperior : CondemnedCard
{
    public MorallySuperior() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        WithDamage(6, 3);
        WithVar("ConsumedJinx", 1, 1);
        WithTip(typeof(JinxPower));
        WithTip(typeof(WeakPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);

        foreach (Creature creature in CombatState.HittableEnemies)
        {
            if (creature.HasPower<JinxPower>())
            {
                int consumedJinx = Math.Min(creature.GetPowerAmount<JinxPower>(), DynamicVars["ConsumedJinx"].IntValue);
                await CommonActions.Apply<JinxPower>(creature, this, -consumedJinx);
                await CommonActions.Apply<WeakPower>(creature, this, consumedJinx);
            }
        }
        
    }
}