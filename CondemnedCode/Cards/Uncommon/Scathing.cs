using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Odds;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Scathing : CondemnedCard
{
    public Scathing() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ScathingPower>(1);
        WithPower<JinxPower>(5);
        WithCostUpgradeBy(-1);
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
        
        await CommonActions.Apply<ScathingPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<ScathingPower>().BaseValue);
    }
}