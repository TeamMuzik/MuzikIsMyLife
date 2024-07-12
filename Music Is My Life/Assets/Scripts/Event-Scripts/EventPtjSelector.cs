using UnityEngine;

public class EventPtjSelector : MonoBehaviour
{
    public int CalculateMostFreqPtjId()
    {
        int today = PlayerPrefs.GetInt("Dday"); // 날짜 바뀐 상태 (8일)
        int[] ptjBehavior = { 0, 0, 0 };

        for (int dayNum = 1; dayNum < today; dayNum++)
        {
            int behaviorId = PlayerPrefs.GetInt($"Day{dayNum}_Behavior");
            if (behaviorId == 0 || behaviorId == 1 || behaviorId == 2)
                ptjBehavior[behaviorId]++;
        }

        // 연산에 사용한 메소드들 (내부 메소드)
        bool AreAllElementsEqual(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] != array[0])
                    return false;
            }
            return true;
        }
        bool AreAllElementsZero(int[] array)
        {
            foreach (int value in array)
            {
                if (value != 0)
                    return false;
            }
            return true;
        }
        int HandleSpecialCase(int[] array)
        {
            return Random.Range(0, 3); // 특별한 경우 0~2 중 랜덤값 반환
        }

        int mostFreqPtjId = Mathf.Max(ptjBehavior[0], ptjBehavior[1], ptjBehavior[2]); // 가장 큰 값 반환
        if (AreAllElementsEqual(ptjBehavior) || AreAllElementsZero(ptjBehavior))
        {
            mostFreqPtjId = HandleSpecialCase(ptjBehavior);
            Debug.Log($"모든 요소가 같거나 모두 0입니다. mostFreqPtjId: {mostFreqPtjId}");
        }
        return mostFreqPtjId;
    }
}