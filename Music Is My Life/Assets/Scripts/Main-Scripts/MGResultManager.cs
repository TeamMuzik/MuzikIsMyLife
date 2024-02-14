using UnityEngine;

public class MGResultManager : MonoBehaviour
{
    public static (string, string) PartTimeDayResult(int behaviorId)
    {
        string resultRes, stressRes;
        int stressDiff;

        float p = Random.value;
        Debug.Log("알바 하루 결과 | 확률: " + p);

        if (p < 0.05f) // 5%의 확률로 가장 좋은 결과
        {
            resultRes = "완벽하게 알바를 해냈다!";
            stressDiff = -20;
        }
        else if (p < 0.15f) // 10%의 확률로 조금 좋은 결과
        {
            if (behaviorId == 2)
                resultRes = "만들기 쉬운 곰인형이 많았다.";
            else
                resultRes = "오늘은 조금 한가했다.";
            stressDiff = -10;

        }
        else if (p < 0.35f) // 20%의 확률로 안 좋은 결과
        {
            switch (behaviorId)
            {
                case 0: // 카페알바
                    if (p < 0.25f)
                        resultRes = "진상 손님을 만났다…";
                    else
                        resultRes = "손님이 많아서 힘든 하루였다.";
                    break;
                case 1: // 사무직알바
                    resultRes = "업무량이 많아서 야근을 했다…";
                    break;
                case 2: // 공장알바
                    resultRes = "만들기 어려운 강아지 인형이 많았다.";
                    break;
                default:
                    throw new System.Exception("오늘 한 행동이 알바가 아닙니다. behaviorId: " + behaviorId);
            }
            stressDiff = 20;
        }
        else // 기본
        {
            resultRes = "알바를 했더니 피곤하다...";
            stressDiff = 10;

        }
        // 3일 연속으로 알바를 한 경우 스트레스 5 추가됨
        if (PlayerPrefs.GetInt("PartTimeContinuity") == 3)
        {
            PlayerPrefs.SetInt("PartTimeContinuity", 0); // 0회로 초기화
            resultRes += "\n3일 연속으로 알바를 했더니 힘들다...";
            stressDiff += 5;
        }
        StatusChanger.UpdateStress(stressDiff);
        if (stressDiff > 0)
            stressRes = "(스트레스 +" + stressDiff + ")";
        else
            stressRes = "(스트레스 " + stressDiff + ")";
        return (resultRes, stressRes);
    }

    public static (string, string) JjirasiDayResult(bool won)
    {
        string resultRes, stressRes;
        int stressDiff;

        if (!won) // 빌런에게 졌을 때
        {
            resultRes = "어그로 경쟁에서 패배한 날이었다...";
            stressDiff = 20;
        }
        else
        {
            float p = Random.value;
            Debug.Log("찌라시 하루 결과 | 확률: " + p);
            if (p < 0.10f) // 10%의 확률로 좋은 결과
            {
                resultRes = "사람들이 찌라시를 잘 받아줬다.";
                stressDiff = -10;
            }
            else if (p < 0.30f) // 20%의 확률로 안 좋은 결과
            {
                resultRes = "경찰 단속에 걸렸다...";
                stressDiff = 20;
            }
            else
            {
                resultRes = "사람들이 찌라시를 잘 받아주지 않았다...";
                stressDiff = 10;
            }
        }
        StatusChanger.UpdateStress(stressDiff);
        if (stressDiff > 0)
            stressRes = "(스트레스 +" + stressDiff + ")";
        else
            stressRes = "(스트레스 " + stressDiff + ")";
        return (resultRes, stressRes);
    }

    public static int CoverDayResult()
    {
        float p = Random.value;
        Debug.Log("커버 하루 결과 | 확률: " + p);
        if (p < 0.05f) // 5%의 확률로 가장 좋은 결과
        {
            StatusChanger.UpdateStress(-20);
            StatusChanger.UpdateMyFame(+10);
            StatusChanger.UpdateBandFame(+10);
            return 1;
        }
        else if (p < 0.15f) // 10%의 확률로 좋은 결과
        {
            StatusChanger.UpdateStress(-10);
            return 2;
        }
        else if (p < 0.35f) // 20%의 확률로 안 좋은 결과
        {
            StatusChanger.UpdateStress(+20);
            return 3;
        }
        else
        {
            return 4;
        }
    }
}