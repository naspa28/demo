/*! \file */
/// <summary>
/// Method for checking user's tracking state on screen
/// </summary>
public class GazeDelegate
{
    /// <summary>
    /// Passing the gaze info value according to the frame
    /// </summary>
    /// <param name="gazeInfo">See <see cref="GazeInfo">GazeInfo</see></param>
    public delegate void onGaze(GazeInfo gazeInfo);
}