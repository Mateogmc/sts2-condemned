using MegaCrit.Sts2.Core.Runs;

namespace Condemned.CondemnedCode.Utils.Interfaces;

/// <summary>
/// Interface that allows models to listen for when a new run is initialized
/// </summary>
/// <remarks>
/// Original implementation by Pikube.
/// </remarks>
public interface IRunInitializedListener
{
    /// <summary>
    /// Invoked immediately after a new run is either created or loaded from a save file. Used to perform custom initialization any time a game starts.
    /// </summary>
    /// <param name="runState">The current RunState</param>
    /// <remarks>
    /// Original implementation by Pikube.
    /// </remarks>
    public void AfterRunInitialized(RunState runState);
}