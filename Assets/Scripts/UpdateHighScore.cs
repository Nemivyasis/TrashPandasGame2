using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHighScore : MonoBehaviour
{
    public Text scoreNumObject;

    void OnAwake()
    {
        scoreNumObject.text = "" + ScoreSingleton.instance.score;
    }
}
