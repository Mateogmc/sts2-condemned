using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Lacerate : CondemnedCard
{
    public Lacerate() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(12, 3);
        WithPower<VulnerablePower>(2, 1);
        WithTip(typeof(VulnerablePower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).WithHitFx("vfx/vfx_scratch_impact").Execute(choiceContext);

        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            await CommonActions.Apply<VulnerablePower>(hittableEnemy, this, DynamicVars.Power<VulnerablePower>().BaseValue);
        }
    }
}