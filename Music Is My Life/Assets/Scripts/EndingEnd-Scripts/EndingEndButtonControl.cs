using UnityEngine;

public class EndingEndButtonControl : MonoBehaviour
{
    public GameObject creditButton;
    public GameObject titleButton;
    public GameObject askButton;

    void Start()
    {
        int seasonNum = PlayerPrefs.GetInt("SeasonNum");
        string endingScene = PlayerPrefs.GetString("Ending");
        creditButton.SetActive(true);
        titleButton.SetActive(true);
        askButton.SetActive(false);

        if (seasonNum != 1 && seasonNum != 2) // seasonNum이 1이거나 2일 때만 다음 시즌으로 진행 가능
            return;
        switch (endingScene)
        {
            case "Ending-Normal":
            case "Ending-GameOver":
                creditButton.SetActive(false);
                titleButton.SetActive(false);
                askButton.SetActive(true);
                break;
        }
    }
}