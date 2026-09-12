using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Condemned.CondemnedCode.Cards.Common;

public class Intent : CondemnedCard
{
    public Intent() : base(0, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        WithEnergy(2, 1);
        WithTip(typeof(Normality));
        WithTip(CardKeyword.Ethereal);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Owner.PlayerCombatState.GainEnergy(DynamicVars.Energy.BaseValue);
        
        var curse = CombatState.CreateCard<Normality>(Owner);
        curse.AddKeyword(CardKeyword.Ethereal);
        await CardPileCmd.AddGeneratedCardToCombat(curse, PileType.Hand, Owner);
    }
}