using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Condemned.CondemnedCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class SigilOfForetell : CondemnedCard
{
    public SigilOfForetell() : base(0, CardType.Skill, CardRarity.Token, TargetType.None)
    {
        WithVar("SigilCards", 1);
        WithKeyword(CardKeyword.Retain);
        WithKeyword(CondemnedKeywords.Brittle);
        
        WithTip(CondemnedKeywords.Foretell);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int amount = DynamicVars["SigilCards"].IntValue;
        
        foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, Owner,
                     new CardSelectorPrefs(SelectionScreenPrompt, amount), c => !c.Keywords.Contains(CondemnedKeywords.Foretell), this))
        {
            card.AddKeyword(CondemnedKeywords.Foretell);
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
        }
    }
}