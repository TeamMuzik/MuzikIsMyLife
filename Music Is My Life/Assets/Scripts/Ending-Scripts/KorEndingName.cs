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

        switch (sceneName)
        {
            case "ConcertInKorea":
                endingName = "엔딩 #3: 야옹의 내한";
                break;
            case "Expedition":
                endingName = "엔딩 #2: 원정을 가다";
                break;
            case "Normal":
                endingName = "엔딩 #1: 다시 백수로...";
                break;
            case "OpeningBand":
                endingName = "엔딩 #4: 스타가 되다";
                break;
            case "Rich":
                endingName = "엔딩 #5: 벼락 부자";
                break;
            case "TooFamousToBeg":
                endingName = "엔딩 #6: 나락 엔딩";
                break;
            default:
                endingName = "게임 오버";
                break;
        }
        PlayerPrefs.SetString("EndingName", endingName);
    }
}