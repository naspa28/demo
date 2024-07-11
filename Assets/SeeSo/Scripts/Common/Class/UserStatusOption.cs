/*! \file */ 
/// <summary>
/// The class contains User Status options information for GazeTracker
/// </summary>
public class UserStatusOption
{
    private bool modeAttention;
    private bool modeBlink;
    private bool modeDrowsiness;

    /// <summary>
    /// Constructor.
    /// </summary>
    public UserStatusOption()
    {
        modeAttention = false;
        modeBlink = false;
        modeDrowsiness = false;
    }

    /// <summary>
    /// Sets the gaze tracker module to check user attention level
    /// </summary>
    public void useAttention()
    {
        modeAttention = true;
    }

    /// <summary>
    /// Sets the gaze tracker module to check user blinking level
    /// </summary>
    public void useBlink()
    {
        modeBlink = true;
    }

    /// <summary>
    /// Sets the gaze tracker module to check user drowsiness level
    /// </summary>
    public void useDrowsiness()
    {
        modeDrowsiness = true;
    }

/// <summary>
/// Sets the gaze tracker module to check all options. (Drowsiness, Attention, Blink)
/// </summary>
    public void useAll()
    {
        modeAttention = true;
        modeBlink = true;
        modeDrowsiness = true;
    }

/// <summary>
/// Returns a Boolean value that indicates whether the gaze tracker module uses attention checking option.
/// </summary>
/// <returns>whether the gaze tracker module uses attention checking option</returns>
    public bool isUseAttention()
    {
        return modeAttention;
    }

/// <summary>
/// Returns a bool value that indicates whether the gaze tracker module uses blinking checking option.
/// </summary>
/// <returns>whether the gaze tracker module uses blinking checking option.</returns>
    public bool isUseBlink()
    {
        return modeBlink;
    }

/// <summary>
/// Returns a bool value that indicates whether the gaze tracker module uses drowsiness checking option.
/// </summary>
/// <returns>whether the gaze tracker module uses drowsiness checking option.</returns>
    public bool isUseDrowsiness()
    {
        return modeDrowsiness;
    }
}