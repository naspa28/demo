using UnityEngine;
/*! \file */ 
/// <summary>
///   The class that generates gaze data using the device's frontal camera.
/// </summary>
/// 
/// <remarks>
///   This class consists of static functions and is implemented as a singleton pattern inside.
/// </remarks>
///- Configures device orientation and calibration setting.
///- Manages the gazetracking lifecycle - start, pause, resume, and close.
///- Manage the gaze tracking that start, pause, resume and stop.
///
public class GazeTracker
{
  /// <summary>
  ///   Initializes GazeTracker.startTracking object
  /// </summary>
  /// <remarks>
  ///   InitializationDelegate will relay initialized object.
  /// <remarks>
  /// 
  /// <b>Important</b>
  /// - Initializing process requires <b>authentication</b>.
  /// 
  /// <param name="license">Key generated from <a href="http://manage.seeso.io">manage page</a></param>
  /// <param name="callback">Delegate represents initialization status</param>
    public static void initGazeTracker(string license, InitializationDelegate.onInitialized callback)
    {
        AndroidBridgeManager.InitGazeTracker(license, callback);
    }
/// <summary>
///   Initializes `GazeTracker.startTracking()` object
/// </summary>
/// <remarks>
///   InitializationDelegate will relay initialized object.
/// <remarks>
/// 
/// <b>Important</b>
///- Initializing process requires <b>authentication</b>.
///- This function works <b>asynchronously</b>.
/// 
/// <param name="license">Key generated from <a href="http://manage.seeso.io">manage page</a></param>
/// <param name="callback">Callback represents initialization status</param>
/// <param name="option">A class containing User status option information</param>
    public static void initGazeTracker(string license, InitializationDelegate.onInitialized callback, UserStatusOption option)
    {
        AndroidBridgeManager.InitGazeTracker(license, callback, option);
    }

/// <summary>
/// Terminates <b>GazeTracker</b> object.
/// </summary>
/// <remarks>
///   Once terminated, every API will be disabled until the initialize function is called again.
/// </remarks>
    public static void deinitGazeTracker()
    {
        AndroidBridgeManager.DeinitGazeTracker();
    }

/// <summary>
///   Is device found bool. (<b>Only Android</b>)
/// </summary>
/// <returns>the bool </returns>
    public static bool isFoundDevice()
    {
        return AndroidBridgeManager.SharedInstance().IsDeviceFound();
    }

/// <summary>
///   Add camera position. (<b>Only Android</b>)
/// </summary>
/// <param name="cameraPosition"> See <see cref="CameraPosition">CameraPosition</see> </param>
    public static void addCameraPosition(CameraPosition cameraPosition)
    {
        AndroidBridgeManager.SharedInstance().AddCameraPosition(cameraPosition);
    }

/// <summary>
/// Gets current camera position. (<b>Only Android</b>)
/// </summary>
/// <returns>See <see cref="CameraPosition">CameraPosition</see></returns>
    public static CameraPosition getCameraPosition()
    {
#if UNITY_ANDROID
        return AndroidBridgeManager.SharedInstance().GetCameraPosition();
#endif
        return null;
    }

/// <summary>
/// Gets camera position list. (<b>Only Android</b>)
/// </summary>
/// <returns><see cref="CameraPosition">CameraPosition</see> arrays. </returns>
    public static CameraPosition[] getCameraPositionList()
    {
#if UNITY_ANDROID
        return AndroidBridgeManager.SharedInstance().GetCameraPositionList();
#endif
         return null;
    }
/// <summary>
///   Select camera position.
/// </summary>
/// <param name="idx">idx The idx value to use in the camera position array currently held by the gaze tracker.</param>
    public static void selectCameraPositions(int idx)
    {
        AndroidBridgeManager.SharedInstance().SelectCameraPosition(idx);
    }
/// <summary>
///   Start gaze tracking
/// </summary>
/// <remarks>
///   This function should be called before start gaze tracking.
/// </remarks>
/// <para>
///   StatusDelegate.onStarted will be called after start succeeded.
/// </para>
    public static void startTracking()
    {
        AndroidBridgeManager.SharedInstance().StartTracking();
    }
/// <summary>
/// Stop gaze tracking.
/// </summary>
/// <para>
///   StatusDelegate.onStopped will be called after succeed to stop tracking.
/// </para>
    public static void stopTracking()
    {
        AndroidBridgeManager.SharedInstance().StopTracking();
    }

/// <summary>
///   Set time interval for <see cref="UserStatusOption">UserStatusOption</see> <i>Attention</i> callback.
/// </summary>
/// <remarks>
///   The UserStatus <i>Attention</i> score will be calculated with given time interval.
/// </remarks>
/// <para>
///   Start and end timestamps are passed to timestampBegin and timestampEnd. via the UserStatusDelegate.onAttention callback.
/// </para>
/// <param name="interval">The value can be set in units of 10 to 60 seconds. (The default is 30 seconds.)</param>
    public static void setAttentionInterval(int interval)
    {
        AndroidBridgeManager.SharedInstance().SetAttentionInterval(interval);
    }
/// <summary>
/// Get current <b>Attention<b> score from the GazeTracker.
/// </summary>
/// <returns>the attention score (0.0 ~ 1.0)</returns>
    public static float getAttentionScore()
    {
        return AndroidBridgeManager.SharedInstance().GetAttentionScore();
    }
/// <summary>
/// Set the user's attention roi (Check this: <a href="https://en.wikipedia.org/wiki/Region_of_interest">Region of Interest</a>).
/// </summary>
/// <param name="left">left Attention region area (default is Device Layout Size).</param>
/// <param name="top">top Attention region area (default is Device Layout Size).</param>
/// <param name="right">right Attention region area (default is Device Layout Size).</param>
/// <param name="bottom">bottom Attention region area (default is Device Layout Size).</param>
    public static void setAttentionRegion(float left, float top, float right, float bottom)
    {
        AndroidBridgeManager.SharedInstance().SetAttentionRegion(left, top, right, bottom);
    }

/**
   * Get current attention region.(left top) is 0,0.
   *
   * @return tleft,top,right,bottom region array.
   */
    public static float[] getAttentionRegion()
    {
        return AndroidBridgeManager.SharedInstance().GetAttentionRegion();
    }

/// <summary>
///   Remove current set attention region.
/// </summary>
    public static void removeAttentionRegion()
    {
        AndroidBridgeManager.SharedInstance().RemoveAttentionRegion();
    }

