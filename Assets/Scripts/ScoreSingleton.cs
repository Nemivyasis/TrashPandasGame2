using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSingleton : MonoBehaviour
{
	private static ScoreSingleton instance;
	public static ScoreSingleton Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<ScoreSingleton>();
				if(instance == null)
				{
					GameObject g = new GameObject();
					instance = g.AddComponent<ScoreSingleton>();
					DontDestroyOnLoad(instance);
				}
			}
			return instance;
		}
	}
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
