using UnityEngine;

public class DayFortune : MonoBehaviour
{
    // 오늘의 Forture Id 번호 가져오기
    public static int GetTodayFortuneId()
    {
        int dayNum = PlayerPrefs.GetInt("Dday");
        int fortuneId = PlayerPrefs.GetInt($"Day{dayNum}_Fortune");
        return fortuneId;
    }

    public static int RandomDraw()
    {
        int dayNum = PlayerPrefs.GetInt("Dday");
        int fortuneId = Random.Range(1, 11);
        PlayerPrefs.SetInt($"Day{dayNum}_Fortune", fortuneId);
        Debug.Log("PickTodayFortune: " + fortuneId);
        return fortuneId;
    }

    public static string GetFortuneMessage(int fortuneId)
    {
        string fortuneMessage;
        switch (fortuneId)
        {
            case 1:
                fortuneMessage = "돈의 요정이 찾아올 것 같아요";
                break;
            case 2:
                fortuneMessage = "평온한 하루가 될 것 같다";
                break;
            case 3:
                fortuneMessage = "누구든 이길 수 있음";
                break;
            case 4:
                fortuneMessage = "당신의 매력이 발휘되는 하루";
                break;
            case 5:
                fortuneMessage = "귀인을 만나게 되리라";
                break;
            case 6:
                fortuneMessage = "휴식을 권장하오";
                break;
            case 7:
                fortuneMessage = "피곤한 하루가 될 것 같다";
                break;
            case 8:
                fortuneMessage = "사람의 갈등을 피하세요";
                break;
            case 9:
                fortuneMessage = "너무 많은 관심은 독이 된다";
                break;
            case 10:
                fortuneMessage = "세상에 공짜는 없다";
                break;
            default:
                throw new System.Exception("fortuneId가 잘못되었습니다.");
        }
        return fortuneMessage;
    }

    public static string GetEffectMessage(int fortuneId)
    {
        string effectMessage;
        switch (fortuneId)
        {
            case 1:
                effectMessage = "알바비 보너스 지급 +5";
                break;
            case 2:
                effectMessage = "스트레스 감소 !";
                break;
            case 3:
                effectMessage = "찌라시 빌런 하";
                break;
            case 4:
                effectMessage = "커버 증가율 + 1000명";
                break;
            case 5:
                effectMessage = "부자 엔딩 확률 +1%";
                break;
            case 6:
                effectMessage = "휴식을 할 경우 스트레스 많이 감소(-50), 일하면 스트레스 많이 증가(+10)";
                break;
            case 7:
                effectMessage = "알바 하드모드";
                break;
            case 8:
                effectMessage = "찌라시 빌런 상";
                break;
            case 9:
                effectMessage = "커버 증가율 -500명";
                break;
            case 10:
                effectMessage = "구걸을 할 경우 돈을 얻지 못함";
                break;
            default:
                throw new System.Exception("fortuneId가 잘못되었습니다.");
        }
        return effectMessage;
    }
}