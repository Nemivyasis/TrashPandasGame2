using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScenePostHighScore : MonoBehaviour
{

    public void OnClick()
    {
        HighScore.instance.AddHighScoreEntry(ScoreSingleton.instance.score, "BOB");
        SceneManager.LoadScene(HighScore.instance.postHighScoreScene);
    }
}
