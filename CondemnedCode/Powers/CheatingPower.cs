using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Powers;

public class CheatingPower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override bool ShouldReceiveCombatHooks => true;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner == Owner.Player && card.Keywords.Contains(CardKeyword.Unplayable))
        {
            modifiedCost = 0;
            return true;
        }
        
        modifiedCost = originalCost;
        return false;
    }
}