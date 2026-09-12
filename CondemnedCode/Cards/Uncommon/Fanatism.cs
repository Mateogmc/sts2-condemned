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

public class Fanatism : CondemnedCard
{

    public Fanatism() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(10, 4);
        WithTip(typeof(JinxPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        int amount = play.Target.GetPowerAmount<JinxPower>();

        var attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_bloody_impact")
            .Execute(choiceContext);

        int movedJinx = 0;
        
        foreach (Creature enemy in CombatState.HittableEnemies)
        {
            if (enemy == play.Target) continue;

            if (enemy.HasPower<JinxPower>())
            {
                movedJinx += enemy.GetPowerAmount<JinxPower>();
                await CommonActions.Apply<JinxPower>(choiceContext, enemy, this, -enemy.GetPowerAmount<JinxPower>());
            } 
        }
        
        await CommonActions.Apply<JinxPower>(choiceContext, play.Target, this, movedJinx);
    }
}