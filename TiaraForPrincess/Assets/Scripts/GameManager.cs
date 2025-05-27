using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void LoadYandex();

    [DllImport("__Internal")]
    private static extern void SaveYandex(string strJson);

    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(int value);


    public PlayerInfo currentPlayer = new PlayerInfo();
    [SerializeField] private MenuControl mm_control;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Invoke("LoadGame", 0.08f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }

    void LoadGame()
    {
#if UNITY_WEBGL
        LoadYandex();
#endif
#if UNITY_EDITOR
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            Debug.Log(Application.persistentDataPath + "/MySaveData.dat");
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            //Debug.Log(data.ToString());
            UpdateLoadingData(data);
        }
        else
        {
            Debug.Log("There is no save data!");
            GameManager.Instance.currentPlayer = PlayerInfo.FirstGame();
            if (mm_control != null)
            {
                //mm_control.OnPoleSelect(GameManager.Instance.currentPlayer.currentPole);
                //mm_control.OnModeSelect(GameManager.Instance.currentPlayer.currentMode);
                //mm_control.ViewScore();
                //mm_control.UpdateAudioSource();
            }
        }
#endif
    }

    public void UpdateLoadingData(SaveData data)
    {
        GameManager.Instance.currentPlayer.isLoaded = true;

        GameManager.Instance.currentPlayer.countBattle = data.countBattle;
        GameManager.Instance.currentPlayer.countWin = data.countWin;
        GameManager.Instance.currentPlayer.totalScore = data.score;

        GameManager.Instance.currentPlayer.inventory = new Inventory(data.csvInventory);
        GameManager.Instance.currentPlayer.tiaraData = new TiaraData(data.csvTiaraData);

        //GameManager.Instance.currentPlayer.maxQwScore = data.qwMaxScore;
        //GameManager.Instance.currentPlayer.maxHexScore = data.hexMaxScore;
        //GameManager.Instance.currentPlayer.maxPrismScore = data.prismMaxScore;
        GameManager.Instance.currentPlayer.isHintView = data.isHints;
        GameManager.Instance.currentPlayer.isSoundFone = data.isFone;
        GameManager.Instance.currentPlayer.isSoundEffects = data.isEffects;
        GameManager.Instance.currentPlayer.volumeFone = data.volFone;
        GameManager.Instance.currentPlayer.volumeEffects = data.volEffects;


        //Debug.Log("Game data loaded! Score=" + GameManager.Instance.currentPlayer.totalScore.ToString() + "  Gold=" + GameManager.Instance.currentPlayer.totalGold.ToString());
        Debug.Log($"Game data loaded! Score={GameManager.Instance.currentPlayer.totalScore}  InventoryCount={GameManager.Instance.currentPlayer.inventory.Count}   TiaraDataCount={GameManager.Instance.currentPlayer.tiaraData.ElementsCount}");

        if (mm_control != null)
        {
            //mm_control.OnPoleSelect(GameManager.Instance.currentPlayer.currentPole);
            //mm_control.OnModeSelect(GameManager.Instance.currentPlayer.currentMode);
            //mm_control.ViewScore();
            //mm_control.UpdateSound();
            //mm_control.ViewScore();
            //mm_control.UpdateAudioSource();
        }
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/MySaveData.dat");
        SaveData data = new SaveData();

        data.score = GameManager.Instance.currentPlayer.totalScore;
        //data.qwMaxScore = GameManager.Instance.currentPlayer.maxQwScore;
        //data.hexMaxScore = GameManager.Instance.currentPlayer.maxHexScore;
        //data.prismMaxScore = GameManager.Instance.currentPlayer.maxPrismScore;
        data.countBattle = GameManager.Instance.currentPlayer.countBattle;
        data.countWin = GameManager.Instance.currentPlayer.countWin;

        data.csvInventory = GameManager.Instance.currentPlayer.inventory.ToCsvString();
        data.csvTiaraData = GameManager.Instance.currentPlayer.tiaraData.ToCsvString();

        data.isHints = GameManager.Instance.currentPlayer.isHintView;
        data.isFone = GameManager.Instance.currentPlayer.isSoundFone;
        data.isEffects = GameManager.Instance.currentPlayer.isSoundEffects;
        data.volFone = GameManager.Instance.currentPlayer.volumeFone;
        data.volEffects = GameManager.Instance.currentPlayer.volumeEffects;

        //DateTime dt = DateTime.Now;
        //data.timeString = $"{dt.Year:0000}-{dt.Month:00}-{dt.Day:00}-{dt.Hour:00}";

#if UNITY_WEBGL
        string jsonStr = JsonUtility.ToJson(data);
        SaveYandex(jsonStr);
        SetToLeaderboard(GameManager.Instance.currentPlayer.totalScore);
#endif

        //PlayerInfo.Instance.Save(jsonStr);
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }
}

