using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Utils.Interfaces;

public interface ICardKeywordsModifiedListener
{
    public void AfterCardKeywordsModified(CardModel card, CardKeyword keyword, bool added);
}