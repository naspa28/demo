using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackingManager : MonoBehaviour
{
    public static TrackingManager Instance { get; private set; }
    private string LisenseKey = "";
    public GameObject OverlayCanvas;

    // Status
    bool isInitialized;
    bool isTracking;
    bool isCalibrating;
    UserStatusOption Option;

    bool useFilteredGaze = true;
    GazeFilter gazeFilter = new GazeFilter();

    public void setUseFilteredGaze(bool b)
    {
        useFilteredGaze = b;
    }

    public GameObject GazePoint;

    bool isNewGaze;
    float gazeX;
    float gazeY;

    float systemWidth;
    float systemHeight;

    // For Orientation
    ScreenOrientation orientation;

    // For Calibration
    public GameObject CalibrationPoint;
    bool isNextStepReady;
    bool isCalibrationFinished;
    float calibrationProgress;
    float calibrationX;
    float calibrationY;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Debug.Log("SeeSo Core Version : " + GazeTracker.getVersionName());
        orientation = Screen.orientation;

        systemWidth = Mathf.Min(Display.main.systemWidth, Display.main.systemHeight);
        systemHeight = Mathf.Max(Display.main.systemWidth, Display.main.systemHeight);

        // Request Camera Permission
        if (!HasCameraPermission())
        {
            RequestCameraPermission();
        }

        initialize();

        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the new OverlayCanvas in the new scene
        OverlayCanvas = GameObject.Find("Canvas");

        if (OverlayCanvas == null)
        {
            Debug.LogWarning("Canvas not found in the new scene!");
        }
        else
        {
            // Find GazePoint and CalibrationPoint under the new OverlayCanvas
            GazePoint = OverlayCanvas.transform.Find("GazePoint")?.gameObject;
            CalibrationPoint = OverlayCanvas.transform.Find("CalibrationPoint")?.gameObject;

            if (GazePoint == null)
            {
                Debug.LogWarning("GazePoint not found under OverlayCanvas!");
            }
            if (CalibrationPoint == null)
            {
                Debug.LogWarning("CalibrationPoint not found under OverlayCanvas!");
            }
        }

        if (isTracking)
        {
            StartCoroutine(WaitForInitializationAndStartTracking());
        }
    }

    bool HasCameraPermission()
    {
        return Permission.HasUserAuthorizedPermission(Permission.Camera);
    }

    void RequestCameraPermission()
    {
        Permission.RequestUserPermission(Permission.Camera);
    }

    // Update is called once per frame
    void Update()
    {
        // Orientation Check
        ScreenOrientation curOrientation = Screen.orientation;
        orientation = curOrientation;

        if (isTracking)
        {
            if (isNewGaze)
            {
                Vector2 overlayCanvasSizeDelta = OverlayCanvas.GetComponent<RectTransform>().sizeDelta;
                GazePoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(gazeX * overlayCanvasSizeDelta.x, gazeY * overlayCanvasSizeDelta.y);
                isNewGaze = false;
                /*userStatusAttention;
                userStatusBlink;
                userStatusDrowsiness;*/
            }

            if (isCalibrating)
            {
                GazePoint.SetActive(false);
            }
            else
            {
                GazePoint.SetActive(true);
            }
        }
        else
        {
            GazePoint.SetActive(false);
        }


        // Calibrate Progress
        if (isCalibrating)
        {
            CalibrationPoint.SetActive(true);

            if (isNextStepReady)
            {
                Vector2 overlayCanvasSizeDelta = OverlayCanvas.GetComponent<RectTransform>().sizeDelta;
                CalibrationPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(calibrationX * overlayCanvasSizeDelta.x, calibrationY * overlayCanvasSizeDelta.y);
                isNextStepReady = false;
                GazeTracker.startCollectSamples();
            }

            if (isCalibrationFinished)
            {
                isCalibrationFinished = false;
                isNextStepReady = false;
                isCalibrating = false;
            }
        }
        else
        {
            CalibrationPoint.SetActive(false);
        }

        // isInitialized
        // isTracking
        // isCalibrating
    }

    public void initialize()
    {
        if (isInitialized) return;

        Option = new UserStatusOption();

        Option.useAttention();
        Option.useBlink();
        Option.useDrowsiness();

        if (UsingUserStatus())
        {
            GazeTracker.initGazeTracker(LisenseKey, onInitialized, Option);
        }
        else
        {
            GazeTracker.initGazeTracker(LisenseKey, onInitialized);
        }

        // Start the coroutine to wait for initialization and start tracking
        StartCoroutine(WaitForInitializationAndStartTracking());
    }

    private IEnumerator WaitForInitializationAndStartTracking()
    {
        // Wait until initialization is complete
        while (!isInitialized)
        {
            yield return null; // Wait for the next frame
        }

        // Start tracking
        startTracking();
    }

    public void deinitialize()
    {
        GazeTracker.deinitGazeTracker();
        isInitialized = false;
        isTracking = false;
        isCalibrating = false;
    }

    public void startTracking()
    {
        if (isTracking || !isInitialized) return;

        Debug.Log("Start Tracking...");
        GazeTracker.setStatusCallback(onStarted, onStopped);
        GazeTracker.setGazeCallback(onGaze);

        if (UsingUserStatus())
        {
            GazeTracker.setUserStatusCallback(onAttention, onBlink, onDrowsiness);
        }
        GazeTracker.startTracking();
    }

    public void stopTracking()
    {
        if (!isInitialized || !isTracking) return;
        GazeTracker.stopTracking();
        isTracking = false;
    }

    public void startCalibration()
    {
        Debug.Log("T!!!" + !isTracking);
        Debug.Log("C!!!" + isCalibrating);
        if (!isTracking) return;
        if (isCalibrating) return;

        GazeTracker.setCalibrationCallback(onCalibrationNextPoint, onCalibrationProgress, onCalibrationFinished);
        bool success = GazeTracker.startCalibration();

        if (!success)
        {
            Debug.Log("startCalibration() fail, please check camera permission OR call startTracking() First");
        }
        else
        {
            isCalibrating = true;
        }
    }

    public void stopCalibration()
    {
        if (!isInitialized || !isCalibrating) return;

        GazeTracker.stopCalibration();

        isCalibrating = false;
        isNextStepReady = false;
        isCalibrationFinished = false;
    }

    public void setCalibrationData(double[] calibrationData)
    {
        bool result = GazeTracker.setCalibrationData(calibrationData);
        Debug.Log("setCalibrationData : " + result);
    }

    void onStarted()
    {
        isTracking = true;
    }

    void onStopped(StatusErrorType error)
    {
        isTracking = false;
    }

    bool UsingUserStatus()
    {
        return Option.isUseAttention() || Option.isUseBlink() || Option.isUseDrowsiness();
    }

    void onGaze(GazeInfo gazeInfo)
    {
        Debug.Log("onGaze " + gazeInfo.timestamp + "," + gazeInfo.x + "," + gazeInfo.y + "," + gazeInfo.trackingState + "," + gazeInfo.eyeMovementState + "," + gazeInfo.screenState);
        isNewGaze = true;

        if (!useFilteredGaze)
        {
            gazeX = _convertCoordinateX(gazeInfo.x);
            gazeY = _convertCoordinateY(gazeInfo.y);
        }
        else
        {
            gazeFilter.filterValues(gazeInfo.timestamp, _convertCoordinateX(gazeInfo.x), _convertCoordinateY(gazeInfo.y));
            gazeX = gazeFilter.getFilteredX();
            gazeY = gazeFilter.getFilteredY();
        }

        gazeInfo = null;
    }

    void onCalibrationNextPoint(float x, float y)
    {
        Debug.Log("onCalibrationNextPoint" + x + "," + y);

        calibrationX = _convertCoordinateX(x);
        calibrationY = _convertCoordinateY(y);
        isNextStepReady = true;
    }

    void onCalibrationProgress(float progress)
    {
        Debug.Log("onCalibrationProgress" + progress);
        calibrationProgress = progress;
    }

    void onCalibrationFinished(double[] calibrationData)
    {
        Debug.Log("OnCalibrationFinished" + calibrationData.Length);
        isCalibrationFinished = true;
    }

    void onAttention(long timestampBegin, long timestamEnd, float score)
    {
        Debug.Log("onAttention " + score);
    }

    void onBlink(long timestamp, bool isBlinkLeft, bool isBlinkRight, bool isBlink, float leftOpenness, float rightOpenness)
    {
        Debug.Log("onBlink " + isBlinkLeft + ", " + isBlinkRight + ", " + isBlink);
    }

    void onDrowsiness(long timestamp, bool isDrowsiness, double intensity)
    {
        Debug.Log("onDrowsiness " + intensity.ToString("F1"));
    }

    public void onInitialized(InitializationErrorType error)
    {
        Debug.Log("onInitialized result : " + error);
        if (error == InitializationErrorType.ERROR_NONE)
        {
            isInitialized = true;
        }
        else
        {
            isInitialized = false;
        }
    }

    float _convertCoordinateX(float x)
    {
        float screenWidth = systemHeight;
        /*if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight)
        {
            screenWidth = systemHeight;
        }*/

        return gazeX = x / screenWidth - 0.5f;
    }

    float _convertCoordinateY(float y)
    {
        float screenHeight = systemWidth;
        /*if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight)
        {
            screenHeight = systemWidth;
        }*/

        return gazeY = 0.5f - y / screenHeight;
    }
}
