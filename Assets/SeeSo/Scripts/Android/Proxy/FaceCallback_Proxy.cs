using UnityEngine;
class FaceCallback_Proxy : AndroidJavaProxy
{

    public FaceCallback_Proxy() : base("camp.visual.gazetracker.callback.FaceCallback")
    {

    }

    void onFace(AndroidJavaObject _faceInfo)
    {
        long timestamp = _faceInfo.Get<long>("timestamp");
        float score = _faceInfo.Get<float>("score");
        AndroidJavaObject frameSize = _faceInfo.
            Get<AndroidJavaObject>("frameSize");
        float frameWidth = frameSize.Call<int>("getWidth");
        float frameHeight = frameSize.Call<int>("getHeight");
        frameSize.Dispose();
        float left = _faceInfo.Get<float>("left");
        float top = _faceInfo.Get<float>("top");
        float right = _faceInfo.Get<float>("right");
        float bottom = _faceInfo.Get<float>("bottom");

        float pitch = _faceInfo.Get<float>("pitch");
        float yaw = _faceInfo.Get<float>("yaw");
        float roll = _faceInfo.Get<float>("roll");

        float centerX = _faceInfo.Get<float>("centerX");
        float centerY = _faceInfo.Get<float>("centerY");
        float centerZ = _faceInfo.Get<float>("centerZ");


        FaceInfo faceInfo = new FaceInfo(timestamp, score,
            frameWidth, frameHeight,
            new Rect(left, top, right - left, bottom - top),
            pitch, yaw, roll, new Vector3(centerX, centerY, centerZ));

        AndroidBridgeManager.SharedInstance().sendFaceInfo(faceInfo);
        _faceInfo.Dispose();
    }


}
