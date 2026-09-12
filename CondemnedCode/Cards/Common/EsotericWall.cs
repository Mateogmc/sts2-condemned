using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Cards.Common;

public class EsotericWall : CondemnedCard
{
    public EsotericWall() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(6, 2);
        WithVar(new EsotericWallVar(6).WithUpgrade(2));
        WithVar(new CalculationBaseVar(0));
        WithVar(new CalculationExtraVar(6).WithUpgrade(2));
        WithVar(new CalculatedBlockVar(ValueProp.Move)
            .WithMultiplier(EnemyMultiplier));
        WithPower<JinxPower>(2, 1);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        List<Creature> enemies = CombatState.HittableEnemies.ToList();

        foreach (Creature creature in enemies)
        {
            await CommonActions.Apply<JinxPower>(creature, this, DynamicVars.Power<JinxPower>().BaseValue);
        }
        
        await CommonActions.CardBlock(this, DynamicVars.CalculatedBlock, play);
    }
    
    private static decimal EnemyMultiplier(CardModel card, Creature? target)
    {
        return card.CombatState?.HittableEnemies.Count ?? 0;
    }
}