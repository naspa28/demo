/*! \file */ 
/// <summary>
/// Methods for checking start/stop state of gaze tracker itself
/// </summary>
public class StatusDelegate
{
    /// <summary>
    /// The function that automatically calls after <b>GazeTracker.startTracking()</b> succeed.
    /// <p> Actions like calibration, preview, etc. are available after it.
    /// </summary>
    public delegate void onStarted();
    /// <summary>
    ///   Error value will be <b>StatusError.ERROR_NONE</b> if gaze tracking stopped after
    ///   <b>GazeTracker.stopTracking()</b> called but different values for a different statuses.
    ///   <p> It works properly when <b>GazeTracker.startTracking()</b> explicitly called at the gaze tracker stopping process.
    /// </summary>
    /// <param name="error">See <see cref="StatusErrorType">StautsErrorType</see></param>
    public delegate void onStopped(StatusErrorType error);
}
