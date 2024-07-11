/*! \file */ 
/// <summary>
///   Methods for processing calibration and showing progress of calibration
/// </summary>
///<b>Important</b>
// - CalibrationDelegate must be called on main thread.
public class CalibrationDelegate
{
    /// <summary>
    /// Callback that returns the x, y coordinate of the next calibration point.
    /// </summary>
    /// <b>Precondition</b>
    ///  - You need to call <see cref="GazeTracker.startCollectSamples">GazeTracker.startCollectSamples()</see> to continue collecting samples for the next calibration target.
    /// 
    /// <param name="x">The x coordinate of the next calibration target. Origin is the top-left of the device screen. The unit is in pixel(px).</param>
    /// <param name="y">The y coordinate of the next calibration target. Origin is the top-left of the device screen. The unit is in pixel(px).</param>
    public delegate void onCalibrationNextPoint(float x, float y);

    /// <summary>
    ///   Callback that returns collection progress of the current calibration target. The progress will be a value in between 0.0 and 1.0.
    /// </summary>
    /// <remarks>
    ///   The next point will be guided when the value reaches 1.0.
    /// </remakrs>
    /// <param name="progress">Calibration progression for each point.</param>
    public delegate void onCalibrationProgress(float progress);
    /// <summary>
    ///   Callback that notifies when the calibration ends.
    /// </summary>
    /// <remarks>
    ///   When this function is called, the calibration UI will be removed.<p>
    ///   The calibrationData passed as a parameter has already been applied to <b>GazeTracker</b>
    /// </remarks>
    /// <b>Note</b>
    /// 1. You can save and load this calibration data directly into <b>GazeTracker</b>.
    /// 2. when restarting the app or etc, you can set data by calling <b>GazeTracker.setCalibrationData</b>.
    ///
    /// <param name="calibrationData">the calibration data</param>
    public delegate void onCalibrationFinished(double[] calibrationData);
}