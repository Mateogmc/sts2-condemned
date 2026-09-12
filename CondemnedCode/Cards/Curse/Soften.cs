using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Cards.Curse;

public class Soften : CondemnedCard
{
    public Soften() : base(-1, CardType.Curse, CardRarity.Curse, TargetType.AllEnemies)
    {
        WithPower<VulnerablePower>(1, 1);
        WithKeywords(CardKeyword.Unplayable);
        WithKeywords(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (Creature target in CombatState.HittableEnemies)
        {
            await CommonActions.Apply<VulnerablePower>(target, this, DynamicVars.Power<VulnerablePower>().BaseValue);
        }
    }
}