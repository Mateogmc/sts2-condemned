using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.DynamicVars;

public class PurgeVar : DynamicVar
{
    public const string defaultName = "Purge";
    
    public PurgeVar(decimal baseValue) : base(defaultName, baseValue)
    { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal strength;

        if (card.Owner.PlayerCombatState != null)
        {
            decimal curseCount = card.Owner.PlayerCombatState.AllCards.Count(c => c.IsCurse() &&  c.Pile.Type != PileType.Exhaust);
            strength = curseCount * BaseValue;
        }
        else return;
        
        PreviewValue = strength;
    }
}