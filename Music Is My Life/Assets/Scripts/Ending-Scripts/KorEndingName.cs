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
                endingName = "내한 엔딩";
                break;
            case "Expedition":
                endingName = "원정 엔딩";
                break;
            case "Normal":
                endingName = "노멀 엔딩";
                break;
            case "OpeningBand":
                endingName = "오프닝 밴드 엔딩";
                break;
            case "Rich":
                endingName = "부자 엔딩";
                break;
            case "TooFamousToBeg":
                endingName = "나락 엔딩";
                break;
            default:
                endingName = "게임 오버";
                break;
        }
        PlayerPrefs.SetString("EndingName", endingName);
    }
}