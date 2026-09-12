using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Purge : CondemnedCard
{
    public Purge() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("PurgeBase", 1, 1);
        WithVar(new PurgeVar(1).WithUpgrade(1));
        WithKeyword(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        List<CardModel> cards = GetAllCurses(Owner).ToList();
        
        int strengthAmount = (int) DynamicVars["Purge"].PreviewValue;

        foreach (CardModel card in cards)
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
        
        await CommonActions.ApplySelf<StrengthPower>(this, strengthAmount);
    }

    private IEnumerable<CardModel> GetAllCurses(Player owner)
    {
        return Owner.PlayerCombatState.AllCards.Where(c =>
            c.IsCurse() && c.Pile.Type != PileType.Exhaust);
    }
}