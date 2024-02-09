using System.Collections;
using UnityEngine;

public class MGResultManager : MonoBehaviour
{
    public static int PartTimeDayResult()
    {
        float p = Random.value;
        Debug.Log("확률: " + p);
        if (PlayerPrefs.GetInt("PartTimeContinuity") == 3) // 3일 연속으로 알바 성공
        {
            PlayerPrefs.SetInt("PartTimeContinuity", 0); // 0회로 초기화
            StatusChanger.UpdateStress(-20);
            return 1;
        }
        else if (p < 0.20f) // 20%의 확률로 바쁜 하루, 스트레스 +20
        {
            StatusChanger.UpdateStress(+20);
            return 2;
        }       
        else
        {
            StatusChanger.UpdateStress(+10);
            return 3;
        }
    }

    public static int JjirasiDayResult()
    {
        float p = Random.value;
        Debug.Log("확률: " + p);
        if (p < 0.10f) // 10%의 확률로 좋은 결과
        {
            StatusChanger.UpdateStress(-10);
            return 1;
        }
        else if (p < 0.30f) // 20%의 확률로 안 좋은 결과
        {
            StatusChanger.UpdateStress(+20);
            return 2;
        }
        else
        {
            StatusChanger.UpdateStress(+10);
            return 3;
        }
    }

    public static int CoverDayResult()
    {
        float p = Random.value;
        Debug.Log("확률: " + p);
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
            StatusChanger.UpdateStress(+10);
            return 3;
        }
        else
        {
            return 4;
        }
    }

    /*public static int BeggingDayResult()
    {
        if (PlayerPrefs.GetInt("MyFame") >= 20) // 내 명성이 20 이상일 경우
        {
            float p = Random.value;
            Debug.Log("확률: " + p);
            if (p * 100 < PlayerPrefs.GetInt("MyFame")) // 내 명성의 확률로 팬이 알아봄?
            {
                return 1;
            }
        }
        return 2; // 알아보지 않음
    }*/
}