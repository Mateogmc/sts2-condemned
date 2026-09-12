using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class Persuasive : CondemnedCard
{
    public Persuasive() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<PersuasivePower>(3, 2);
        WithTip(typeof(PersuasivePower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<PersuasivePower>(choiceContext, Owner.Creature, this, DynamicVars.Power<PersuasivePower>().BaseValue);
    }
}