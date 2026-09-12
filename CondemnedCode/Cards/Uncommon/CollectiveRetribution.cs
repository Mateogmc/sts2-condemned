using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class CollectiveRetribution : CondemnedCard
{
    public CollectiveRetribution() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(6, 3);
        WithVar("HitCountBase", 0);
        WithVar("HitCountExtra", 1);
        WithVar(
            new CustomCalculatedVar("HitCount")
                .WithMultiplier((card, target) => ((CardModel)card).Owner.Creature.CombatState.HittableEnemies.Count(c => c.HasPower<JinxPower>()))
        );
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        for (int i = 0; i < DynamicVars["HitCount"].PreviewValue; i++)
        {
            await CommonActions.CardAttack(this, play).WithHitFx("vfx/vfx_dramatic_stab").Execute(choiceContext);
        } 
    }
}