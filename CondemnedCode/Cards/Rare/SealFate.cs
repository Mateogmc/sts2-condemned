using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Rare;

public class SealFate : CondemnedCard
{
    public SealFate() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithVar("SealFateMultiplier", 2, 1);
        WithKeywords(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (Creature enemy in CombatState.HittableEnemies)
        {
            decimal finalJinx = enemy.GetPowerAmount<JinxPower>() * DynamicVars["SealFateMultiplier"].BaseValue - enemy.GetPowerAmount<JinxPower>();
            
            await CommonActions.Apply<JinxPower>(choiceContext, enemy, this, finalJinx);
        }
    }
}