using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public string targetScene;
    public void ChangeScene()
    {
        SceneManager.LoadScene(targetScene); //해당 씬으로 이동
    }
}