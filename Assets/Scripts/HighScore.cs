using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highScoreEntryTransformList;

    private float templateHeight = 1.5f;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);



        string jsonString = PlayerPrefs.GetString("highScoreTable");
        SavedHighscores highscores = JsonUtility.FromJson<SavedHighscores>(jsonString);

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

        for (int i = 0; i < 5; i++)
        {
            CreateHighScoreEntryTransform(highscores.highScoreEntries[i], entryContainer, highScoreEntryTransformList);
        }
    }

    private void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default:
                rankString = rank + "th"; break;

            case 1: rankString = "1st"; break;
            case 2: rankString = "2nd"; break;
            case 3: rankString = "3rd"; break;
        }
        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highScoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highScoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;


        transformList.Add(entryTransform);
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
}
