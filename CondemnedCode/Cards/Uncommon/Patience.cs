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

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Patience : CondemnedCard
{
    public Patience() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        WithBlock(13, 3);
        WithVar("RetainCards", 1, 1);
        WithTip(CardKeyword.Retain);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        int amount = DynamicVars["RetainCards"].IntValue;

        foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, Owner,
                     new CardSelectorPrefs(SelectionScreenPrompt, amount), c => !c.Keywords.Contains(CardKeyword.Retain), this))
        {
            card.AddKeyword(CardKeyword.Retain);
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
            CardCmd.Preview(card, 1f);
        }
    }
}