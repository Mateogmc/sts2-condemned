using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace Condemned.CondemnedCode.StaticHoverTips;

public class CondemnedStaticHoverTips
{
    [CustomEnum] public static StaticHoverTip Keywords;
    
    private static HoverTip GetStaticHoverTip(StaticHoverTip staticHoverTip)
    {
        string locEntry = $"CONDEMNED-{Enum.GetName(staticHoverTip).ToUpper()}";
        
        const string locTable = "static_hover_tips";
        return new HoverTip(
            new LocString(locTable, locEntry + ".title"), 
            new LocString(locTable, locEntry + ".description")
        );
    }
}