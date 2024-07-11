/*! \file */ 
/// <summary>
/// Method for checking initialization state
/// <p>The callback function that calls when <b>GazeTracker.init</b> function is called.
/// </summary>
public class InitializationDelegate
{
    /// <summary>
    /// Returns a <b>InitializationErrorType.ERROR_NONE</b> when succeed.
    /// </summary>
    /// <param name="error"><see cref="InitailizationErrorType">InitializationErrorType</see></param>
    public delegate void onInitialized(InitializationErrorType error);
}

