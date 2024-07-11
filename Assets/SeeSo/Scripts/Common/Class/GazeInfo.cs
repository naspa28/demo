/*! \file */ 
/// <summary>
///   A class composed of information about gaze tracking.
/// </summary>
public class GazeInfo
{
    /// <summary>
    ///   Timestamp of gaze point. (unit : ms, format: UTC)
    /// </summary>
    public long timestamp;

    /// <summary>
    /// x coordinate value of gaze point. Origin is device screen. The unit is in point(px).
    /// </summary>
    public float x;
    /// <summary>
    /// y coordinate value of gaze point. Origin is device screen. The unit is in point(px).
    /// </summary>
    public float y;

    /// <summary>
    /// x coordinate value of last fixation point. Origin is device screen. The unit is in point(px).
    /// </summary>
    public float fixationX;

    /// <summary>
    /// y coordinate value of last fixation point. Origin is device screen. The unit is in point(px).
    /// </summary>
    public float fixationY;

    /// <summary>
    /// openness degree of left eye(0.0~1.0). value will only return when userStatusOption in on
    /// </summary>
    public float leftOpenness;

    /// <summary>
    /// openness degree of right eye(0.0~1.0). value will only return when userStatusOption in on
    /// </summary>
    public float rightOpenness;

    /// <summary>
    /// check <see cref="TrackingState"> TrackingState </see>
    /// </summary>
    public TrackingState trackingState;
    /// <summary>
    /// check <see cref="EyeMovementState"> EyeMovementState </see>
    /// </summary>
    public EyeMovementState eyeMovementState;

    /// <summary>
    /// check <see cref="ScreenState"> ScreenState </see>
    /// </summary>
    public ScreenState screenState;


    public GazeInfo(long _timestamp, float _x, float _y, float _fixationX, float _fixationY, float _leftOpenness, float _rightOpenness, TrackingState _trackingState, EyeMovementState _eyeMovementState, ScreenState _screenState)
    {
        timestamp = _timestamp;
        x = _x;
        y = _y;
        fixationX = _fixationX;
        fixationY = _fixationY;
        leftOpenness = _leftOpenness;
        rightOpenness = _rightOpenness;
        trackingState = _trackingState;
        eyeMovementState = _eyeMovementState;
        screenState = _screenState;
    }
}