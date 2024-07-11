using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class GraphPlotter : MonoBehaviour
{
    public RectTransform graphContainer;
    public GameObject dataPointPrefab;
    public TextMeshProUGUI graphTitle;
    public LineRenderer lineRenderer;
    public GameObject xAxisLabelPrefab;
    public GameObject graphPanel; // Reference to the graph panel
    public RectTransform legendContainer; // Reference to the legend container
    public GameObject legendItemPrefab; // Prefab for legend items


    private Dictionary<string, Color> dataColors = new Dictionary<string, Color>()
    {
        { "lvl", Color.red },
        { "star", Color.green },
        { "try_count", Color.blue },
        { "corr", Color.yellow },
        { "ans_rate", Color.magenta },
        { "time", Color.cyan },
        { "conc", Color.black }
    };

    public void ShowGraph(string game)
    {
        graphTitle.text = $"{game} Graph";
        graphPanel.SetActive(true);

        List<GameSession> gameSessions = LocalDataManager.Instance.GetGameSessions(game);
        PlotGraph(gameSessions);
        CreateLegend();
    }

    // Method to close the graph panel
    public void CloseGraph()
    {
        graphPanel.SetActive(false);
    }

    private void PlotGraph(List<GameSession> gameSessions)
    {
        foreach (Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        float graphWidth = graphContainer.GetComponent<RectTransform>().rect.width;
        float graphHeight = graphContainer.GetComponent<RectTransform>().rect.height;
        float yMax = GetMaxValue(gameSessions);  // Assuming a maximum value for the data points

        for (int i = 0; i < gameSessions.Count; i++)
        {
            float xPosition = (i / (float)(gameSessions.Count - 1)) * graphWidth - graphWidth / 2;  // Adjusting xPosition to account for pivot
            CreateDataPoint(new Vector3(xPosition, gameSessions[i].lvl / yMax * graphHeight - graphHeight / 2), "lvl", 0);
            CreateDataPoint(new Vector3(xPosition, gameSessions[i].star / yMax * graphHeight - graphHeight / 2), "star", 5);
            CreateDataPoint(new Vector3(xPosition, gameSessions[i].try_count / yMax * graphHeight - graphHeight / 2), "try_count", 10);
            CreateDataPoint(new Vector3(xPosition, gameSessions[i].corr / yMax * graphHeight - graphHeight / 2), "corr", 15);
            CreateDataPoint(new Vector3(xPosition, gameSessions[i].ans_rate / yMax * graphHeight - graphHeight / 2), "ans_rate", 20);
            CreateDataPoint(new Vector3(xPosition, gameSessions[i].time / yMax * graphHeight - graphHeight / 2), "time", 25);
            CreateDataPoint(new Vector3(xPosition, gameSessions[i].conc / yMax * graphHeight - graphHeight / 2), "conc", 30);

            // Add date label
            GameObject dateLabel = Instantiate(xAxisLabelPrefab, graphContainer);
            RectTransform labelRect = dateLabel.GetComponent<RectTransform>();
            labelRect.anchoredPosition = new Vector2(xPosition, -graphHeight / 2 - 10f); // Position below the graph
            TextMeshProUGUI text = dateLabel.GetComponent<TextMeshProUGUI>();
            text.text = gameSessions[i].date;
            text.fontSize = 12;
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;
        }

        ConnectDataPoints(gameSessions, yMax);
    }

    private float GetMaxValue(List<GameSession> gameSessions)
    {
        float maxValue = 0f;

        foreach (var session in gameSessions)
        {
            maxValue = Mathf.Max(maxValue, session.lvl, session.star, session.try_count, session.corr, session.ans_rate, session.time, session.conc);
        }

        return maxValue;
    }

    private Vector2 CreateDataPoint(Vector2 anchoredPosition, string dataType, float yOffset)
    {
        GameObject dataPoint = Instantiate(dataPointPrefab, graphContainer);
        RectTransform rectTransform = dataPoint.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + yOffset);
        RawImage rawImage = dataPoint.GetComponent<RawImage>();
        rawImage.color = dataColors[dataType];

        Debug.Log($"Created data point for {dataType} at position: {rectTransform.anchoredPosition}");

        return rectTransform.anchoredPosition;
    }

    private void ConnectDataPoints(List<GameSession> gameSessions, float yMax)
    {
        // Connecting data points for each data type
        foreach (string dataType in dataColors.Keys)
        {
            List<Vector2> points = new List<Vector2>();

            for (int i = 0; i < gameSessions.Count; i++)
            {
                float graphHeight = graphContainer.GetComponent<RectTransform>().rect.height;
                float xPosition = (i / (float)(gameSessions.Count - 1)) * graphContainer.GetComponent<RectTransform>().rect.width;

                float yPosition = 0;

                switch (dataType)
                {
                    case "lvl":
                        yPosition = gameSessions[i].lvl / yMax * graphHeight;
                        break;
                    case "star":
                        yPosition = gameSessions[i].star / yMax * graphHeight;
                        break;
                    case "try_count":
                        yPosition = gameSessions[i].try_count / yMax * graphHeight;
                        break;
                    case "corr":
                        yPosition = gameSessions[i].corr / yMax * graphHeight;
                        break;
                    case "ans_rate":
                        yPosition = gameSessions[i].ans_rate / yMax * graphHeight;
                        break;
                    case "time":
                        yPosition = gameSessions[i].time / yMax * graphHeight;
                        break;
                    case "conc":
                        yPosition = gameSessions[i].conc / yMax * graphHeight;
                        break;
                }

                points.Add(new Vector2(xPosition, yPosition));
            }

            if (points.Count > 1)
            {
                LineRenderer lr = Instantiate(lineRenderer, graphContainer);
                lr.positionCount = points.Count;
                lr.SetPositions(points.ConvertAll(p => new Vector3(p.x, p.y, 0)).ToArray());
                lr.startColor = dataColors[dataType];
                lr.endColor = dataColors[dataType];
                lr.startWidth = 2f;
                lr.endWidth = 2f;
            }
        }
    }

    private void CreateLegend()
    {
        foreach (Transform child in legendContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var data in dataColors)
        {
            GameObject legendItem = Instantiate(legendItemPrefab, legendContainer);
            legendItem.transform.Find("LegendColor").GetComponent<Image>().color = data.Value;
            legendItem.transform.Find("LegendLabel").GetComponent<TextMeshProUGUI>().text = data.Key;
        }
    }
}
