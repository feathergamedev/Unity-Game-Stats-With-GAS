using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager instance;

    // TODO: Replace this with your {Current web app URL} from GAS
    private static readonly string GAS_URL = "USE_YOUR_CurrentWebAppURL_FROM_GAS";

    public static PlayerStatsManager Instance
    {
        get
        {
            if (instance == null)
            {
                var node = new GameObject("PlayerStatsManager");
                instance = node.AddComponent<PlayerStatsManager>();
                DontDestroyOnLoad(node);
            }

            return instance;
        }
    }

    public enum PostMethod
    {
        GainNewVisitor,
        AddNewScore,
        GetScoreByRank,
        GetScoreOfTopN,
    }

    public struct FieldData
    {
        public string FieldName;
        public string FieldValue;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GainNewVisitor(Action<string> onComplete = null)
    {
        StartCoroutine(ConnectToGAS(PostMethod.GainNewVisitor, onComplete));
    }

    public void AddNewScore(int myScore, Action<string> onComplete = null)
    {
        FieldData scoreField;
        scoreField.FieldName = "score";
        scoreField.FieldValue = myScore.ToString();

        StartCoroutine(ConnectToGAS(PostMethod.AddNewScore, onComplete, new List<FieldData>() { scoreField }));
    }

    public void GetScoreByRank(int rank, Action<string> onComplete = null)
    {
        FieldData rankField;
        rankField.FieldName = "targetRank";
        rankField.FieldValue = rank.ToString();

        StartCoroutine(ConnectToGAS(PostMethod.GetScoreByRank, onComplete, new List<FieldData>() { rankField }));
    }

    public void GetScoreOfTopN(int number, Action<string> onComplete = null)
    {
        FieldData numberField;
        numberField.FieldName = "number";
        numberField.FieldValue = number.ToString();

        StartCoroutine(ConnectToGAS(PostMethod.GetScoreOfTopN, onComplete, new List<FieldData>() { numberField }));
    }

    private IEnumerator ConnectToGAS(PostMethod method, Action<string> onComplete = null, List<FieldData> fieldData = null)
    {
        WWWForm form = new WWWForm();

        form.AddField("method", method.ToString());

        if (fieldData != null)
        {
            foreach (FieldData f in fieldData)
            {
                form.AddField(f.FieldName, f.FieldValue);
            }
        }

        using (UnityWebRequest www = UnityWebRequest.Post(GAS_URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var outputMsg = www.downloadHandler.text;
                onComplete?.Invoke(outputMsg);

                Debug.LogFormat("Method : {0}, Message : {1}", method.ToString(), outputMsg);
            }
        }
    }
}
