using UnityEngine;
using TMPro;
using System.Collections;

public class CoverGame : MonoBehaviour
{
    public TMP_Text subsStatus;
    public TMP_Text fameStatus;
    public TMP_Text dialTitle;
    public TMP_Text dialContent;
    public GameObject scorePanel; // 점수판
    public TMP_Text resultContent; // 점수 결과

    private int totalSubs; // PlayerPrefs에서 가져옴
    private int newSubs;
    private int newFame;
    private int turn;

    private const int unit = 500; // 구독자수 증가 단위 (명)
    private int rangeStart; // 구독자수 최소치 / 5
    private int rangeEnd; // 구독자수 최대치 / 5
    private int subsMultiplier; // 구독자 증가 배율

    void Start()
    {
        totalSubs = PlayerPrefs.GetInt("Subscribers");
        newSubs = 0;
        newFame = 0;
        turn = 5; // 5에서 0까지 감소

        dialTitle.text = "";
        dialContent.text = "야옹의 곡을 기타 커버한다.\n(클릭 시 자동 진행)";
        subsStatus.text = "구독자 수: " + totalSubs;
        fameStatus.text = "증가한 명성: " + newFame + "\n나의 명성: " + PlayerPrefs.GetInt("MyFame");
        scorePanel.SetActive(false); // 결과 보기 비활성화

        // 음향기기 버프 적용
        rangeStart = PlayerPrefs.GetInt("Subs_Min") / 500;
        rangeEnd = PlayerPrefs.GetInt("Subs_Max") / 500;
        subsMultiplier = PlayerPrefs.GetInt("Subs_Multiplier");
        Debug.Log("CoverGame- Subs_Min: " + (rangeStart*500) + "Subs_Max: " + (rangeEnd*500) + "Subs_Multiplier: " + subsMultiplier);
    }

    public IEnumerator CoverCouroutine() // 커버 게임 코루틴
    {
        int divisor = 2500; // 명성 오르는 단위
        int beforeFame = PlayerPrefs.GetInt("MyFame");

        dialTitle.text = "기타 연주중...";
        while (turn-- > 0)
        {
            newSubs = Random.Range(rangeStart, rangeEnd + 1) * unit * subsMultiplier; // 랜덤하게 구독자수 증가
            int fameDiff = (totalSubs + newSubs) / divisor - totalSubs / divisor;
            if (fameDiff > 0) // divisor 단위가 바뀌는지 확인하기 위해
            {
                newFame += fameDiff;
                fameStatus.text = "증가한 명성: " + newFame + "\n누적 명성: " + (newFame + beforeFame);
                dialContent.text = "구독자가 " + newSubs + "명 늘었다.\n(나의 명성 +" + fameDiff+")";
            }
            else
                dialContent.text = "구독자가 " + newSubs + "명 늘었다.";

            totalSubs += newSubs; // 구독자수 증가
            subsStatus.text = "구독자 수: " + totalSubs;
            Debug.Log("turn: " + turn + "증가한 구독자 수: " + newSubs + "증가한 명성: " + fameDiff);
            yield return new WaitForSeconds(0.5f); // 특정 시간 동안 기다림, 테스트용으로 0.5초
        }
        PlayerPrefs.SetInt("Subscribers", totalSubs); // 구독자수 업데이트
        StatusChanger.UpdateMyFame(newFame); // 명성 증가
        // 결과창
        resultContent.text = "증가한 명성: " + newFame + "\n나의 명성: " + PlayerPrefs.GetInt("MyFame");
        scorePanel.SetActive(true); // 나가기 버튼 활성화 (클릭 시 Main으로)
        Debug.Log("명성 증가량: " + newFame + ", 점수판 활성화");
        yield break; // 코루틴 끝내기
    }
    public void StartCoverGame()
    {
        if (turn == 5) // turn이 4 이하이면 버튼 클릭하지 못하도록
            StartCoroutine(CoverCouroutine()); // 코루틴 시작
    }
}