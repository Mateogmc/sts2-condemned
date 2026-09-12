using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;

namespace Condemned.CondemnedCode.Utils;

public class CondemnedCalculatedDamageVar : CalculatedVar
{
    public ValueProp Props { get; }

    public CondemnedCalculatedDamageVar(string name, ValueProp props)
        : base(name)
    {
        Props = props;
    }

    protected override DynamicVar GetBaseVar()
    {
        return ((CardModel)_owner).DynamicVars[$"{Name}Base"];
    }

    protected override DynamicVar GetExtraVar()
    {
        return ((CardModel)_owner).DynamicVars[$"{Name}Extra"];
    }

    public override void UpdateCardPreview(
        CardModel card,
        CardPreviewMode previewMode,
        Creature? target,
        bool runGlobalHooks)
    {
        Decimal num = Calculate(target);

        if (runGlobalHooks)
        {
            ICombatState combatState =
                card.CombatState ?? card.Owner.Creature.CombatState;

            PreviewValue = Hook.ModifyDamage(
                card.Owner.RunState,
                combatState,
                target,
                card.Owner.Creature,
                num,
                Props,
                card,
                null,
                ModifyDamageHookType.All,
                previewMode,
                out IEnumerable<AbstractModel> _
            );
        }
        else
        {
            PreviewValue = num;
        }

        PreviewValue = Math.Max(PreviewValue, 0M);
    }
}