using Condemned.CondemnedCode.Utils.Interfaces;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace Condemned.CondemnedCode.Utils;

public class HookUtils
{
    
    /// <summary>
    /// Defines a void method that accepts the current RunState as an argument.
    /// </summary>
    /// <remarks>
    /// Original implementation by Pikube.
    /// </remarks>
    public delegate void AfterRunInitializedHandler(RunState runState);
    /// <summary>
    /// Invoked immediately after a new run is either created or loaded from a save file. Used to perform custom initialization any time a game starts.
    /// </summary>
    /// <remarks>
    /// Original implementation by Pikube.
    /// </remarks>
    public static event AfterRunInitializedHandler? AfterRunInitialized;

    internal static void OnAfterRunInitialized(RunState runState)
    {
        AfterRunInitialized?.Invoke(runState);
        foreach (IRunInitializedListener listener in runState.IterateHookListeners(null).OfType<IRunInitializedListener>())
        {
            listener.AfterRunInitialized(runState);
        }
    }
    /// <summary>
    /// Invoked immediately after a saved run is loaded. Used to perform additional initialization after loading a saved game but not when a new game is created.
    /// </summary>
    /// <remarks>
    /// Original implementation by Pikube. Thank you.
    /// </remarks>
    public static event HookUtils.AfterRunLoadedFromSaveHandler? AfterRunLoadedFromSave;

    internal static void OnAfterRunLoadedFromSave(RunState runState, SerializableRun save)
    {
        HookUtils.AfterRunLoadedFromSaveHandler runLoadedFromSave = HookUtils.AfterRunLoadedFromSave;
        if (runLoadedFromSave != null)
            runLoadedFromSave(runState, save);
        foreach (IRunLoadedFromSaveListener fromSaveListener in runState.IterateHookListeners((ICombatState) null).OfType<IRunLoadedFromSaveListener>())
            fromSaveListener.AfterRunLoadedFromSave(runState, save);
    }
    
    /// <summary>
    /// Defines a void method that accepts the current RunState and a SerializableRun
    /// </summary>
    /// <remarks>
    /// Original implementation by Pikube.
    /// </remarks>
    public delegate void AfterRunLoadedFromSaveHandler(RunState runState, SerializableRun save);
    
    public static event AfterCardKeywordsModifiedHandler? AfterCardKeywordsModified;

    internal static void OnAfterCardKeywordsModified(IRunState runState, CardModel card, CardKeyword keyword, bool added)
    {
        if (runState == null) return;
        AfterCardKeywordsModified?.Invoke(card, keyword, added);
        GD.Print("[CONDEMNED] AfterCardKeywordsModified");
        foreach (ICardKeywordsModifiedListener listener in runState.IterateHookListeners(card.CombatState).OfType<ICardKeywordsModifiedListener>())
        {
            listener.AfterCardKeywordsModified(card, keyword, added);
            GD.Print(listener.GetType().FullName);
        }
    }
    
    public delegate void AfterCardKeywordsModifiedHandler(CardModel card, CardKeyword keyword, bool added);
}