    /// <summary>
    /// Set the forced device orientation for the GazeTracker.
    /// </summary>
    /// <param name="orientation"> The forced device orientation. </param>
    public static void setForcedOrientation(UIInterfaceOrientation orientation)
    {
        AndroidBridgeManager.SharedInstance().SetForcedOrientation(orientation);
    }

    /// <summary>
    /// Reset the forced device orientation for the GazeTracker.
    /// </summary>
    public static void resetForcedOrientation()
    {
        AndroidBridgeManager.SharedInstance().ResetForcedOrientation();
    }

    /// <summary>
    /// A Bool value indicating whether the gaze tracking is in progress
    /// </summary>
    /// <returns>bool</returns>
    public static bool isTracking()
    {
        return AndroidBridgeManager.SharedInstance().IsTracking();
    }

/// <summary>
///   The parameter that sets the FPS of the gaze tracking source.
/// </summary>
/// <remarks>
///   FPS can be dropped due to device spec. The default value is 30.
/// </remarks>
/// <para>
///   FPS parameter should be 0...30.
/// </para>
/// <param name="fps">Custom FPS(Frame Per Second) for gaze tracking</param>
/// <returns>Whether fps setting is normally performed</returns>
    public static bool setTrackingFPS(int fps)
    {
        return AndroidBridgeManager.SharedInstance().SetTrackingFPS(fps);
    }

/// <summary>
/// Start the calibration process.
///  The <b>false</b> return means calibration process will be stopped.
/// </summary>
/// <param name="mode"> Can select calibration option. Check <see cref="CalibrationModeType">CalibrationModeType</see></param>
/// <param name="criteria">Option that manage calibration process and accuracy. Check <see cref="AccuracyCriteria">AccracyCriteria</see></param>
/// <param name="left">Region that needs to be calibrated. The unit is px. </param>
/// <param name="top">Region that needs to be calibrated. The unit is px.</param>
/// <param name="right">Region that needs to be calibrated. The unit is px.</param>
/// <param name="bottom">Region that needs to be calibrated. The unit is px.</param>
/// <returns>If it was false when the tracker is <b>not</b> tracking or or value of <b>mode</b> is not defined. the calibration region has set outside the device screen. </returns>
    public static bool startCalibration(CalibrationModeType mode, AccuracyCriteria criteria, float left, float top, float right, float bottom)
    {
        return AndroidBridgeManager.SharedInstance().StartCalibration(mode, criteria, left, top, right, bottom);
    }
/// <summary>
/// Start the calibration process.
/// <p>The <b>false</b> return means calibration process will be stopped.
/// </summary>
/// <param name="mode">Can select calibration option. Check <see cref="CalibrationModeType">CalibrationModeType</see></param>
/// <param name="criteria">Option that manage calibration process and accuracy. Check <see cref="AccuracyCriteria">AccracyCriteria</see></param>
/// <returns>If it was false when the tracker is <b>not</b> tracking or value of <b>mode</b> is not defined.</returns>
    public static bool startCalibration(CalibrationModeType mode, AccuracyCriteria criteria)
    {
        return AndroidBridgeManager.SharedInstance().StartCalibration(mode, criteria);
    }

/// <summary>
/// Start the calibration process.
/// <p>The <b>false</b> return means calibration process will be stopped.
/// </summary>
/// <param name="left">Region that needs to be calibrated. The unit is px. </param>
/// <param name="top">Region that needs to be calibrated. The unit is px.</param>
/// <param name="right">Region that needs to be calibrated. The unit is px.</param>
/// <param name="bottom">Region that needs to be calibrated. The unit is px.</param>
/// <returns>If it was false when the tracker is <b>not</b> tracking or the calibration region has set outside the device screen.</returns>
    public static bool startCalibration(float left, float top, float right, float bottom)
    {
        return AndroidBridgeManager.SharedInstance().StartCalibration(left, top, right, bottom);
    }

/// <summary>
/// Start the calibration process.
/// <p>The <b>false</b> return means calibration process will be stopped.
/// </summary>
/// <returns>If it was false when the tracker is <b>not</b> tracking.</returns>
    public static bool startCalibration()
    {
        return AndroidBridgeManager.SharedInstance().StartCalibration();
    }

/// <summary>
///   Stop the calibration process.
/// </summary>
    public static void stopCalibration()
    {
        AndroidBridgeManager.SharedInstance().StopCalibration();
    }

/// <summary>
/// Continues calibration after showing the next calibration point to the user.
/// </summary>
/// <b>precondition</b>
/// 1. (optional) Previous calibration is completed to the coordinates obtained through <see cref="CalibrationCallback">CalibrationCallback.onCalibrationNextPoint(x:y:)</see> has finished.
/// 2. (optional) Checked CalibrationCallback.onCalibrationProgress returned 1.0.
/// 3. Obtained the next calibration coordinates from CalibrationCallback.onCalibrationNextPoint.
/// 4. Finished place specific View (ex. Custom View) on coordinates from step 3.
/// <returns>Whether calibration of this step could start successfully.</returns>
    public static bool startCollectSamples()
    {
        return AndroidBridgeManager.SharedInstance().StartCollectSamples();
    }

/// <summary>
///   Set existing calibration data to <b>GazeTracker</b>.
/// </summary>
/// <param name="calibrationData">Calibration Data</param>
/// <returns>Whether Gaze Tracking is in progress and also data set successfully</returns>
    public static bool setCalibrationData(double[] calibrationData)
    {
        return AndroidBridgeManager.SharedInstance().SetCalibrationData(calibrationData);
    }

/// <summary>
/// Set view to present camera preview that <b>GazeTracker</b> is using
/// </summary>
/// <param name="left">Region that needs to be shown. The unit is px.</param>
/// <param name="top">Region that needs to be shown. The unit is px.</param>
/// <param name="right">Region that needs to be shown. The unit is px.</param>
/// <param name="bottom">Region that needs to be shown. The unit is px.</param>
    public static void setCameraPreview(float left, float top, float right, float bottom)
    {
        AndroidBridgeManager.SharedInstance().SetCameraPreview(left, top, right, bottom);
    }
/// <summary>
///   Removes the camera preview.
/// </summary>
    public static void removeCameraPreview()
    {
        AndroidBridgeManager.SharedInstance().RemoveCameraPreview();
    }

