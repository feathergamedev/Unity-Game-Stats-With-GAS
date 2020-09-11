using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEMO_StatsController : MonoBehaviour
{

    [SerializeField]
    private Text m_visitorCountText;



    [SerializeField]
    private InputField m_scoreInputField;

    [SerializeField]
    private Text m_rankOfScoreText;



    [SerializeField]
    private InputField m_rankInputField;

    [SerializeField]
    private Text m_scoreOfRankText;



    [SerializeField]
    private Text m_leaderBoardText;

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
            RefreshLeaderBoard(x);
        });
    }

    private void RefreshLeaderBoard(string str)
    {
        var data = str.Split(',');

        m_leaderBoardText.text = "";

        for (int i = 0; i < 10; i++)
        {
            m_leaderBoardText.text += string.Format("No.{0} : {1}\n", i + 1, data[i]);
        }
    }

    public void OpenSourceSpreadsheet()
    {
        Application.OpenURL("https://docs.google.com/spreadsheets/d/13cJe2iY1ZHyHjrYoayrAQ85KvbhapfnAS_RvOmu5ll4/edit?usp=sharing");
    }
}
