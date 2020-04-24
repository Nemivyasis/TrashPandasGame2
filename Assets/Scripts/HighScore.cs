using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighScoreEntry> highScoreEntryList;
    private List<Transform> highScoreEntryTransformList;

    public float templateHeight = 20f;

    private void Awake()
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        highScoreEntryList = new List<HighScoreEntry>()
        {
            new HighScoreEntry{ score = 10000, name = "AAA"},
            new HighScoreEntry{ score = 10000, name = "AAA"},
            new HighScoreEntry{ score = 10000, name = "AAA"},
            new HighScoreEntry{ score = 10000, name = "AAA"},
            new HighScoreEntry{ score = 10000, name = "AAA"}
        };

        highScoreEntryTransformList = new List<Transform>();

        foreach(HighScoreEntry entry in highScoreEntryList)
        {
            CreateHighScoreEntryTransform(entry, entryContainer, highScoreEntryTransformList);
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
        entryTransform.Find("Place").GetComponent<Text>().text = rankString;

        int score = highScoreEntry.score;
        entryTransform.Find("Score").GetComponent<Text>().text = score.ToString();

        string name = highScoreEntry.name;
        entryTransform.Find("Name").GetComponent<Text>().text = name;
    }

    // Represents a single high score entry
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }
}