    /// <summary>
    /// Set view to present camera preview alpha
    /// </summary>
    /// <param name="alpha">0.0f ~ 1.0f</param>
    public static void setCameraPreviewAlpha(float alpha)
    {
        AndroidBridgeManager.SharedInstance().SetCameraPreviewAlpha(alpha);
    }

    /// <summary>
    ///   Sets StatusDelegate.
    /// </summary>
    /// <param name="onStarted">see StatusDelegate.onStarted</param>
    /// <param name="onStopped">see StatusDelegate.onStopped</param>
    public static void setStatusCallback(StatusDelegate.onStarted onStarted, StatusDelegate.onStopped onStopped)
    {
        AndroidBridgeManager.SharedInstance().SetStatusCallback(onStarted, onStopped);
    }

/// <summary>
///   Removes StatusDelegate.
/// </summary>
    public static void removeStatusCallback()
    {
        AndroidBridgeManager.SharedInstance().RemoveStatusCallback();
    }

/// <summary>
///   Sets GazeDelegate.
/// </summary>
/// <param name="onGaze">see GazeDelegate.OnGaze</param>
    public static void setGazeCallback(GazeDelegate.onGaze onGaze)
    {
        AndroidBridgeManager.SharedInstance().SetGazeCallback(onGaze);
    }


/// <summary>
///   Sets FaceDelegate.
/// </summary>
/// <param name="onFace">see FaceDelegate.OnFace</param>
    public static void setFaceCallback(FaceDelegate.onFace onFace)
    {
        AndroidBridgeManager.SharedInstance().SetFaceCallback(onFace);
    }

