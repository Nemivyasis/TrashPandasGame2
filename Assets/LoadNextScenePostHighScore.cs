using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScenePostHighScore : MonoBehaviour
{

    public void OnClick()
    {
        SceneManager.LoadScene(HighScore.instance.postHighScoreScene);
    }
}
