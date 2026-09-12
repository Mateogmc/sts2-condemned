using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.DynamicVars;

public class MindsEyeVar : DynamicVar
{
    public const string defaultName = "MindsEye";
    
    public MindsEyeVar(decimal baseValue) : base(defaultName, baseValue)
    { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal cardCount;

        if (card.Owner.PlayerCombatState != null)
        {
            cardCount = card.Owner.PlayerCombatState.Hand.Cards.Count - 1;
        }
        else return;
        
        PreviewValue = cardCount;
    }
}