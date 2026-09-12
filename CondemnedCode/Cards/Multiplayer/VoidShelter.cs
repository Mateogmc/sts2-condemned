using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Token;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Multiplayer;

public class VoidShelter : CondemnedCard
{
    public VoidShelter() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<VoidShelterPower>(2, 1);
    }

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await CommonActions.Apply<VoidShelterPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<VoidShelterPower>().BaseValue);
    }
}