using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleCollectionScript : MonoBehaviour
{
    int score = 0;
    public Text label;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bubble")
        {
            score += 100;
            collider.gameObject.SetActive(false);
            UpdateScore(score);
        }
    }

    void UpdateScore(int score)
    {
        label.text = ""+score;
    }
}
