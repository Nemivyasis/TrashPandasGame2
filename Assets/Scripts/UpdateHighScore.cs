using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHighScore : MonoBehaviour
{
    public Text scoreNumObject;

    void Awake()
    {
        scoreNumObject.text = "" + ScoreSingleton.Instance.score;
    }
}
