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
            case "NormalBandFame":
                endingName = "엔딩 #3: 처음이자 마지막 내한";
                ending = "Ending-NormalBandFame";
                break;
            case "GreatBandFame":
                endingName = "엔딩 #4: 야옹의 내한";
                ending = "Ending-GreatBandFame";
                break;
            case "ExcellentBandFame":
                endingName = "엔딩 #5: 박 터지는 티켓팅";
                ending = "Ending-ExcellentBandFame";
                break;
            case "NormalMyFame":
                endingName = "엔딩 #6: 애매한 스타";
                ending = "Ending-NormalMyFame";
                break;
            case "GreatMyFame":
                endingName = "엔딩 #7: 유튜브 콜라보";
                ending = "Ending-GreatMyFame";
                break;
            case "ExcellMyFame":
                endingName = "엔딩 #8: 스타는 외로워";
                ending = "Ending-ExcellMyFame";
                break;
            case "Entertainment":
                endingName = "엔딩 #9: 공연 기획사";
                ending = "Ending-Entertainment";
                break;
            case "WanDuck":
                endingName = "엔딩 #10: 완덕";
                ending = "Ending-Entertainment";
                break;
             case "BackStage":
                endingName = "엔딩 #11: 백스테이지";
                ending = "Ending-BackStage";
                break;
            case "Collaboration":
                endingName = "엔딩 #12: 콜라보 공연";
                ending = "Ending-Collaboration";
                break;
            case "Rich":
                endingName = "엔딩 #13: 벼락 부자";
                ending = "Ending-Rich";
                break;
            case "Narak":
                endingName = "엔딩 #14: 나락도 락이다";
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
