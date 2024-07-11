using UnityEngine;
class CalibrationCallback_Proxy : AndroidJavaProxy
{

    public CalibrationCallback_Proxy() : base("camp.visual.gazetracker.callback.CalibrationCallback")
    {

    }

    void onCalibrationNextPoint(float x, float y)
    {
        AndroidBridgeManager.SharedInstance().CalibrationNextPoint(x, y);
    }

    void onCalibrationProgress(float progress)
    {
        AndroidBridgeManager.SharedInstance().CalibrationProgress(progress);
    }

    void onCalibrationFinished(double[] calibrationData)
    {
        AndroidBridgeManager.SharedInstance().CalibrationFinished(calibrationData);
    }
}