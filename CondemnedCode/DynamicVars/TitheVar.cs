using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.DynamicVars;

public class TitheVar : DynamicVar
{
    public const string defaultName = "Tithe";
    
    public TitheVar(decimal baseValue) : base(defaultName, baseValue)
    { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal drawAmount = 0m;

        if (card.CombatState != null)
        {
            foreach (Creature c in card.CombatState.HittableEnemies)
            {
                if (c.HasPower<JinxPower>())
                    drawAmount++;
            }
        }
        
        PreviewValue = drawAmount;
    }
}