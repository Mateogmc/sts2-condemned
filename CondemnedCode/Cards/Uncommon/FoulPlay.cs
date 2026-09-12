using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class FoulPlay : CondemnedCard
{
    public FoulPlay() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(6, 3);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(CondemnedKeywords.Sleight);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        
        if (IsUpgraded)
        {
            List<CardModel> curses = Owner.PlayerCombatState.AllCards.ToList().FindAll(c => c.IsCurse() && c.Pile.Type != PileType.Exhaust);
            
            foreach (CardModel curse in curses)
            {
                curse.AddKeyword(CondemnedKeywords.Sleight);
                CondemnedKeywordModel.TriggerCardKeywordsModified(this);
            }
        }
        else
        {
            List<CardModel> curses = Owner.PlayerCombatState.Hand.Cards.ToList().FindAll(c => c.IsCurse());
            
            foreach (CardModel curse in curses)
            {
                curse.AddKeyword(CondemnedKeywords.Sleight);
                CondemnedKeywordModel.TriggerCardKeywordsModified(this);
            }
        }
        
    }
}