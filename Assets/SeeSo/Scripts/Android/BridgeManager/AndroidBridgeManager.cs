using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System;

public class AndroidBridgeManager : SeeSoBridgeManager
{

    private AndroidBridgeManager()
    {

    }

    private static AndroidBridgeManager manager;

    //delegate
    private InitializationDelegate.onInitialized onInitialized;

    //proxy
    private InitailizationCallback_Proxy initializationCallback_Proxy;
    private StatusCallback_Proxy statusCallback_Proxy;
    private GazeCallback_Proxy gazeCallback_Proxy;
    private FaceCallback_Proxy faceCallback_Proxy;
    private CalibrationCallback_Proxy calibrationCallback_Proxy;
    private UserStatusCallback_Proxy userStatusCallback_Proxy;

    //native
    private AndroidJavaObject nativeTracker;
    private AndroidJavaClass nativeGazeTrackerClass;

    private AndroidJavaObject activity;
    private AndroidJavaObject preview;
    private int VISIBLE;
    private int INVISIBLE;

    private bool isInitializing = false;
    private bool isUseAttention = false;
    private bool isUseBlink = false;
    private bool isUseDrowsiness = false;

    public new static AndroidBridgeManager SharedInstance()
    {
        if (manager == null)
        {
            manager = new AndroidBridgeManager();
            manager.InitProxy();
            if (manager.nativeGazeTrackerClass == null)
            {
                manager.nativeGazeTrackerClass = new AndroidJavaClass("camp.visual.gazetracker.GazeTracker");
            }

        }
        return manager;
    }

