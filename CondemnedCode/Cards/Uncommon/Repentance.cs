using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Curse;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Repentance : CondemnedCard
{
    public Repentance() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(8, 3);
        WithVar(new CalculationBaseVar(0M));
        WithVar(new CalculationExtraVar(1M));
        WithVar(new CalculatedVar("HitCount").WithMultiplier((card, target) =>
            CombatManager.Instance.History.CardPlaysFinished.Count(
                e =>
                    e.HappenedThisTurn(card.CombatState) && e.CardPlay.Card.Rarity == CardRarity.Curse &&
                    e.CardPlay.Player == card.Owner)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play)
            .WithHitCount((int)DynamicVars["HitCount"].PreviewValue)
            .WithHitFx("vfx/vfx_fire_burst")
            .Execute(choiceContext);
    }
}