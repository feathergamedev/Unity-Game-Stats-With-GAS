using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DEMO_PlayerStatsManager : MonoBehaviour
{
    public static DEMO_PlayerStatsManager instance;

    private static readonly string GAS_URL = "https://script.google.com/macros/s/AKfycbxsqiK3SKyekA7J3O9ahr9c2EOJtu-3ZvIlJU3y9DNMENmkG2I/exec";

    public static DEMO_PlayerStatsManager Instance
    {
        get
        {
            if (instance == null)
            {
                var node = new GameObject("DEMO_PlayerStatsManager");
                instance = node.AddComponent<DEMO_PlayerStatsManager>();
                DontDestroyOnLoad(node);
            }

            return instance;
        }
    }

    public enum PostMethod
    {
        GainNewVisitor,
        GainNewFinisher,
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

    public void GainNewFinisher(Action<string> onComplete = null)
    {
        StartCoroutine(ConnectToGAS(PostMethod.GainNewFinisher, onComplete));
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
