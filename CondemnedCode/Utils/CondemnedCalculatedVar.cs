using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Utils;

public class CondemnedCalculatedVar : CalculatedVar
{
    public CondemnedCalculatedVar(string name) : base(name)
    {
    }

    protected override DynamicVar GetBaseVar() => ((CardModel) this._owner).DynamicVars[$"{Name}Base"];
    protected override DynamicVar GetExtraVar() => ((CardModel) this._owner).DynamicVars[$"{Name}Extra"];
}