using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuControl : MonoBehaviour
{
    //[SerializeField] private SoundPanelControl spControl;
    [SerializeField] private Text[] arTxtRecItems;

    [SerializeField] private RawImage riAvatar;
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void LoadTiara()
    {
        SceneManager.LoadScene("TiaraScene");
    }
    public void ViewScore()
    {
        txtScore.text = $"{GameManager.Instance.currentPlayer.totalScore}     {GameManager.Instance.currentPlayer.countWin}({GameManager.Instance.currentPlayer.countBattle})";
    }
    public void ViewAvatar()
    {
        txtName.text = GameManager.Instance.currentPlayer.playerName;
        riAvatar.texture = GameManager.Instance.currentPlayer.photo;
        Debug.Log($"ViewAvatar => name={GameManager.Instance.currentPlayer.playerName}");
    }
    public void ViewLeaderboard(string strJson)
    {
        if (strJson == "")
        {
            Debug.Log("ViewLeaderboard strJson= <" + strJson + ">");
            for (int i = 0; i < arTxtRecItems.Length; i++) arTxtRecItems[i].text = "";
            return;
        }
        try
        {
            //Debug.Log("ViewLeaderboard => " + strJson);
            //PersonRecord[] data = JsonConvert.DeserializeObject<PersonRecord[]>(strJson);
            //PersonRecord[] data = JsonUtility.FromJson<PersonRecord[]>(strJson);
            PersonRecord[] data = GetDataFromJson(strJson);
            //Debug.Log("data=>" + data);
            //StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length && i < arTxtRecItems.Length; i++)
            {
                arTxtRecItems[i].text = $"{data[i]}";
                //Debug.Log("VL => " + data[i].ToString());
                //sb.Append($"{data[i]}\n");
            }
            //txtDescrLeader.text = sb.ToString();
            //Debug.Log("VL sb=" + sb.ToString());
        }
        catch
        {
            arTxtRecItems[0].text = Language.Instance.CurrentLanguage == "ru" ? "Ошибка" : "Error";
        }
        //panelLiders.SetActive(true);
    }

    private PersonRecord[] GetDataFromJson(string s)
    {
        List<PersonRecord> arr = new List<PersonRecord>();
        string[] ss = s.Split("{");
        for (int i = 1; i < ss.Length; i++)
        {
            int end = ss[i].LastIndexOf('}');
            //Debug.Log($"ss[i]={ss[i]} end={end}");
            string strJson = $"{ss[i].Substring(0, end)}";
            strJson = "{" + strJson + "}";
            //Debug.Log($"strJson={strJson}");
            PersonRecord pr = JsonUtility.FromJson<PersonRecord>(strJson);
            //Debug.Log($"pr={pr}");
            arr.Add(pr);
        }

        return arr.ToArray();
    }
}

[Serializable]
public class MyArrRecords
{
    public PersonRecord[] records { get; set; }
    public MyArrRecords() { }
    public override string ToString()
    {
        return $"Counts={records.Length}";
    }
}

[Serializable]
public class PersonRecord
{
    //public int Rank { get; set; }
    public int Rank;
    //public int Score { get; set; }
    public int Score;
    //public string Name { get; set; }
    public string Name;

    public PersonRecord() { }
    public PersonRecord(int r, int sc, string nm)
    {
        Rank = r;
        Score = sc;
        Name = nm;
    }
    public override string ToString()
    {
        //string nm = String.Format("{0,-25}", Name);
        //return $"{Rank:00} {nm} {Score}";
        return $"{Rank:00} {Name} {Score}";
    }
}

