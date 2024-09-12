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
            case "Normal":
                endingName = "엔딩 #1: 다시 백수로...";
                ending = "Ending-Normal";
                break;
            case "Expedition":
                endingName = "엔딩 #2: 원정을 가다";
                ending = "Ending-Expedition";
                break;
            case "Rich":
                endingName = "엔딩 #3: 벼락 부자";
                ending = "Ending-Rich";
                break;
            case "Narak":
                endingName = "엔딩 #4: 나락도 락이다";
                ending = "Ending-Narak";
                break;
            case "NormalMyFame":
                endingName = "엔딩 #5: 속상해도 열심히!";
                ending = "Ending-NormalMyFame";
                break;
            case "GreatMyFame":
                endingName = "엔딩 #6: 야옹과 합주";
                ending = "Ending-GreatMyFame";
                break;
            case "ExcellMyFame":
                endingName = "엔딩 #7: 스타는 외로워";
                ending = "Ending-ExcellMyFame";
                break;
            case "NormalBandFame":
                endingName = "엔딩 #8: 마지막 내한";
                ending = "Ending-NormalBandFame";
                break;
            case "GreatBandFame":
                endingName = "엔딩 #9: 야옹의 내한";
                ending = "Ending-GreatBandFame";
                break;
            case "ExcellentBandFame":
                endingName = "엔딩 #10: 내한의 신 야옹";
                ending = "Ending-ExcellentBandFame";
                break;
            case "Entertainment":
                endingName = "엔딩 #11: 기획사를 세우다";
                ending = "Ending-Entertainment";
                break;
            case "WanDuck":
                endingName = "엔딩 #12: 덕질의 끝";
                ending = "Ending-WanDuck";
                break;
            case "BackStage":
                endingName = "엔딩 #13: 공연에 초대받다";
                ending = "Ending-BackStage";
                break;
            case "Collaboration":
                endingName = "엔딩 #14: 야옹과의 콜라보 앨범 발매";
                ending = "Ending-Collaboration";
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
