using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEMO_Controller : MonoBehaviour
{

    [SerializeField]
    private Text m_visitorCountText;

    [SerializeField]
    private Text m_finisherCountText;



    [SerializeField]
    private InputField m_scoreInputField;

    [SerializeField]
    private Text m_rankOfScoreText;



    [SerializeField]
    private InputField m_rankInputField;

    [SerializeField]
    private Text m_scoreOfRankText;



    [SerializeField]
    private Text m_leaderboardText;

    // Start is called before the first frame update
    void Start()
    {
        GainNewVisitor();
        GetScoreOfTop10();
    }

    private void GainNewVisitor()
    {
        DEMO_PlayerStatsManager.Instance.GainNewVisitor((x) =>
        {
            ShowVisitorCount(x);
        });
    }

    private void ShowVisitorCount(string visitorNum)
    {
        m_visitorCountText.text = string.Format("你是第 {0} 位使用者！", visitorNum);
    }

    public void GainNewFinisher()
    {
        DEMO_PlayerStatsManager.Instance.GainNewFinisher((x) =>
        {
            ShowFinisherCount(x);
        });
    }

    private void ShowFinisherCount(string finisherNum)
    {
        m_finisherCountText.text = string.Format("你是第 {0} 位通關者！", finisherNum);
    }

    public void SendScoreToLeaderboard()
    {
        var score = 0;
        var isNumber = int.TryParse(m_scoreInputField.text, out score);

        if (!isNumber)
            return;

        DEMO_PlayerStatsManager.Instance.AddNewScore(score, (x) =>
        {
            ShowTheRankOfScore(score, x);
        });
    }

    private void ShowTheRankOfScore(int score, string rank)
    {
        m_rankOfScoreText.text = string.Format("Score : {0}\nRank  : No.{1}", score, rank);
    }

    public void SearchScoreByRank()
    {
        var rank = 1;
        var isNumber = int.TryParse(m_rankInputField.text, out rank);

        if (!isNumber || rank <= 0)
            return;

        DEMO_PlayerStatsManager.Instance.GetScoreByRank(rank, (x) =>
        {
            ShowTheScoreOfTheRank(rank, x);
        });
    }

    private void ShowTheScoreOfTheRank(int rank, string score)
    {
        m_scoreOfRankText.text = string.Format("Rank   : No.{0}\nScore  : {1}", rank, score);
    }

    public void GetScoreOfTop10()
    {
        DEMO_PlayerStatsManager.Instance.GetScoreOfTopN(10, (x) =>
        {
            RefreshLeaderboard(x);
        });
    }

    private void RefreshLeaderboard(string str)
    {
        var data = str.Split(',');

        m_leaderboardText.text = "";

        for (int i = 0; i < 10; i++)
        {
            m_leaderboardText.text += string.Format("No.{0} : {1}\n", i + 1, data[i]);
        }
    }

    // Should only be used inside the engine.
    public void OpenSourceSpreadsheet()
    {
        Application.OpenURL("https://docs.google.com/spreadsheets/d/13cJe2iY1ZHyHjrYoayrAQ85KvbhapfnAS_RvOmu5ll4/edit?usp=sharing");
    }
}
