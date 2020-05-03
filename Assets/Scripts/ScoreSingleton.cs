using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSingleton : MonoBehaviour
{
    public static ScoreSingleton instance;
    public int score = 0;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
