using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BubbleCollectionScript : MonoBehaviour
{
    int score = 0;
    public Text label;
    public AudioClip popFX;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bubble")
        {
            source.PlayOneShot(popFX);
            score += 100;
            collider.gameObject.SetActive(false);
            UpdateScore(score);
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Fish")
        {
            source.PlayOneShot(popFX);
            score += 100;
            collider.gameObject.SetActive(false);
            UpdateScore(score);
        }
		if(collider.gameObject.tag == "Damager")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

    void UpdateScore(int score)
    {
        label.text = ""+score;
    }
}
