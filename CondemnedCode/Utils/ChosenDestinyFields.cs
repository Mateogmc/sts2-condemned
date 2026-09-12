using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Utils;

public static class ChosenDestinyFields
{
    public static readonly SavedSpireField<CardModel, bool> HasPermanentInnate =
        new(() => false, "condemned_has_permanent_innate");
    public static readonly SavedSpireField<CardModel, bool> PermanentInnate =
        new(() => false, "condemned_permanent_innate");
}