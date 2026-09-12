using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.DynamicVars;

public class AtoneVar : DynamicVar
{
    public const string defaultName = "Atone";
    
    public AtoneVar(decimal baseValue) : base(defaultName, baseValue)
    { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal totalDamage;

        if (card.Owner.PlayerCombatState != null)
        {
            if (target != null) 
                totalDamage = target.GetPowerAmount<JinxPower>() * BaseValue + card.Owner.Creature.GetPowerAmount<StrengthPower>();
            else
                totalDamage = card.DynamicVars.Damage.BaseValue;
        }
        else return;
        
        PreviewValue = totalDamage;
    }
}