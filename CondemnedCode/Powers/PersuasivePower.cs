using BaseLib.Utils;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Powers;

public class PersuasivePower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override bool ShouldReceiveCombatHooks => true;

    public override decimal ModifyBlockAdditive(Creature target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (cardSource is not { Type: CardType.Skill }) return 0m;

        return Amount * (cardSource.Keywords.Count + cardSource.BaseReplayCount > 0 ? 1 : 0);
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource is not { Type: CardType.Attack }) return 0m;

        return Amount * (cardSource.Keywords.Count + cardSource.BaseReplayCount > 0 ? 1 : 0);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Skill && !cardPlay.Card.DynamicVars.ContainsKey("Block"))
        {
            await CreatureCmd.GainBlock(
                cardPlay.Card.Owner.Creature,
                0, ValueProp.Unpowered,
                cardPlay);
        }

        if (cardPlay.Card.IsCurse())
        {
            await CommonActions.Apply<JinxPower>(choiceContext,
                Owner.CombatState.HittableEnemies.TakeRandom(1, Owner.Player.RunState.Rng.CombatTargets).First(),
                cardPlay.Card, Amount * (cardPlay.Card.Keywords.Count + (cardPlay.Card.BaseReplayCount > 0 ? 1 : 0)));
        }
    }
}