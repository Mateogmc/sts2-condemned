using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Cards.Uncommon;
using Condemned.CondemnedCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.DynamicVars;

public class MockeryVar : DynamicVar
{
    public const string defaultName = "Mockery";
    
    public MockeryVar(decimal baseValue) : base(defaultName, baseValue)
    { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal curseCount;

        if (card.Owner.PlayerCombatState != null)
        {
            curseCount = card.Owner.PlayerCombatState.AllCards.Count(c => c.IsCurse() && c.Pile.Type != PileType.Exhaust);    
        }
        else
        {
            curseCount = card.Owner.Deck.Cards.Count(c => c.IsCurse());
        }
        
        PreviewValue = Mockery.stacks * curseCount * (card.IsUpgraded ? 2 : 1);
    }
}