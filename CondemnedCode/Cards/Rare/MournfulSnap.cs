using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.StaticHoverTips;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Cards.Rare;

public class MournfulSnap : CondemnedCard
{

    public MournfulSnap() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithVar(new CalculationBaseVar(0));
        WithVar(new ExtraDamageVar(1));
        WithVar(
            new CalculatedDamageVar(ValueProp.Move)
                .WithMultiplier((card, target) =>
                {
                    int jinxCount = 0;

                    foreach (Creature enemy in ((CardModel)card).Owner.Creature.CombatState.HittableEnemies)
                    {
                        jinxCount += enemy.GetPowerAmount<JinxPower>();
                    }
                    
                    return jinxCount;
                })
        );
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).WithHitFx("vfx/hellraiser_attack_vfx").Execute(choiceContext);
    }
}