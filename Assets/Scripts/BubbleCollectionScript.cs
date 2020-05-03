using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BubbleCollectionScript : MonoBehaviour
{
    public int score;
    public Text label;
    public AudioClip popFX;
    public AudioClip biteFX;
    private AudioSource source;

    void Awake()
    {
        if(ScoreSingleton.instance != null)
        {
            score = ScoreSingleton.instance.score;
            UpdateScore(score);
        }

        source = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bubble")
        {
            source.PlayOneShot(popFX);
            AddScore();
            collider.gameObject.SetActive(false);
        }
        if (collider.gameObject.tag == "Damager")
        {

            source.PlayOneShot(biteFX);
            Invoke("DelayReloadScene", 0.3f);
        }

    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Fish")
        {
            source.PlayOneShot(biteFX);
            AddScore();
            collider.gameObject.SetActive(false);
        }
		if(collider.gameObject.tag == "Damager")
		{
            source.PlayOneShot(biteFX);
            Invoke("DelayReloadScene", 0.3f);
		}
	}

    void AddScore()
    {
        score += 100;

        if (ScoreSingleton.instance != null)
        {
            ScoreSingleton.instance.score = score;
        }

        UpdateScore(score);
    }

    void DelayReloadScene ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateScore(int score)
    {
        label.text = "" + score;
    }
}
