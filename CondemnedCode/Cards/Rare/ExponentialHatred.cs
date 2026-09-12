using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class ExponentialHatred : CondemnedCard
{
    public ExponentialHatred() : base(-1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithPower<JinxPower>(1);
        WithKeyword(CondemnedKeywords.Brittle);
        WithTip(typeof(JinxPower));
    }

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int count = ResolveEnergyXValue();
        if (IsUpgraded) count++;

        for (int i = 0; i < count; i++)
        {
            await CommonActions.Apply<JinxPower>(choiceContext, play.Target, this, DynamicVars.Power<JinxPower>().BaseValue * count);
        }
    }
}