    /// <summary>
    ///   Removes GazeDelegate.
    /// </summary>
    public static void removeGazeCallback()
    {
        AndroidBridgeManager.SharedInstance().RemoveGazeCallback();
    }

    /// <summary>
    ///   Removes FaceDelegate.
    /// </summary>
    public static void removeFaceCallback()
    {
        AndroidBridgeManager.SharedInstance().RemoveFaceCallback();
    }

    /// <summary>
    ///   Sets CalibrationDelegate.
    /// </summary>
    /// <param name="onCalibrationNext">see CalibrationDelegate.onCalibrationNextPoint</param>
    /// <param name="onCalibrationProgress">see CalibrationDelegate.onCalibrationProgress</param>
    /// <param name="onCalibrationFinished">see CalibrationDelegate.onCalibrationFinished</param>
    public static void setCalibrationCallback(CalibrationDelegate.onCalibrationNextPoint onCalibrationNext, CalibrationDelegate.onCalibrationProgress onCalibrationProgress, CalibrationDelegate.onCalibrationFinished onCalibrationFinished)
    {
        AndroidBridgeManager.SharedInstance().SetCalibrationCallback(onCalibrationNext, onCalibrationProgress, onCalibrationFinished);
    }

/// <summary>
///   Removes CalibartionDelegate.
/// </summary>
    public static void removeCalibrationCallback()
    {
        AndroidBridgeManager.SharedInstance().RemoveCalibrationCallback();
    }

/// <summary>
///   Sets UserStatusDelegate.
/// </summary>
/// <param name="onAttention">see UserStatusDelegate.onAttention</param>
/// <param name="onBlink">see UserStatusDelegate.onBlink</param>
/// <param name="onDrowsiness">see UserStatusDelegate.onDrowsiness</param>
    public static void setUserStatusCallback(UserStatusDelegate.onAttention onAttention, UserStatusDelegate.onBlink onBlink, UserStatusDelegate.onDrowsiness onDrowsiness)
    {
        AndroidBridgeManager.SharedInstance().SetUserStatusCallback(onAttention, onBlink, onDrowsiness);
    }

/// <summary>
///   removes UserStatusDelegate.
/// </summary>
    public static void removeUserStatusCallback()
    {
        AndroidBridgeManager.SharedInstance().RemoveUserStatusCallback();
    }

/// <summary>
/// Returns current SDK version.
/// </summary>
/// <returns>Current SDK version.</returns>
    public static string getVersionName()
    {
        return AndroidBridgeManager.GetVersionName();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
