using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Keywords;

public class CondemnedKeywords
{
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Foretell;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Sleight;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Brittle;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Recharge;
    
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Cursed;
    
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Looping;
}