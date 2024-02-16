using UnityEngine;

public class KorEndingName : MonoBehaviour
{
    public string sceneName;

    void Start()
    {
        SaveKorEndingName();
    }

    void SaveKorEndingName()
    {
        string endingName;
        string ending;
        switch (sceneName)
        {
            case "ConcertInKorea":
                endingName = "엔딩 #3: 야옹의 내한";
                ending = "Ending-ConcertInKorea";
                break;
            case "Expedition":
                endingName = "엔딩 #2: 원정을 가다";
                ending = "Ending-Expedition";
                break;
            case "Normal":
                endingName = "엔딩 #1: 다시 백수로...";
                ending = "Ending-Normal";
                break;
            case "OpeningBand":
                endingName = "엔딩 #4: 스타가 되다";
                ending = "Ending-OpeningBand";
                break;
            case "Rich":
                endingName = "엔딩 #5: 벼락 부자";
                ending = "Ending-Rich";
                break;
            case "TooFamousToBeg":
                endingName = "엔딩 #6: 나락";
                ending = "Ending-Narak";
                break;
            default:
                endingName = "게임 오버";
                ending = "";
                break;
        }
        PlayerPrefs.SetString("EndingName", endingName);
        PlayerPrefs.SetString("Ending", ending);
    }
}
