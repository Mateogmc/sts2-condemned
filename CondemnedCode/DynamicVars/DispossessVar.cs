using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.DynamicVars;

public class DispossessVar : DynamicVar
{
    public const string defaultName = "Dispossess";
    
    public DispossessVar(decimal baseValue) : base(defaultName, baseValue)
    { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal damage;

        if (card.Owner.PlayerCombatState != null)
        {
            decimal curseCount = card.Owner.PlayerCombatState.AllCards.Count(c => c.IsCurse() && c.Pile.Type != PileType.Exhaust);
            damage = curseCount * BaseValue;
        }
        else return;
        
        PreviewValue = damage;
    }
}