using UnityEngine;
using TMPro;

public class StatusController : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text dday;
    public TMP_Text date;
    public TMP_Text status;

    void Start()
    {
        //추후 GameManager 등 통해 처음인지 파악 필요함
        StatusChanger.UpdateDay(); // 날짜 업데이트
        if (PlayerPrefs.GetInt("Dday") <= 0)
        {
            GoToEnding();
        }
        NameText(); // 플레이어 이름
        DdayText();
        DateText();
        StatusText();
    }

    public void GoToEnding() // 엔딩으로
    {
        int money = PlayerPrefs.GetInt("Money");
        int myFame = PlayerPrefs.GetInt("MyFame");
        int bandFame = PlayerPrefs.GetInt("Fame");

        SceneMove sceneMove = new SceneMove();
        if (money > 2500000)
        {
            sceneMove.targetScene = "Ending-Expedition";
        }
        else if (myFame > 100)
        {
            sceneMove.targetScene = "Ending-OpeningBand";
        }
        else if (bandFame > 15)
        {
            sceneMove.targetScene = "Ending-ConcertInKorea";
        }
        else
        {
            sceneMove.targetScene = "Ending-Normal";
        }
        sceneMove.ChangeScene();
    }
    public void NameText()
    {
        playerName.text = PlayerPrefs.GetString("PlayerName") + "의 상태";
    }

    public void DdayText()
    {
        dday.text = "D-" + PlayerPrefs.GetInt("Dday");
    }
    public void DateText()
    {
        date.text = PlayerPrefs.GetString("Date");
    }

    public void StatusText()
    {
        status.text = "돈: " + PlayerPrefs.GetInt("Money") + "원"
                    + "\n내 명성: " + PlayerPrefs.GetInt("MyFame")
                    + "\n야옹의 명성: " + PlayerPrefs.GetInt("BandFame")
                    + "\n스트레스: " + PlayerPrefs.GetInt("Stress") + "%";
    }
}