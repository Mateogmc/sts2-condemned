using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class Solitude : CondemnedCard
{
    public Solitude() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithPower<SolitudePower>(1);
        WithCostUpgradeBy(-1);
        WithKeyword(CardKeyword.Unplayable);
    }

    protected override bool ShouldGlowGoldInternal => IsPlayable;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<SolitudePower>(choiceContext, Owner.Creature, this, DynamicVars.Power<SolitudePower>().BaseValue);
        PlayerCmd.EndTurn(Owner, false);
    }

    protected override bool IsPlayable => PileType.Hand.GetPile(Owner).Cards.Where(c => c.IsCurse() || c == this).Count() == PileType.Hand.GetPile(Owner).Cards.Count();
}