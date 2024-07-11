/*! \file */ 
/// <summary>
/// This class indicates the relative position from the camera to the screen origin.
/// <p><see cref="GazeTracker">GazeTracker</see> uses the information to calculate gaze coordinate.
/// <p>For more information, see <a href="https://docs.seeso.io/docs/document/gaze-coordinate-android">gaze coordinate</a>
/// </summary>
public class CameraPosition
{
    public string modelName;
    public float screenOriginX;
    public float screenOriginY;
    public bool cameraOnLongerAxis;
/// <summary>
/// 
/// </summary>
/// <param name="modelName">Device model name</param>
/// <param name="screenOriginX">X's Distance from the camera to the top left corner of the screen (mm)</param>
/// <param name="screenOriginY">Y's Distance from the camera to the top left corner of the screen (mm)</param>
/// <param name="cameraOnLongerAxis">A boolean value representing when the camera is placed on the device's long axis.</param>
    public CameraPosition(string modelName, float screenOriginX, float screenOriginY, bool cameraOnLongerAxis)
    {
        this.modelName = modelName;
        this.screenOriginX = screenOriginX;
        this.screenOriginY = screenOriginY;
        this.cameraOnLongerAxis = cameraOnLongerAxis;
    }
}
