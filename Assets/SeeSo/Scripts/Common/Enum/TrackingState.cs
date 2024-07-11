/*! \file */ 
/// <summary>
///   The enum that contains state types using at GazeDelegate.
/// </summary>
/// <b>Note</b>
/// - <b>SUCCESS</b> means that face alignment is in a best position (Gaze tracking success, with valid x and y).
/// - <b>LOW_CONFIDENCE</b> means that face alignment is not in the best position, should not be used for precise gaze tracking (Gaze tracking success, with less accurate x and y).
/// - <b>UNSUPPORTED</b> means that face alignment is not suitable for tracking (Gaze tracking fail, with invalid x and y).
/// - <b>FACE_MISSING</b> means that face is missing.
public enum TrackingState
{
    SUCCESS = 0,
    LOW_CONFIDENCE = 1,
    UNSUPPORTED = 2,
    FACE_MISSING = 3
}