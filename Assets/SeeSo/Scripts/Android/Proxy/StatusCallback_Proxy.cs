using UnityEngine;
class StatusCallback_Proxy : AndroidJavaProxy
{

    public StatusCallback_Proxy() : base("camp.visual.gazetracker.callback.StatusCallback")
    {

    }

    void onStarted()
    {
        AndroidBridgeManager.SharedInstance().StatusStarted();
    }

    void onStopped(AndroidJavaObject error)
    {
        StatusErrorType statusErrorType = (StatusErrorType)error.Call<int>("ordinal");
        AndroidBridgeManager.SharedInstance().StatusStopped(statusErrorType);
        error.Dispose();
    }
}