
public class SeeSoBridgeManager
{
    private static SeeSoBridgeManager manager;

    protected StatusDelegate.onStarted onStarted;
    protected StatusDelegate.onStopped onStopped;
    protected GazeDelegate.onGaze onGaze;
    protected FaceDelegate.onFace onFace;
    protected CalibrationDelegate.onCalibrationNextPoint onNextPoint;
    protected CalibrationDelegate.onCalibrationProgress onProgress;
    protected CalibrationDelegate.onCalibrationFinished onFinished;
    protected UserStatusDelegate.onAttention onAttention;
    protected UserStatusDelegate.onBlink onBlink;
    protected UserStatusDelegate.onDrowsiness onDrowsiness;

    public static SeeSoBridgeManager SharedInstance()
    {
        if (manager == null)
        {
            manager = new SeeSoBridgeManager();
        }
        return manager;
    }

    public static void InitGazeTracker(string license, InitializationDelegate.onInitialized callback) { }

    public static void InitGazeTracker(string license, InitializationDelegate.onInitialized callback, UserStatusOption option) { }

    public static void DeinitGazeTracker(){}


    public virtual void StartTracking() { }

    public virtual void StopTracking() { }


    public virtual void SetAttentionInterval(int interval) { }

    public virtual float GetAttentionScore() { return 0.0f; }

    public virtual void SetAttentionRegion(float left, float top, float right, float bottom) { }

    public virtual float[] GetAttentionRegion() { return null; }

    public virtual void RemoveAttentionRegion() { }

    public virtual bool IsDeviceFound() { return false; }

    public virtual void SetForcedOrientation(UIInterfaceOrientation orientation) { }

    public virtual void ResetForcedOrientation() { }

    public virtual void AddCameraPosition(CameraPosition cameraPosition) { }

    public virtual CameraPosition GetCameraPosition() { return null; }

    public virtual CameraPosition[] GetCameraPositionList() { return null; }

    public virtual void SelectCameraPosition(int idx) { }

    public virtual bool IsTracking() { return false; }

    public virtual bool SetTrackingFPS(int fps) { return false; }

    public virtual bool StartCalibration(CalibrationModeType mode, AccuracyCriteria criteria, float left, float top, float right, float bottom) { return false; }

    public virtual bool StartCalibration(CalibrationModeType mode, AccuracyCriteria criteria){ return false; }

    public virtual bool StartCalibration(float left, float top, float right, float bottom) { return false; }

    public virtual bool StartCalibration() { return false; }

    public virtual bool StartCollectSamples() { return false; }

    public virtual void StopCalibration() { }

    public virtual bool SetCalibrationData(double[] calibrationData) { return false; }

    public virtual void SetCameraPreview(float left, float top, float right, float bottom) { }

    public virtual void SetCameraPreviewAlpha(float alpha) { }

    public virtual void RemoveCameraPreview() { }

    public void SetStatusCallback(StatusDelegate.onStarted start, StatusDelegate.onStopped stop) {
        this.onStarted = start;
        this.onStopped = stop;
    }

    public void RemoveStatusCallback() {
        this.onStarted = null;
        this.onStopped = null;
    }

    public void SetGazeCallback(GazeDelegate.onGaze onGaze) {
        this.onGaze = onGaze;
    }

    public void RemoveGazeCallback() {
        this.onGaze = null;
    }

    public void SetFaceCallback(FaceDelegate.onFace onFace)
    {
        this.onFace = onFace;
    }

    public void RemoveFaceCallback()
    {
        this.onFace = null;
    }

    public void SetCalibrationCallback(CalibrationDelegate.onCalibrationNextPoint nextPoint, CalibrationDelegate.onCalibrationProgress progress, CalibrationDelegate.onCalibrationFinished finished) {
        this.onNextPoint = nextPoint;
        this.onProgress = progress;
        this.onFinished = finished;
    }

    public void RemoveCalibrationCallback() {
        this.onNextPoint = null;
        this.onProgress = null;
        this.onFinished = null;
    }

    public void SetUserStatusCallback(UserStatusDelegate.onAttention onAttention, UserStatusDelegate.onBlink onBlink, UserStatusDelegate.onDrowsiness onDrowsiness)
    {
        this.onAttention = onAttention;
        this.onBlink = onBlink;
        this.onDrowsiness = onDrowsiness;
    }

    public void RemoveUserStatusCallback()
    {
        this.onAttention = null;
        this.onBlink = null;
        this.onDrowsiness = null;
    }



    public static string GetVersionName() { return ""; }
}
