using BaseLib.Utils;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Mockery : CondemnedCard
{
    public static decimal stacks = 1m;
    
    public Mockery() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithBlock(3, 2);
        WithVar(new MockeryVar(1));
        WithTip(typeof(JinxPower));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        
        decimal curseCount = Owner.PlayerCombatState.AllCards.Count(c => c.IsCurse() && c.Pile.Type != PileType.Exhaust);

        await CommonActions.Apply<JinxPower>(choiceContext, play.Target, this, stacks *  curseCount * (IsUpgraded ? 2 : 1));
    }
}