    private void InitActivity()
    {
        if (activity == null)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
         
            AndroidJavaClass viewClass = new AndroidJavaClass("android.view.View");
            VISIBLE = viewClass.GetStatic<int>("VISIBLE");
            INVISIBLE = viewClass.GetStatic<int>("INVISIBLE");
            Debug.Log("check visible " + VISIBLE + ", invisible " + INVISIBLE);


            unityPlayer.Dispose();
            viewClass.Dispose();
        }
        else
        {
            Debug.Log("already getted activity");
        }
    }

    private void InitPreview(AndroidJavaObject context)
    {
        if (preview == null)
        {
            preview = new AndroidJavaObject("android.view.TextureView", context);
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject layoutParams = new AndroidJavaObject("android.widget.FrameLayout$LayoutParams", 1, 1);
                activity.Call("addContentView", preview, layoutParams);
            }));
        }
        else
        {
            Debug.Log("already added textureView");
        }
    }

    public new static void InitGazeTracker(string license, InitializationDelegate.onInitialized initialized)
    {
        SharedInstance().onInitialized = initialized;
        SharedInstance().ResetInstanceVariables();
        if (SharedInstance().nativeGazeTrackerClass == null)
        {
            SharedInstance().nativeGazeTrackerClass = new AndroidJavaClass("camp.visual.gazetracker.GazeTracker");
        }
        AndroidJavaClass Build = new AndroidJavaClass("android.os.Build");
        
        if (SharedInstance().nativeGazeTrackerClass != null  && Build != null)
        {
            SharedInstance().InitActivity();
            AndroidJavaObject context = SharedInstance().activity.Call<AndroidJavaObject>("getApplicationContext");
            SharedInstance().InitPreview(context);

            SharedInstance().nativeGazeTrackerClass.CallStatic("initGazeTracker", context, license, SharedInstance().initializationCallback_Proxy);
            SharedInstance().isInitializing = true;
        }

        Build.Dispose();
    }

    public new static void InitGazeTracker(string license, InitializationDelegate.onInitialized initialized, UserStatusOption option)
    {
        SharedInstance().onInitialized = initialized;
        SharedInstance().ResetInstanceVariables();
        if (SharedInstance().nativeGazeTrackerClass == null)
        {
            SharedInstance().nativeGazeTrackerClass = new AndroidJavaClass("camp.visual.gazetracker.GazeTracker");
        }
        AndroidJavaClass Build = new AndroidJavaClass("android.os.Build");
        if (SharedInstance().nativeGazeTrackerClass != null && Build != null)
        {
            SharedInstance().InitActivity();
            AndroidJavaObject context = SharedInstance().activity.Call<AndroidJavaObject>("getApplicationContext");
            SharedInstance().InitPreview(context);
            AndroidJavaObject userStatusOption = new AndroidJavaObject("camp.visual.gazetracker.constant.UserStatusOption");
            if (option.isUseAttention())
            {
               SharedInstance().isUseAttention = true;
               userStatusOption.Call("useAttention");
            }
            if (option.isUseBlink())
            {
                SharedInstance().isUseBlink = true;
                userStatusOption.Call("useBlink");
            }
            if (option.isUseDrowsiness())
            {
                SharedInstance().isUseDrowsiness = true;
                userStatusOption.Call("useDrowsiness");
            }          
            SharedInstance().nativeGazeTrackerClass.CallStatic("initGazeTracker", context, license, SharedInstance().initializationCallback_Proxy, userStatusOption);
            SharedInstance().isInitializing = true;

            userStatusOption.Dispose();
        }

        Build.Dispose();
    }

    public new static void DeinitGazeTracker()
    {
        if (SharedInstance().nativeTracker != null)
        {
            SharedInstance().isInitializing = false;
            SharedInstance().isUseAttention = false;
            SharedInstance().isUseBlink = false;
            SharedInstance().isUseDrowsiness = false;
            SharedInstance().nativeGazeTrackerClass.CallStatic("deinitGazeTracker", SharedInstance().nativeTracker);
            SharedInstance().activity.Dispose();
            SharedInstance().activity = null;
            SharedInstance().nativeTracker.Dispose();
            SharedInstance().preview.Dispose();
            SharedInstance().preview = null;
            SharedInstance().nativeTracker = null;
        }
        SharedInstance().nativeGazeTrackerClass.Dispose();
        SharedInstance().nativeGazeTrackerClass = null;
        manager = null;
    }


    public override bool IsDeviceFound()
    {
        if(nativeTracker != null)
        {
            return nativeTracker.Call<bool>("isDeviceFound");
        }
        return false;
    }

    public override void AddCameraPosition(CameraPosition cameraPosition)
    {
        if(nativeTracker != null)
        {
            AndroidJavaObject camPosition = new AndroidJavaObject("camp.visual.gazetracker.device.CameraPosition", cameraPosition.modelName, cameraPosition.screenOriginX, cameraPosition.screenOriginY, cameraPosition.cameraOnLongerAxis);
            nativeTracker.Call("addCameraPosition", camPosition);
            camPosition.Dispose();
        }
    }

    public override CameraPosition GetCameraPosition()
    {
        if(nativeTracker != null)
        {
            AndroidJavaObject currentPosition = nativeTracker.Call<AndroidJavaObject>("getCameraPosition");
            string modelName = currentPosition.Get<string>("modelName");
            float screenOriginX = currentPosition.Get<float>("screenOriginX");
            float screenOriginY = currentPosition.Get<float>("screenOriginY");
            bool cameraOnLongerAxis = currentPosition.Get<bool>("cameraOnLongerAxis");
            currentPosition.Dispose();
            CameraPosition cameraPosition = new CameraPosition(modelName, screenOriginX, screenOriginY, cameraOnLongerAxis);
            return cameraPosition;

        }
        return null;
    }

    public override CameraPosition[] GetCameraPositionList()
    {
        if (nativeTracker != null)
        {
            
            AndroidJavaObject positions = nativeTracker.Call<AndroidJavaObject>("getCameraPositionList");
            int size = positions.Call<int>("size");
            List<CameraPosition> list = new List<CameraPosition>();
            for (int i =0; i< size; i++)
            {
                AndroidJavaObject currentPosition = positions.Call<AndroidJavaObject>("get", i);
                string modelName = currentPosition.Get<string>("modelName");
                float screenOriginX = currentPosition.Get<float>("screenOriginX");
                float screenOriginY = currentPosition.Get<float>("screenOriginY");
                bool cameraOnLongerAxis = currentPosition.Get<bool>("cameraOnLongerAxis");
                CameraPosition cameraPosition = new CameraPosition(modelName, screenOriginX, screenOriginY, cameraOnLongerAxis);
                list.Add(cameraPosition);
            }
            positions.Dispose();
            return list.ToArray();
        }
        return null;
    }

    public override void SelectCameraPosition(int idx)
    {
        if (nativeTracker != null)
        {
            nativeTracker.Call("selectCameraPosition", idx);
        }
    }


    public override void SetAttentionRegion(float left, float top, float right, float bottom)
    {
        if (nativeTracker != null)
        {
            nativeTracker.Call("setAttentionRegion", left, top, right, bottom);
        }
    }


    public override float[] GetAttentionRegion()
    {
        if (nativeTracker != null)
        {
            return nativeTracker.Call<float[]>("getAttentionRegion");
        }
        return null;
    }

    public override void RemoveAttentionRegion()
    {
        if(nativeTracker != null)
        {
            nativeTracker.Call("removeAttentionRegion");
        }
    }

    public override void StartTracking()
    {
        if (nativeTracker != null)
        {
            Debug.Log("startTracking called");
            nativeTracker.Call("startTracking");
        }
        else
        {
            Debug.Log("startTracking call failed");
        }
    }

    public override void StopTracking()
    {
        if (nativeTracker != null)
        {
            nativeTracker.Call("stopTracking");
        }
    }

    public override void SetAttentionInterval(int interval)
    {
        if (nativeTracker != null)
        {
            nativeTracker.Call("setAttentionInterval", interval);
        }
    }

    public override float GetAttentionScore()
    {
        if (nativeTracker != null)
        {
            return nativeTracker.Call<float>("getAttentionScore");
        }
        return 0.0f;
    }

    public override bool IsTracking()
    {
        if (nativeTracker != null)
        {
            return nativeTracker.Call<bool>("isTracking");

        }
        return false;
    }

    public override bool SetTrackingFPS(int fps)
    {
        if (nativeTracker != null)
        {
            return nativeTracker.Call<bool>("setTrackingFPS", fps);
        }
        return false;
    }

    public void StatusStarted()
    {
        onStarted?.Invoke();
    }

    public void StatusStopped(StatusErrorType error)
    {
        onStopped?.Invoke(error);
    }

    public void CalibrationNextPoint(float x, float y)
    {
        onNextPoint?.Invoke(x, y);
    }

    public void CalibrationProgress(float progress)
    {
        onProgress?.Invoke(progress);
    }

    public void CalibrationFinished(double[] calibrationData)
    {
        onFinished?.Invoke(calibrationData);
    }

    public void Attention(long timestampBegin, long timestamEnd, float score)
    {
        if (SharedInstance().isUseAttention)
        {
            onAttention?.Invoke(timestampBegin, timestamEnd, score);
        }        
    }

    public void Blink(long timestamp, bool isBlinkLeft, bool isBlinkRight, bool isBlink, float leftOpenness, float rightOpenness)
    {
        if (SharedInstance().isUseBlink)
        {
            onBlink?.Invoke(timestamp, isBlinkLeft, isBlinkRight, isBlink, leftOpenness, rightOpenness);
        }        
    }

    public void Drowsiness(long timestamp, bool isDrowsiness, double intensity)
    {
        if (SharedInstance().isUseDrowsiness)
        {
            onDrowsiness?.Invoke(timestamp, isDrowsiness, intensity);
        }        
    }

    public void sendGazeInfo(GazeInfo gazeInfo)
    {
        onGaze?.Invoke(gazeInfo);
    }

    public void sendFaceInfo(FaceInfo faceInfo)
    {
        onFace?.Invoke(faceInfo);
    }

    public override bool StartCalibration(CalibrationModeType mode, AccuracyCriteria criteria, float left, float top, float right, float bottom)
    {
        if (nativeTracker != null)
        {
            AndroidJavaClass enumMode = new AndroidJavaClass("camp.visual.gazetracker.constant.CalibrationModeType");
            AndroidJavaClass enumCriteria = new AndroidJavaClass("camp.visual.gazetracker.constant.AccuracyCriteria");
            AndroidJavaObject _mode = enumMode.GetStatic<AndroidJavaObject>(mode.ToString());
            AndroidJavaObject _criteria = enumCriteria.GetStatic<AndroidJavaObject>(criteria.ToString());
            bool result =  nativeTracker.Call<bool>("startCalibration", _mode, _criteria, left, top, right, bottom);
            enumMode.Dispose();
            enumCriteria.Dispose();
            _mode.Dispose();
            _criteria.Dispose();
            return result;
        }
        return false;
    }

    public override bool StartCalibration(CalibrationModeType mode, AccuracyCriteria criteria)
    {
        if (nativeTracker != null)
        {
            AndroidJavaClass enumMode = new AndroidJavaClass("camp.visual.gazetracker.constant.CalibrationModeType");
            AndroidJavaClass enumCriteria = new AndroidJavaClass("camp.visual.gazetracker.constant.AccuracyCriteria");
            AndroidJavaObject _mode = enumMode.GetStatic<AndroidJavaObject>(mode.ToString());
            AndroidJavaObject _criteria = enumCriteria.GetStatic<AndroidJavaObject>(criteria.ToString());
            bool result = nativeTracker.Call<bool>("startCalibration", _mode, _criteria);
            enumMode.Dispose();
            enumCriteria.Dispose();
            _mode.Dispose();
            _criteria.Dispose();
            return result;
        }
        return false;
    }

    public override bool StartCalibration(float left, float top, float right, float bottom)
    {
        if (nativeTracker != null)
        {
            return nativeTracker.Call<bool>("startCalibration", left, top, right, bottom);
        }
        return false;
    }

    public override bool StartCalibration()
    {
        if (nativeTracker != null)
        {
            return nativeTracker.Call<bool>("startCalibration");
        }
        return false;
    }

    public override void StopCalibration()
    {
        if (nativeTracker != null)
        {
            nativeTracker.Call("stopCalibration");
        }
    }

    public override bool StartCollectSamples()
    {
        if (nativeTracker != null)
        {
            return nativeTracker.Call<bool>("startCollectSamples");
        }
        return false;
    }

    public override bool SetCalibrationData(double[] calibrationData)
    {
        if (nativeTracker != null)
        {
            return nativeTracker.Call<bool>("setCalibrationData", calibrationData);
        }
        return false;
    }

    public override void SetCameraPreview(float left, float top, float right, float bottom)
    {
        if (preview != null && nativeTracker != null)
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                int width = (int)(right - left);
                int height = (int)(bottom - top);
                AndroidJavaObject layoutParams = new AndroidJavaObject("android.widget.FrameLayout$LayoutParams", width, height);
                layoutParams.Set<int>("leftMargin", (int)left);
                layoutParams.Set<int>("topMargin", (int)top);
                preview.Call("setLayoutParams", layoutParams);
                preview.Call("setVisibility", VISIBLE);
                preview.Call<bool>("post", new AndroidJavaRunnable(() => {
                    nativeTracker.Call<bool>("setCameraPreview", preview);
                }));

                layoutParams.Dispose();
            }));
        }
    }

    public override void SetCameraPreviewAlpha(float alpha)
    {
        if(preview != null && nativeTracker != null)
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                preview.Call("setAlpha", alpha);
            }));
        }
    }

    public override void RemoveCameraPreview()
    {
        if (preview != null)
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                preview.Call("setVisibility", INVISIBLE);
            }));
        }
        if (nativeTracker != null)
        {
            nativeTracker.Call("removeCameraPreview");
        }
    }

    public new static string GetVersionName()
    {
        if (SharedInstance().nativeGazeTrackerClass != null)
        {
            return SharedInstance().nativeGazeTrackerClass.CallStatic<string>("getVersionName");
        }
        return "";
    }

    public void ResetInstanceVariables()
    {
        AndroidBridgeManager manager = AndroidBridgeManager.SharedInstance();
        if (manager != null)
        {
            manager.onStarted = null;
            manager.onStopped = null;
            manager.onGaze = null;
            manager.onFace = null;
            manager.onNextPoint = null;
            manager.onProgress = null;
            manager.onFinished = null;
        }
    }

    public void ConnectNativeCallbacks(AndroidJavaObject nativeTracker, InitializationErrorType error)
    {
        if (nativeTracker != null)
        {
            this.nativeTracker = nativeTracker;
            this.nativeTracker.Call("setStatusCallback", statusCallback_Proxy);
            this.nativeTracker.Call("setGazeCallback", gazeCallback_Proxy);
            this.nativeTracker.Call("setCalibrationCallback", calibrationCallback_Proxy);
            this.nativeTracker.Call("setUserStatusCallback", userStatusCallback_Proxy);
            this.nativeTracker.Call("setFaceCallback", faceCallback_Proxy);
        }
        else
        {
            this.nativeTracker = null;
        }
        onInitialized(error);
        onInitialized = null;
        isInitializing = false;
    }

    private void InitProxy()
    {
        initializationCallback_Proxy = new InitailizationCallback_Proxy();
        statusCallback_Proxy = new StatusCallback_Proxy();
        gazeCallback_Proxy = new GazeCallback_Proxy();
        faceCallback_Proxy = new FaceCallback_Proxy();
        calibrationCallback_Proxy = new CalibrationCallback_Proxy();
        userStatusCallback_Proxy = new UserStatusCallback_Proxy();
    }
}