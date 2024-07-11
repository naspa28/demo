using UnityEngine;
class GazeCallback_Proxy : AndroidJavaProxy
{

    public GazeCallback_Proxy() : base("camp.visual.gazetracker.callback.GazeCallback")
    {

    }

    void onGaze(AndroidJavaObject _gazeInfo)
    {
        long timestamp = _gazeInfo.Get<long>("timestamp");
        float x = _gazeInfo.Get<float>("x");
        float y = _gazeInfo.Get<float>("y");
        float fixationX = _gazeInfo.Get<float>("fixationX");
        float fixationY = _gazeInfo.Get<float>("fixationX");
        float leftOpenness = _gazeInfo.Get<float>("leftOpenness");
        float rightOpenness = _gazeInfo.Get<float>("rightOpenness");
        AndroidJavaObject trackingStateObj = _gazeInfo.Get<AndroidJavaObject>("trackingState");
        TrackingState trackingState = (TrackingState)trackingStateObj.Call<int>("ordinal");
        trackingStateObj.Dispose();

        AndroidJavaObject eyeMovementStateObj = _gazeInfo.Get<AndroidJavaObject>("eyeMovementState");
        EyeMovementState eyeMovementState = (EyeMovementState)eyeMovementStateObj.Call<int>("ordinal");
        eyeMovementStateObj.Dispose();

        AndroidJavaObject screenStateObj = _gazeInfo.Get<AndroidJavaObject>("screenState");
        ScreenState screenState = (ScreenState)screenStateObj.Call<int>("ordinal");
        screenStateObj.Dispose();

        GazeInfo gazeInfo = new GazeInfo(timestamp,
                                         x,
                                         y,
                                         fixationX,
                                         fixationY,
                                         leftOpenness,
                                         rightOpenness,
                                         trackingState,
                                         eyeMovementState,
                                         screenState);

        AndroidBridgeManager.SharedInstance().sendGazeInfo(gazeInfo);
        _gazeInfo.Dispose();
    }

}