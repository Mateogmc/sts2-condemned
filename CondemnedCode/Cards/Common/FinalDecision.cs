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

public class FinalDecision : CondemnedCard
{
    public FinalDecision() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(6);
        WithCards(1, 1);
        WithTip(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        int amount = (int)DynamicVars.Cards.BaseValue;

        IReadOnlyList<CardPileAddResult> cardPileAddResultList = await CardPileCmd.Add(
            await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, amount)), PileType.Hand);
        foreach (CardPileAddResult card in cardPileAddResultList)
        {
            card.cardAdded.AddKeyword(CardKeyword.Exhaust);
            await CardPileCmd.Add(card.cardAdded, Owner.PlayerCombatState.Hand);
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
        }
    }
}