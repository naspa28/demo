/*! \file */ 
/// <summary>
/// The enum that contains error types of StatusDelegate
/// </summary>
/// <b>Note</b>
/// - <b>ERROR_NONE</b> means that <i>GazeTracker.stopTracking()</i> call succeed without error.
/// - <b>ERROR_CAMERA_START</b> means that occurs when <i>GazeTracker.startTracking()</i> is called but front camera of device is not available.
/// - <b>ERROR_CAMERA_INTERRUPT</b> means that occurs when camera is unavailable.
public enum StatusErrorType
{
    ERROR_NONE = 0,
    ERROR_CAMERA_START = 1,
    ERROR_CAMERA_INTERRUPT = 2
}
