using Condemned.CondemnedCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Extensions;

public static class CardModelExtensions
{
    public static bool IsCurse(this CardModel card)
    {
        return card.Type == CardType.Curse || card.Keywords.Contains(CondemnedKeywords.Cursed);
    }
}