public class PlayerInfo
{
    public bool isLoaded = false;
    public int totalScore = 0;
    public int currentScore = 0;
    public int countBattle = 0;
    public int countWin = 0;

    public Inventory inventory;
    public Inventory currentInventory;

    public TiaraData tiaraData;

    public int countSecond = 0;
    public int countLine = 0;
    public int countFigure = 0;
    public int countHardFigure = 0;
    public bool isSurprise = false;

    public int maxQwScore = 0;
    public int maxHexScore = 0;
    public int maxPrismScore = 0;

    /*    public int contructions;
        public int decors;
        public int company;

        public int maxLevel;
        public int currentLevel;
        public int currentLive = 3;
        public int currentGold = 0;
        public int currentHint = 0;

        public int totalGold = 0;

        public int maxHP = 100;
        public int currentHP = 100;
        public int maxMagic = 10;
        public int currentMagic = 0;
        public int maxFire = 10;
        public int currentFire = 0;
        public int maxToxin = 10;
        public int currentToxin = 0;
    */
    public bool isHintView = true;
    public bool isSoundFone = true;
    public bool isSoundEffects = true;
    public int volumeFone = 50;
    public int volumeEffects = 100;

    public string playerName = "-------";
    public Texture photo = null;


    public PlayerInfo()
    {
        //maxLevel = 0;
        //currentLevel = 0;
        inventory = new Inventory();
        tiaraData = new TiaraData();
    }

    public static PlayerInfo FirstGame()
    {
        return new PlayerInfo();
    }

    public void LevelComplete()
    {
        //UpdateReward(currentLevel);
        /*currentLevel++;
        if (currentLevel > maxLevel)
        {
            maxLevel = currentLevel;
        }*/
        totalScore += currentScore;
    }

    /*    public void UpdateReward(int numLevel)
        {
            LevelInfo lev = LevelLogic.arrLevels[numLevel];
            switch (lev.rw.type)
            {
                case 0:
                    maxHP += lev.rw.value;
                    break;
                case 1:
                    maxFire += lev.rw.value;
                    break;
                case 2:
                    maxMagic += lev.rw.value;
                    break;
                case 3:
                    maxToxin += lev.rw.value;
                    break;
            }
            Debug.Log($"UpdateReward : maxHP={maxHP} maxFire={maxFire} maxMagic={maxMagic} maxToxin={maxToxin} reward: << {lev.rw.ToString()} >>  lev={lev}");
        }*/

    public void ClearCurrentParam()
    {
        currentScore = 0;
        /*currentGold = 0;
        currentHP = maxHP;
        currentMagic = 0;
        currentFire = 0;
        currentToxin = 0;*/
    }

    public void AddBonus(int bonusID)
    {
        switch (bonusID)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}

[Serializable]
public class SaveData
{
    public int countBattle;
    public int countWin;
    public int score;
    //public int qwMaxScore;
    //public int hexMaxScore;
    //public int prismMaxScore;
    public string csvInventory = "";
    public string csvTiaraData = "";

    public bool isFone;
    public bool isEffects;
    public bool isHints;
    public int volFone;
    public int volEffects;
    public override string ToString()
    {
        return "SaveData: mode=" + countBattle + " pole=" + countWin;
    }
}

