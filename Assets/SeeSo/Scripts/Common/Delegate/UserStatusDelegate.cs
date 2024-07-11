/*! \file */ 
/// <summary>
/// Methods for checking user's state with custom user options
/// </summary>
/// <b>Note</b>
/// 1. Attention: How much the user attention is focused on the screen content for interval time (0.0 ~ 1.0)
/// 2. Drowsiness: If the user feel drowsiness (True/False)
/// 3. Blink: If the user blink eyes (left eye, right eye, general(both eyes)
///  
public class UserStatusDelegate
{
    /// <summary>
    ///   Callback function that informs the user's level of concentration on the screen.
    /// </summary>
    /// <b>Tip</b>
    /// 1. Timestamp range of the data will be passed as timestampBegin and timestampEnd in onAttention callback.
    /// 2. The default time interval is <b>30 seconds</b>.
    /// 3. If the user attention level is <b>Low</b>, score in onAttention callback will be closed to <b>0.0</b>.
    /// 4. If the user attention level is <b>High</b>, score in onAttention callback will be closed to <b>1.0</b>.
    /// <param name="timestampBegin">Beginning Timestamp of the data</param>
    /// <param name="timestamEnd">Ending Timestamp of the data</param>
    /// <param name="score"> User Attention rate score between the timestamps</param>
    public delegate void onAttention(long timestampBegin, long timestamEnd, float score);
    /// <summary>
    ///   A callback function that informs whether the user is blinking or not.
    /// </summary>
    /// <b>Tip</b>
    ///  1. If the user blink left eye, isBlinkLeft in onBlink callback will be true.
    ///  2. If the user blink right eye, isBlinkRight in onBlink callback will be true.
    ///  3. If the user blink eyes, isBlink in onBlink callback will be true (This is a general blink condition).
    ///  4. If the user's eyes are wide, eyeOpenness in onBlink callback will be closed to 1.0 (not available yet).
    ///  5. If the user's eyes are narrow, eyeOpenness in onBlink callback will be closed to 0.0 (not available yet).
    /// <param name="timestamp">Timestamp of the data</param>
    /// <param name="isBlinkLeft">User left blink flag</param>
    /// <param name="isBlinkRight">User right blink flag</param>
    /// <param name="isBlink">User blink flag</param>
    /// <param name="leftOpenness">User left eye-openness rate.</param>
    /// <param name="rightOpenness">User right eye-openness rate.</param>
    public delegate void onBlink(long timestamp, bool isBlinkLeft, bool isBlinkRight, bool isBlink, float leftOpenness, float rightOpenness);
    /// <summary>
    ///   If the user feel Drowsiness, isDrowsiness in onDrowsiness callback will be true, Otherwise, isDrowsiness will be false.
    /// </summary>
    /// <b>Tip</b>
    /// 1. Timestamp of the data will be passed as timestamp in onDrowsiness callback.
    /// 2. If the user feel Drowsiness, isDrowsiness in onDrowsiness callback will be true, Otherwise, isDrowsiness will be false.
    /// <param name="timestamp">Timestamp of the data</param>
    /// <param name="isDrowsiness">User drowsiness flag</param>
    /// <param name="intensity">Level of drowsiness intensity (0 to 1)</param>
    public delegate void onDrowsiness(long timestamp, bool isDrowsiness, double intensity);
}