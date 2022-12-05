using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct ScoreEntry
{
    public ScoreEntry(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
    public string PlayerName;
    public int Score;
}

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    class SaveData
    {
        public List<ScoreEntry> HallOfFameList;
    }

    public static GameManager Instance;
    public string PlayerName;
    public string PlayerNameHighscore;
    public int Highscore;
    public List<ScoreEntry> HallOfFameList = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Load();
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateHallOfFame(string playerName, int score)
    {
        ScoreEntry newScore = new ScoreEntry(playerName, score);

        if (HallOfFameList.Count == 0)
        {
            HallOfFameList.Add(newScore);
            return;
        }

        int indexToAdd = -1;
        for (int i = 0; i < HallOfFameList.Count; i++)
        {
            if (HallOfFameList[i].Score < newScore.Score)
            {
                indexToAdd = i;
                break;
            }
        }

        if (indexToAdd == -1 && HallOfFameList.Count < 5)
        {
            HallOfFameList.Add(newScore);
        }

        if (indexToAdd == -1) return;

        HallOfFameList.Insert(indexToAdd, newScore);

        if (HallOfFameList.Count > 5)
        {
            ScoreEntry lowestScore = HallOfFameList[0];
            foreach (ScoreEntry scoreEntry in HallOfFameList)
            {
                if (lowestScore.Score > scoreEntry.Score)
                {
                    lowestScore = scoreEntry;
                }
            }
            HallOfFameList.Remove(lowestScore);
        }
    }

    public void Save()
    {
        SaveData data = new SaveData();
        data.HallOfFameList = new List<ScoreEntry>(HallOfFameList);

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log(Application.persistentDataPath);

        if (!File.Exists(path))
            return;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (data.HallOfFameList != null)
        {
            HallOfFameList = new List<ScoreEntry>(data.HallOfFameList);
            PlayerNameHighscore = HallOfFameList[0].PlayerName;
            Highscore = HallOfFameList[0].Score;
        }
        else
        {
            HallOfFameList = new();
        }
    }

}
