using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuitGame : MonoBehaviour
{
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void SetTimeScale(int timeScale) 
    {
        Time.timeScale = timeScale;
    }
}
