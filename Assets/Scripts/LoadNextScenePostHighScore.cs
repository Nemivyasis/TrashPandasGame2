using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextScenePostHighScore : MonoBehaviour
{

    public InputField nameInput;
    string playerName;

    public void OnClick()
    {
        playerName = nameInput.text;

        HighScore.Instance.AddHighScoreEntry(ScoreSingleton.Instance.score, playerName);
        SceneManager.LoadScene(HighScore.Instance.postHighScoreScene);
    }
}
