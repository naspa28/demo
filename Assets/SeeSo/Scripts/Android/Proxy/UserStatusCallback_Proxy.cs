using UnityEngine;
class UserStatusCallback_Proxy : AndroidJavaProxy
{

    public UserStatusCallback_Proxy() : base("camp.visual.gazetracker.callback.UserStatusCallback")
    {

    }

    void onAttention(long timestampBegin, long timestamEnd, float score)
    {
        AndroidBridgeManager.SharedInstance().Attention(timestampBegin, timestamEnd, score);
    }

    void onBlink(long timestamp, bool isBlinkLeft, bool isBlinkRight, bool isBlink, float leftOpenness, float rightOpenness)
    {
        AndroidBridgeManager.SharedInstance().Blink(timestamp, isBlinkLeft, isBlinkRight, isBlink, leftOpenness, rightOpenness);
    }

    void onDrowsiness(long timestamp, bool isDrowsiness, float drowsiness)
    {
        AndroidBridgeManager.SharedInstance().Drowsiness(timestamp, isDrowsiness, drowsiness);
    }
}
