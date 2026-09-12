using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Cards.Uncommon;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.DynamicVars;

public class LookDownVar : DynamicVar
{
    public const string defaultName = "LookDown";
    
    public LookDownVar(decimal baseValue) : base(defaultName, baseValue)
    { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal jinxCount;

        if (card.Owner.PlayerCombatState != null)
        {
            if (target != null)
            {
                jinxCount = target.GetPowerAmount<JinxPower>() * card.DynamicVars.Block.BaseValue;
            }
            else
            {
                jinxCount = card.DynamicVars.Block.BaseValue;
            }
                
        }
        else
        {
            jinxCount = card.Owner.Deck.Cards.Count(c => c.IsCurse());
        }
        
        PreviewValue = Mockery.stacks * jinxCount * (card.IsUpgraded ? 2 : 1);
    }
}