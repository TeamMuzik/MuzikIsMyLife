using UnityEngine;
using TMPro;
using System.Collections;

public class CoverGame : MonoBehaviour
{
    public TMP_Text subsStatus;
    public TMP_Text fameStatus;
    public TMP_Text message;
    public GameObject finishBtn; // 필드로 버튼 오브젝트 받음

    private int totalSubs = 0; // PlayerPrefs에 넣지 않고, 이 스크립트가 불러와진 가장 처음에 초기화
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

        message.text = "커버곡 연주를 시작합니다.\n(클릭해서 시작)";
        subsStatus.text = "구독자수: " + totalSubs;
        fameStatus.text = "획득한 명성: " + newFame;
        finishBtn.SetActive(false); // 나가기 버튼 비활성화
        Debug.Log("CoverGame 씬 시작");
    }

    public IEnumerator CoverCouroutine() // 커버 게임 코루틴
    {
        while (turn-- > 0)
        {
            newSubs = Random.Range(1, 11) * rate; // 랜덤하게 구독자수 증가
            message.text = "구독자 수가 " + newSubs + "명 증가하였습니다.";
            totalSubs += newSubs; // 구독자수 증가
            subsStatus.text = "구독자수: " + totalSubs;
            if (totalSubs / 3000 > (totalSubs - newSubs) / 3000) // 3000 단위가 바뀌었는지 확인하기 위해
            {
                newFame++;
                fameStatus.text = "획득한 명성: " + newFame;
            }
            Debug.Log("Coroutine이 0.5초 기다림, turn: " + turn);
            yield return new WaitForSeconds(0.5f); // 특정 시간 동안 기다림, 테스트용으로 0.5초
        }
        PlayerPrefs.SetInt("Subscribers", totalSubs); // 구독자수 업데이트
        StatusChanger.UpdateMyFame(newFame); // 명성 증가
        finishBtn.SetActive(true); // 나가기 버튼 활성화 (클릭 시 Main으로)
        Debug.Log("명성 증가량: " + newFame + "& 나가기 버튼 활성화");
        yield break; // 코루틴 끝내기
    }
    public void AddSubcribers()
    {
        if (turn == 5) // turn이 4 이하이면 버튼 클릭하지 못하도록
            StartCoroutine(CoverCouroutine()); // 코루틴 시작
    }

}