using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.DynamicVars;

public class HurtfulWordsVar : DynamicVar
{
    public const string defaultName = "HurtfulWords";
    
    public HurtfulWordsVar(decimal baseValue) : base(defaultName, baseValue)
    { }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal damage;

        if (card.Owner.PlayerCombatState != null)
        {
            decimal keywordCount = card.Keywords.Count;
            if (card.BaseReplayCount > 0) keywordCount++;
            
            damage = keywordCount * BaseValue;
        }
        else return;
        
        PreviewValue = damage;
    }
}