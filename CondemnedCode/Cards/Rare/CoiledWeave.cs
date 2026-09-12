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

namespace Condemned.CondemnedCode.Cards.Rare;

public class CoiledWeave : CondemnedCard
{
    public CoiledWeave() : base(2, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        WithTip(CondemnedKeywords.Looping);
        WithCostUpgradeBy(-1);
        WithKeyword(CondemnedKeywords.Brittle);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int amount = 1;

        IReadOnlyList<CardPileAddResult> cardPileAddResultList = await CardPileCmd.Add(
            await CardSelectCmd.FromCombatPile(choiceContext, PileType.Hand.GetPile(Owner), Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, amount)), PileType.Hand);
        foreach (CardPileAddResult card in cardPileAddResultList)
        {
            card.cardAdded.AddKeyword(CondemnedKeywords.Looping);
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
            CardCmd.Preview(card.cardAdded, 1f);
        }
    }
}