using ChartAndGraph;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO
// 1. 통계 메인 화면에서 게임 기록 버튼 클릭 시 게임 기록 패널로 이동 - 완
// 2. Record 패널에 기록 된 게임 불러오기 - 완
// 3. Progress 패널에 스테이지별 진행도 표시

// start에서 데이터들 불러오고 저장 해둔다

public class StatisticsManager : MonoBehaviour
{
    // 씬의 오브젝트들을 저장할 변수
    // 각 패널들을 저장 할 변수
    public GameObject panel_statistics;
    public GameObject panel_record;
    public GameObject panel_progress;

    // RecordPanel의 오브젝트들
    public GameObject Content;
    public GameObject Content_template;


    // ProgressPanel의 오브젝트들
    public GameObject btn_next;
    public GameObject btn_prev;
    public GameObject graph_playtime;
    public GameObject graph_accuracy;
    public GameObject graph_progress;
    private int page_index = 0;

    public GameObject top_graph_playtime;
    public GameObject top_graph_accuracy;
    public GameObject top_graph_progress;

    // 로컬 JSON에서 불러온 데이터들을 저장할 배열
    private List<GameSession> gameRecords;
    private List<GameSession> vm;
    private List<GameSession> sp;
    private List<GameSession> fg;
    private List<GameSession> pc;
    private List<GameSession> sr;

    void Start()
    {
        // Load data
        vm = LocalDataManager.Instance.GetGameSessions("vm");
        sp = LocalDataManager.Instance.GetGameSessions("sp");
        fg = LocalDataManager.Instance.GetGameSessions("fg");
        pc = LocalDataManager.Instance.GetGameSessions("pc");
        sr = LocalDataManager.Instance.GetGameSessions("sr");

        // Aggregate all game sessions
        gameRecords = new List<GameSession>();
        gameRecords.AddRange(vm);
        gameRecords.AddRange(sp);
        gameRecords.AddRange(fg);
        gameRecords.AddRange(pc);
        gameRecords.AddRange(sr);

        // Render panels
        RenderRecordPanel();
        RenderProgressPanel();

        // Initial graph display
        ProgressRadarGraphRender(page_index);
    }

    void RenderRecordPanel()
    {
        // Ensure the template is active for instantiation
        Content_template.SetActive(true);

        void RenderGameRecords(List<GameSession> gameSessions, string gameName)
        {
            for (int i = 0; i < gameSessions.Count; i++)
            {
                GameObject newContent = Instantiate(Content_template, Content_template.transform.parent);
                newContent.name = "GameData_" + gameName + "_" + (i + 1);

                string date_day = gameSessions[i].date.Substring(0, 10);
                string date_second = gameSessions[i].date.Substring(11, 8);

                newContent.transform.Find("text_date_day").GetComponent<Text>().text = date_day;
                newContent.transform.Find("text_date_second").GetComponent<Text>().text = date_second;
                newContent.transform.Find("text_others").GetComponent<Text>().text = " " + gameName + " / Lv " + gameSessions[i].lvl + " / " + gameSessions[i].prog + " / " + gameSessions[i].corr + " % / " + gameSessions[i].time + "s";

                newContent.SetActive(true);
            }
        }

        // Render records for each game
        RenderGameRecords(vm, "vm");
        RenderGameRecords(sp, "sp");
        RenderGameRecords(fg, "fg");
        RenderGameRecords(pc, "pc");
        RenderGameRecords(sr, "sr");

        // Deactivate template
        Content_template.SetActive(false);
    }

    void RenderProgressPanel()
    {
        float[] playtime = new float[5];
        float[] accuracy = new float[5];
        float[] progress = new float[5];
        int[] gameCount = new int[5];

        void AggregateGameData(List<GameSession> gameSessions, int index)
        {
            foreach (var session in gameSessions)
            {
                playtime[index] += session.time;
                accuracy[index] += session.corr;
                progress[index] += session.prog;
                gameCount[index]++;
            }

            if (gameCount[index] != 0)
            {
                accuracy[index] /= gameCount[index];
            }
        }

        // Aggregate data for each game
        AggregateGameData(vm, 0);
        AggregateGameData(pc, 1);
        AggregateGameData(fg, 2);
        AggregateGameData(sr, 3);
        AggregateGameData(sp, 4);

        // Insert data into graphs
        if (graph_playtime != null && graph_accuracy != null && graph_progress != null)
        {
            RadarChart playtimeChart = graph_playtime.GetComponent<RadarChart>();
            RadarChart accuracyChart = graph_accuracy.GetComponent<RadarChart>();
            RadarChart progressChart = graph_progress.GetComponent<RadarChart>();

            playtimeChart.DataSource.SetValue("Player 1", "VM", playtime[0]);
            playtimeChart.DataSource.SetValue("Player 1", "PC", playtime[1]);
            playtimeChart.DataSource.SetValue("Player 1", "FG", playtime[2]);
            playtimeChart.DataSource.SetValue("Player 1", "SR", playtime[3]);
            playtimeChart.DataSource.SetValue("Player 1", "SP", playtime[4]);

            accuracyChart.DataSource.SetValue("Player 1", "VM", accuracy[0]);
            accuracyChart.DataSource.SetValue("Player 1", "PC", accuracy[1]);
            accuracyChart.DataSource.SetValue("Player 1", "FG", accuracy[2]);
            accuracyChart.DataSource.SetValue("Player 1", "SR", accuracy[3]);
            accuracyChart.DataSource.SetValue("Player 1", "SP", accuracy[4]);

            progressChart.DataSource.SetValue("Player 1", "VM", progress[0]);
            progressChart.DataSource.SetValue("Player 1", "PC", progress[1]);
            progressChart.DataSource.SetValue("Player 1", "FG", progress[2]);
            progressChart.DataSource.SetValue("Player 1", "SR", progress[3]);
            progressChart.DataSource.SetValue("Player 1", "SP", progress[4]);
        }
    }

    public void next()
    {
        page_index++;
        if (page_index > 2)
        {
            page_index = 0;
        }
        ProgressRadarGraphRender(page_index);
    }

    public void prev()
    {
        page_index--;
        if (page_index < 0)
        {
            page_index = 2;
        }
        ProgressRadarGraphRender(page_index);
    }

    void ProgressRadarGraphRender(int index)
    {
        switch (index)
        {
            case 0:
                top_graph_playtime.SetActive(true);
                top_graph_accuracy.SetActive(false);
                top_graph_progress.SetActive(false);
                break;
            case 1:
                top_graph_playtime.SetActive(false);
                top_graph_accuracy.SetActive(true);
                top_graph_progress.SetActive(false);
                break;
            case 2:
                top_graph_playtime.SetActive(false);
                top_graph_accuracy.SetActive(false);
                top_graph_progress.SetActive(true);
                break;
        }
    }

}