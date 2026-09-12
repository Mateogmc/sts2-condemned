using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class ReinforcedSpell : CondemnedCard
{
    public ReinforcedSpell() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ReinforcedSpellPower>(1, 1);
        WithTip(typeof(JinxPower));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<ReinforcedSpellPower>(choiceContext, Owner.Creature, DynamicVars.Power<ReinforcedSpellPower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}