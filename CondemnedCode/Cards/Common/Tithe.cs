using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Common;

public class Tithe : CondemnedCard
{
    public Tithe() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        WithDamage(5, 2);
        WithVar(new TitheVar(1));
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        decimal drawAmount = 0m;
        
        foreach (Creature c in CombatState.HittableEnemies)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(c)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
            if (c.HasPower<JinxPower>())
                drawAmount++;
        }

        if (drawAmount > 0)
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars["Tithe"].PreviewValue, Owner);
        }
    }
}