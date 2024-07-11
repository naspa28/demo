using UnityEngine;
class InitailizationCallback_Proxy : AndroidJavaProxy
{

    public InitailizationCallback_Proxy() : base("camp.visual.gazetracker.callback.InitializationCallback")
    {

    }

    void onInitialized(AndroidJavaObject tracker, AndroidJavaObject error)
    {

        InitializationErrorType initializationErrorType = (InitializationErrorType)error.Call<int>("ordinal");
        AndroidBridgeManager manager = AndroidBridgeManager.SharedInstance();
        if (initializationErrorType == InitializationErrorType.ERROR_NONE)
        {
            manager.ConnectNativeCallbacks(tracker, initializationErrorType);
        }
        else
        {
            manager.ConnectNativeCallbacks(null, initializationErrorType);
        }
        error.Dispose();
    }
}
