using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.DynamicVars;

public class EsotericWallVar : DynamicVar
{
    public const string defaultName = "EsotericWall";
    
    public EsotericWallVar(decimal baseValue) : base(defaultName, baseValue)
    { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal blockAmount = 0;

        if (card.CombatState != null)
        {
            List<Creature> enemies = card.CombatState.HittableEnemies.ToList();
            int blockTimes = 0;

            foreach (Creature creature in enemies)
            {
                blockTimes++;
            }
            
            blockAmount = blockTimes * BaseValue;
        }
        
        PreviewValue = blockAmount;
    }
}