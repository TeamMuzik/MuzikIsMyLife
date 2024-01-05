using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("Fame", 0);
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("Stress", 0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
