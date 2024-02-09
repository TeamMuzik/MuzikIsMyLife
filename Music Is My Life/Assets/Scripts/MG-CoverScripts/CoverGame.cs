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
    private int rate = 500; // 처음 초기 증가율 (명)

    void Start()
    {
        totalSubs = PlayerPrefs.GetInt("Subscribers");
        newSubs = 0;
        newFame = 0;
        turn = 5; // 5에서 0까지 감소
        /* 장비 버프에 따라 rate를 변경하는 코드 */
        dialTitle.text = "";
        dialContent.text = "커버곡 연주를 시작합니다.\n(클릭해서 시작)";
        subsStatus.text = "구독자수: " + totalSubs;
        fameStatus.text = "얻은 명성: " + newFame + "\n누적 명성: " + PlayerPrefs.GetInt("MyFame");
        scorePanel.SetActive(false); // 결과 보기 비활성화
        Debug.Log("CoverGame 씬 시작");
    }

    public IEnumerator CoverCouroutine() // 커버 게임 코루틴
    {
        int divisor = 2500; // 명성 오르는 단위
        int beforeFame = PlayerPrefs.GetInt("MyFame");

        dialTitle.text = "야옹의 곡을 기타 커버중...";
        while (turn-- > 0)
        {
            newSubs = Random.Range(1, 11) * rate; // 랜덤하게 구독자수 증가
            dialContent.text = "구독자 수가 " + newSubs + "명 증가하였습니다.";
            int fameDiff = (totalSubs + newSubs) / divisor - totalSubs / divisor;
            if (fameDiff > 0) // divisor 단위가 바뀌는지 확인하기 위해
            {
                newFame += fameDiff;
                fameStatus.text = "얻은 명성: " + newFame + "\n누적 명성: " + (newFame + beforeFame);
            }
            totalSubs += newSubs; // 구독자수 증가
            subsStatus.text = "구독자수: " + totalSubs;
            Debug.Log("Coroutine이 0.5초 기다림, turn: " + turn);
            yield return new WaitForSeconds(0.5f); // 특정 시간 동안 기다림, 테스트용으로 0.5초
        }
        PlayerPrefs.SetInt("Subscribers", totalSubs); // 구독자수 업데이트
        StatusChanger.UpdateMyFame(newFame); // 명성 증가
        resultContent.text = "얻은 명성: " + newFame + "\n누적 명성: " + PlayerPrefs.GetInt("MyFame");
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