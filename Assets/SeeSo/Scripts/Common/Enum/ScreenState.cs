/*! \file */ 
/// <summary>
/// The enum that contains state types using at <b>GazeDelegate</b>.
/// </summary>
/// <b>Note</b>
/// - <b>INSIDE_OF_SCREEN</b> means that gaze tracking has succeeded and the gaze point is inside the device screen.
/// - <b>OUTSIDE_OF_SCREEN</b> means that gaze tracking has succeeded and the gaze point is outside the device screen.
/// - <b>UNKNOWN</b> means that gaze tracking is failed.
public enum ScreenState
{
    INSIDE_OF_SCREEN = 0,
    OUTSIDE_OF_SCREEN = 1,
    UNKNOWN = 2
}
