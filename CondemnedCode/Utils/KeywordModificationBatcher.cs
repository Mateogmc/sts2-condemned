using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Utils;

public sealed class BatchState
{
    public int Depth;
    public readonly HashSet<CardModel> Pending =
        new(ReferenceEqualityComparer.Instance);
}

public static class KeywordModificationBatcher
{
    private static readonly AsyncLocal<BatchState?> _current = new();

    public static BatchState? Current => _current.Value;
    public static bool IsBatching    => _current.Value is not null;
    public static void SetCurrent(BatchState? s) => _current.Value = s;

    public static void Enter()
    {
        var s = _current.Value;
        if (s is null) _current.Value = new BatchState { Depth = 1 };
        else           s.Depth++;
    }

    public static void Exit(BatchState? s)
    {
        if (s is null || s.Depth == 0) return;
        s.Depth--;
        if (s.Depth > 0) return;
        Flush(s);
    }

    public static void Record(CardModel card) => _current.Value?.Pending.Add(card);

    private static void Flush(BatchState s)
    {
        foreach (var card in s.Pending)
            HookUtils.OnAfterCardKeywordsModified(
                card.RunState, card, CardKeyword.None, true);
        s.Pending.Clear();
    }
}