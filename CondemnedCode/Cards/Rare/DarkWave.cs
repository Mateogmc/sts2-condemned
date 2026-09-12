using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Vfx;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Cards.Rare;

public class DarkWave : CondemnedCard
{
    public DarkWave() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(5, 2);
        WithPower<StrengthPower>(1);
        WithPower<JinxPower>(1, 1);
        WithCards(1);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CondemnedVfx.DarkWave(Owner.Creature);
        await CommonActions.CardAttack(this, play).Execute(choiceContext);

        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            await CommonActions.Apply<JinxPower>(choiceContext, hittableEnemy, this, DynamicVars.Power<JinxPower>().BaseValue);
            await CommonActions.Apply<StrengthPower>(choiceContext, hittableEnemy, this,  -DynamicVars.Power<StrengthPower>().BaseValue);
        }

        await CommonActions.Draw(this, choiceContext);
    }
}