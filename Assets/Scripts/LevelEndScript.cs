﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScript : MonoBehaviour
{
    [SerializeField]
    private string nextScene = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HighScore.instance.CheckIfHighScore(ScoreSingleton.instance.score))
        {
            HighScore.instance.postHighScoreScene = nextScene;
            SceneManager.LoadScene("NewHighScore");

        }
        else if (nextScene != "" && collision.gameObject.tag == "Player")
            SceneManager.LoadScene(nextScene);
    }
}
