using UnityEngine;


public class CalibrationHandler : MonoBehaviour
{
    private TrackingManager trackingManager;

    void Start()
    {
        // Find the TrackingManager in the scene
        trackingManager = FindObjectOfType<TrackingManager>();

        if (trackingManager == null)
        {
            Debug.LogError("TrackingManager not found in the scene. Make sure it's properly set up.");
        }
    }

    public void OnButtonClick()
    {
        if (trackingManager != null)
        {
            trackingManager.startCalibration();
        }
    }
}
