using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.StaticHoverTips;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class ChooseACard : CondemnedCard
{
    public ChooseACard() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        WithBlock(3, 1);
        WithKeywords(CondemnedKeywords.Brittle);
        WithTip(CondemnedKeywords.Foretell);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        
        int amount = 1;

        var allCards = new CardPile(PileType.Play);

        foreach (CardModel card in Owner.PlayerCombatState.DiscardPile.Cards.Where(c => !c.Keywords.Contains(CondemnedKeywords.Foretell)))
        {
            allCards.AddInternal(card);
        }

        var selectedCards = await CardSelectCmd.FromCombatPile(
            choiceContext,
            allCards,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, amount)
        );

        foreach (CardModel card in selectedCards)
        {
            card.AddKeyword(CondemnedKeywords.Foretell);
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
        }
        
        await CardPileCmd.Shuffle(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CondemnedKeywords.Brittle);
        AddKeyword(CardKeyword.Exhaust);
    }
}