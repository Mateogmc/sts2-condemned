using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.Condemned.Scenes.Vfx;
using Condemned.CondemnedCode.Cards.Ancient;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Vfx;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Cards.Basic;

public class Disparage : CondemnedCard, ITranscendenceCard
{
    public static decimal stacks = 1m;
    
    public Disparage() : base(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithBlock(6, 2);
        WithTip(CondemnedKeywords.Cursed);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        int amount = 1;
        
        var allCards = new CardPile(PileType.Play);
        
        List<CardModel> cardPileAddResultList = new List<CardModel>();

        if (IsUpgraded)
        {
            foreach (var card in Owner.PlayerCombatState.AllCards.Where(c => !c.IsCurse()))
            {
                if (card == this || card.Pile.Type == PileType.Exhaust) continue;
                allCards.AddInternal(card);
            }
            
            cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, allCards, Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, amount))).ToList();
        }
        else
        {
            cardPileAddResultList = (await CardSelectCmd.FromHand(choiceContext, Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, amount), c => !c.IsCurse(), this)).ToList();
        }

        
        foreach (CardModel card in cardPileAddResultList)
        {
            card.AddKeyword(CondemnedKeywords.Cursed);
            CardCmd.Preview(card, 0.5f);
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);
        }
    }

    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<Ostracism>();
}