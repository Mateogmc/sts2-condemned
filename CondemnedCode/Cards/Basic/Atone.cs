using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Utils;
using Condemned.Condemned.Scenes.Vfx;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Vfx;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Cards.Basic;

public class Atone : CondemnedCard
{
    public Atone() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(8, 2);
        WithVar(new CalculationBaseVar(8).WithUpgrade(2));
        WithVar(new ExtraDamageVar(2).WithUpgrade(1));
        WithVar(
            new CalculatedDamageVar(ValueProp.Move)
                .WithMultiplier((CardModel _, Creature? target) => target?.GetPowerAmount<JinxPower>() ?? 0)
            );
        
        WithVar(new DynamicVar("JinxCountBase", 0));
        WithVar(new DynamicVar("JinxCountExtra", 1));
        WithVar(
            new CondemnedCalculatedVar("JinxCount")
                .WithMultiplier((CardModel card, Creature? _) => 
                    card.Owner.PlayerCombatState.AllCards.Count(c => c.Pile.Type != PileType.Exhaust && c.IsCurse())
                    )
        );
        
        WithVar(new DynamicVar("PreviewedDamageBase", 8).WithUpgrade(2));
        WithVar(new DynamicVar("PreviewedDamageExtra", 2).WithUpgrade(1));
        WithVar(
            new CondemnedCalculatedDamageVar("PreviewedDamage", ValueProp.Move)
                .WithMultiplier((CardModel card, Creature? target) =>
                    {
                        int jinxCount = card.Owner.PlayerCombatState.AllCards.Count(c => c.Pile.Type != PileType.Exhaust && c.IsCurse());
                    return
                        (target?.GetPowerAmount<JinxPower>() ?? 0) + (target != null ? (target.HasPower<ArtifactPower>() ? 0 : jinxCount) : jinxCount) + card.Owner.Creature.GetPowerAmount<ReinforcedSpellPower>();
                }
                )
        );
        
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        
        await CommonActions.Apply<JinxPower>(choiceContext, play.Target, this, DynamicVars["JinxCount"].PreviewValue);

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, play)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }
}