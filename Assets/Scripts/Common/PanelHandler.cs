using UnityEngine;

public class PanelHandler : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject statisticsPanel;
    public GameObject statistics;

    public GameObject vmPanel;
    public GameObject fgPanel;
    public GameObject pcPanel;
    public GameObject spPanel;
    public GameObject srPanel;

    void Start()
    {
        // Ensure all panels are hidden at the start
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (statisticsPanel != null) statisticsPanel.SetActive(false);
        if (statistics != null) statistics.SetActive(false);
        if (vmPanel != null) vmPanel.SetActive(false);
        if (fgPanel != null) fgPanel.SetActive(false);
        if (pcPanel != null) pcPanel.SetActive(false);
        if (spPanel != null) spPanel.SetActive(false);
        if (srPanel != null) srPanel.SetActive(false);
    }

    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
        else
        {
            Debug.LogError("SettingsPanel is not assigned!");
        }
    }

    public void ToggleStatisticsPanel()
    {
        if (statisticsPanel != null)
        {
            statisticsPanel.SetActive(!statisticsPanel.activeSelf);
        }
        else
        {
            Debug.LogError("StatisticsPanel is not assigned!");
        }
    }

    public void ToggleStatistics()
    {
        if (statistics != null)
        {
            statistics.SetActive(!statistics.activeSelf);
        }
        else
        {
            Debug.LogError("Statistics is not assigned!");
        }
    }

    public void ToggleVMPanel()
    {
        if (vmPanel != null)
        {
            vmPanel.SetActive(!vmPanel.activeSelf);
        }
        else
        {
            Debug.LogError("VMPanel is not assigned!");
        }
    }

    public void ToggleFGPanel()
    {
        if (fgPanel != null)
        {
            fgPanel.SetActive(!fgPanel.activeSelf);
        }
        else
        {
            Debug.LogError("FGPanel is not assigned!");
        }
    }

    public void TogglePCPanel()
    {
        if (pcPanel != null)
        {
            pcPanel.SetActive(!pcPanel.activeSelf);
        }
        else
        {
            Debug.LogError("PCPanel is not assigned!");
        }
    }

    public void ToggleSPPanel()
    {
        if (spPanel != null)
        {
            spPanel.SetActive(!spPanel.activeSelf);
        }
        else
        {
            Debug.LogError("SPPanel is not assigned!");
        }
    }

    public void ToggleSRPanel()
    {
        if (srPanel != null)
        {
            srPanel.SetActive(!srPanel.activeSelf);
        }
        else
        {
            Debug.LogError("SRPanel is not assigned!");
        }
    }
}
