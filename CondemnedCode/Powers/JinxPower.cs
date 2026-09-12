using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Condemned.CondemnedCode.Powers;

public class JinxPower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
}