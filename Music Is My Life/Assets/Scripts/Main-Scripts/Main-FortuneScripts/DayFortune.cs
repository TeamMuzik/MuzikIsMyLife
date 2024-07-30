
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

    public static void ResetYdayFortuneEffect()
    {
        int ydayNum = PlayerPrefs.GetInt("Dday") - 1;
        if (ydayNum == 0)
        {
            Debug.Log("ResetFortuneEffect: 어제 적용된 행운이 없습니다.");
            return;
        }
        int fortuneId = PlayerPrefs.GetInt($"Day{ydayNum}_Fortune");
        switch (fortuneId)
        {
            case 4:
                // 커버 증가 최소최대 +1000명 돌려놓기
                StatusChanger.UpdateSubsMin(-1000);
                StatusChanger.UpdateSubsMax(-1000);
                break;
            case 6:
                // 휴식을 할 경우 스트레스 많이 감소(-50), 다른 일 하면 스트레스 많이 증가(+10)
                int behaviorId = PlayerPrefs.GetInt($"Day{ydayNum}_Behavior");
                if (behaviorId == 7)
                    StatusChanger.UpdateStress(-50);
                else
                    StatusChanger.UpdateStress(+10);
                break;
            case 9:
                // 커버 증가 최소최대 -500명 돌려놓기
                StatusChanger.UpdateSubsMin(+500);
                StatusChanger.UpdateSubsMax(+500);
                break;
            case int n when (n == 1 || n == 2 || n == 3 || n == 5 || n == 7 || n == 8 || n == 10):
                // 1, 3, 5, 7, 8, 10은 미니게임 내에서 해결함. 2는 일회성.
                Debug.Log($"ResetFortuneEffect: fortuneId가 {n}일 때는 되돌려야 하는 변수가 없습니다.");
                break;

            // 행운 미적용
            case -1:
                Debug.Log("ResetFortuneEffect: 어제 적용된 행운이 없습니다.");
                return;
            // 비정상
            case 0:
                throw new System.Exception("ResetFortuneEffect: 날짜가 잘못되었습니다. 15일 이상일 때 이 에러가 발생할 수 있습니다.");
            default:
                throw new System.Exception("ResetFortuneEffect: fortuneId가 잘못되었습니다. fortuneId: " + fortuneId);
        }
        Debug.Log("전날의 행운을 되돌렸습니다: " + fortuneId);
    }

    public static void AddOrLogFortuneEffect(int fortuneId)
    {
        string effectMessage;
        switch (fortuneId)
        {
            case 1: // 적용 확인 필요
                effectMessage = "알바비 보너스 지급 +5";
                break;
            case 2:
                effectMessage = "스트레스 감소!";
                StatusChanger.UpdateStress(-10);
                break;
            case 3: // 적용 확인 필요
                effectMessage = "찌라시 빌런 하";
                break;
            case 4:
                effectMessage = "커버 증가율 + 1000명";
                StatusChanger.UpdateSubsMin(+1000);
                StatusChanger.UpdateSubsMax(+1000);
                break;
            case 5:
                effectMessage = "부자 엔딩 확률 +1%";
                break;
            case 6:
                effectMessage = "휴식을 할 경우 스트레스 많이 감소(-50), 일하면 스트레스 많이 증가(+10)";
                break;
            case 7: // 적용 확인 필요
                effectMessage = "알바 하드모드";
                break; // 적용 확인 필요
            case 8:
                effectMessage = "찌라시 빌런 상";
                break;
            case 9:
                effectMessage = "커버 증가율 -500명";
                StatusChanger.UpdateSubsMin(-500);
                StatusChanger.UpdateSubsMax(-500);
                break;
            case 10:
                effectMessage = "구걸을 할 경우 돈을 얻지 못함";
                break;

            // 비정상
            case -1:
                throw new System.Exception("AddOrLogFortuneEffect: 행운을 정상적으로 뽑지 못하여 효과가 적용되지 않았습니다.");
            case 0:
                throw new System.Exception("AddOrLogFortuneEffect: 날짜가 잘못되었습니다. 15일 이상일 때 이 에러가 발생할 수 있습니다.");
            default:
                throw new System.Exception("AddFortuneEffect: fortuneId가 잘못되었습니다. fortuneId: " + fortuneId);
        }
        Debug.Log(effectMessage);
    }

    /*public static string GetFortuneMessage(int fortuneId)
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
    }*/
}
