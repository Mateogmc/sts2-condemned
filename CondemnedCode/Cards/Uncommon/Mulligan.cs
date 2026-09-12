using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
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

public class Mulligan : CondemnedCard
{
    public Mulligan() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        List<CardModel> hand = Owner.PlayerCombatState.Hand.Cards.ToList();

        foreach (CardModel card in hand)
        {
            await CardPileCmd.Add(card, PileType.Discard);
        }

        for (int i = 0; i < hand.Count; i++)
        {
            await CardPileCmd.Draw(choiceContext, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}