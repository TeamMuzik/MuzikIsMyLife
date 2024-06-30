
using UnityEngine;

public class CompUIController : MonoBehaviour
{
    public GameObject compStartPanel;
    public GameObject fortuneOpenPanel;
    public GameObject fortuneContentPanel;
    public GameObject snsOpenPanel;

    private void Start()
    {
        compStartPanel.SetActive(true);
        fortuneOpenPanel.SetActive(false);
        fortuneContentPanel.SetActive(false);
        snsOpenPanel.SetActive(false);
    }
}