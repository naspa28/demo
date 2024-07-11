

/*! \file */
using UnityEngine;
/// <summary>
///   A class composed of information about gaze tracking.
/// </summary>
public class FaceInfo
{
    /// <summary>
    ///   Timestamp of gaze point. (unit : ms, format: UTC)
    /// </summary>
    public long timestamp;

    /// <summary>
    ///   Value of facial recognition confidence (0.0 ~ 1.0).
    /// </summary>
    public float score;

    /// <summary>
    ///   camera frame width size.
    /// </summary>
    public float frameWidth;

    /// <summary>
    ///   camera frame height size.
    /// </summary>
    public float frameHeight;

    /// <summary>
    ///   Indicates the position of the face in the camera frame.
    /// </summary>
    public Rect rect;


    /// <summary>
    /// This is the rotation of the face around the x-axis.
    /// It measures the up-and-down tilt of the face.
    /// </summary>
    public float pitch;

    /// <summary>
    /// This is the rotation of the face around the y-axis.
    /// It measures the left-and-right turn of the face.
    /// </summary>
    public float yaw;

    /// <summary>
    /// This is the rotation of the face around the z-axis.
    /// It measures the clockwise and counterclockwise tilt of the face.
    /// </summary>
    public float roll;

    /// <summary>
    /// The x,y,z distance of the center of the face from the camera. The unit is mm.
    /// </summary>
    public Vector3 centerXYZ;

    public FaceInfo(long _timestamp, float _score, float _frameWidth, float _frameHeight, Rect _rect, float _pitch, float _yaw, float _roll, Vector3 _centerXYZ)
    {
        timestamp = _timestamp;
        score = _score;
        frameWidth = _frameWidth;
        frameHeight = _frameHeight;
        rect = _rect;
        pitch = _pitch;
        yaw = _yaw;
        roll = _roll;
        centerXYZ = _centerXYZ;
    }
}