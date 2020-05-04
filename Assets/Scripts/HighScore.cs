using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private static HighScore instance;
    public static HighScore Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HighScore>();
                if (instance == null)
                {
                    GameObject g = new GameObject();
                    instance = g.AddComponent<HighScore>();
                    DontDestroyOnLoad(g);
                }
            }
            return instance;
        }
    }

    private List<Transform> highScoreEntryTransformList;
    public string postHighScoreScene;

    private float templateHeight = 1.5f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        string jsonString = PlayerPrefs.GetString("highScoreTable");
        SavedHighscores highscores = JsonUtility.FromJson<SavedHighscores>(jsonString);

        if(highscores == null)
        {
            highscores = new SavedHighscores();
            highscores.highScoreEntries = new List<HighScoreEntry>();

            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("highScoreTable", json);
            PlayerPrefs.Save();
        }

        // sort
        for (int i = 0; i < highscores.highScoreEntries.Count; i++)
        {
            for (int j = i + 1; j < highscores.highScoreEntries.Count; j++)
            {
                if (highscores.highScoreEntries[j].score > highscores.highScoreEntries[i].score)
                {
                    HighScoreEntry tempEntry = highscores.highScoreEntries[i];
                    highscores.highScoreEntries[i] = highscores.highScoreEntries[j];
                    highscores.highScoreEntries[j] = tempEntry;
                }
            }
        }

        highScoreEntryTransformList = new List<Transform>();
    }



    public void AddHighScoreEntry(int score, string name)
    {
        // check name length to only accept 3 letters, caps them all for the user
        if (name.Length > 3)
        {
            name = name.Trim();
            name = name.Substring(0, 3);
            name = name.ToUpper();
        }

        // create
        HighScoreEntry entry = new HighScoreEntry { score = score, name = name };

        // load
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        SavedHighscores highscores = JsonUtility.FromJson<SavedHighscores>(jsonString);

        // store
        highscores.highScoreEntries.Add(entry);

        for (int i = 0; i < highscores.highScoreEntries.Count; i++)
        {
            for (int j = i + 1; j < highscores.highScoreEntries.Count; j++)
            {
                if (highscores.highScoreEntries[j].score > highscores.highScoreEntries[i].score)
                {
                    HighScoreEntry tempEntry = highscores.highScoreEntries[i];
                    highscores.highScoreEntries[i] = highscores.highScoreEntries[j];
                    highscores.highScoreEntries[j] = tempEntry;
                }
            }
        }

        if (highscores.highScoreEntries.Count > 5)
            highscores.highScoreEntries.RemoveAt(highscores.highScoreEntries.Count -1);

        // save
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();
    }

    private class SavedHighscores
    {
        public List<HighScoreEntry> highScoreEntries;
    }

    // Represents a single high score entry
    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }

    public bool CheckIfHighScore(int score)
    {
        // load
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        SavedHighscores highscores = JsonUtility.FromJson<SavedHighscores>(jsonString);

        if (highscores.highScoreEntries.Count < 5)
            return true;

        for (int i = 0; i < highscores.highScoreEntries.Count; i++)
        {
            if (score > highscores.highScoreEntries[i].score)
                return true;
        }
        return false;
    }
}
