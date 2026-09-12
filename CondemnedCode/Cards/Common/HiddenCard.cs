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

namespace Condemned.CondemnedCode.Cards.Common;

public class HiddenCard : CondemnedCard
{
    public HiddenCard() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(6, 2);
        WithCostUpgradeBy(-1);
        WithTip(CondemnedKeywords.Sleight);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        int amount = 1;

        foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, Owner,
                     new CardSelectorPrefs(SelectionScreenPrompt, amount), c => !c.Keywords.Contains(CondemnedKeywords.Sleight), this))
        {
            card.AddKeyword(CondemnedKeywords.Sleight);
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
        }
    }
}