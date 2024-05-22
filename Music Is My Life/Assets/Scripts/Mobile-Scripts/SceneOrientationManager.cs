using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOrientationManager : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MG-Office") // Replace with your game scene name
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
