using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CoverGame : MonoBehaviour
{
    public TMP_Text subsStatus;
    public TMP_Text fameStatus;
    public TMP_Text dialTitle;
    public TMP_Text dialContent;
    public GameObject scorePanel; // 점수판
    public TMP_Text resultContent; // 점수 결과
    public TMP_Text resultDay; // 오늘 하루 결과
    public GameObject characterObject;
    public List<Sprite> availableSprites; // 커버 이미지 교체할 스프라이트

    private int totalSubs; // PlayerPrefs에서 가져옴
    private int newSubs;
    private int beforeMyFame;
    private int newFame;
    private int turn;

    private const int unit = 500; // 구독자 수 증가 단위 (명)
    private int rangeStart; // 구독자 수 최소치 / 5
    private int rangeEnd; // 구독자 수 최대치 / 5
    private int subsMultiplier; // 구독자 증가 배율

    void Start()
    {
        totalSubs = PlayerPrefs.GetInt("Subscribers");
        newSubs = 0;
        beforeMyFame = PlayerPrefs.GetInt("MyFame");
        newFame = 0;
        turn = 5; // 5에서 0까지 감소

        // 음향기기 버프 적용
        rangeStart = PlayerPrefs.GetInt("Subs_Min") / 500;
        rangeEnd = PlayerPrefs.GetInt("Subs_Max") / 500;
        subsMultiplier = PlayerPrefs.GetInt("Subs_Multiplier");
        Debug.Log("CoverGame- Subs_Min: " + (rangeStart*500) + "Subs_Max: " + (rangeEnd*500) + "Subs_Multiplier: " + subsMultiplier);

        dialTitle.text = "야옹의 곡을 기타 커버 (클릭 시 자동 진행)";
        dialContent.text = $"구매한 음향기기에 따라 구독자 수가 증가합니다.\n"
            + "구독자 수가 늘어나면 나의 명성이 올라갑니다.\n"
            + $"(현재 구독자 증가 범위: {rangeStart * 500 * subsMultiplier}명~{rangeEnd * 500 * subsMultiplier}명)"
            ;
        subsStatus.text = "구독자 수: " + totalSubs + "명";
        fameStatus.text = "나의 명성: " + beforeMyFame;
        scorePanel.SetActive(false); // 결과 보기 비활성화
    }

    public void StartCoverGame()
    {
        if (turn == 5) // turn이 4 이하이면 버튼 클릭하지 못하도록
            StartCoroutine(CoverCouroutine()); // 코루틴 시작
    }

    public IEnumerator CoverCouroutine() // 커버 게임 코루틴
    {
        int divisor = 2500; // 명성 오르는 단위

        while (turn-- > 0)
        {
            newSubs = Random.Range(rangeStart, rangeEnd + 1) * unit * subsMultiplier; // 랜덤하게 구독자 수 증가
            int fameDiff = (totalSubs + newSubs) / divisor - totalSubs / divisor;
            if (fameDiff > 0) // divisor 단위가 바뀌는지 확인하기 위해
            {
                newFame += fameDiff;
                fameStatus.text = "나의 명성: " + (beforeMyFame + newFame) + " (+" + newFame + ")";
                dialContent.text = "구독자가 " + newSubs + "명 늘었다.\n(나의 명성 +" + fameDiff+")";
            }
            else
                dialContent.text = "구독자가 " + newSubs + "명 늘었다.";

            totalSubs += newSubs; // 구독자 수 증가
            subsStatus.text = "구독자 수: " + totalSubs + "명";
            // Debug.Log("turn: " + turn + "| 증가한 구독자 수: " + newSubs + " | 증가한 명성: " + fameDiff);

            // 시간 기다리기
            // 0번 스프라이트
            ChangeCoverSprite(0);
            dialTitle.text = "   기타 연주중   ";
            yield return new WaitForSeconds(0.2f);
            dialTitle.text = "   기타 연주중.  ";
            yield return new WaitForSeconds(0.2f); // 특정 시간 동안 기다림

            // 1번 스프라이트
            ChangeCoverSprite(1);
            dialTitle.text = "   기타 연주중.. ";
            yield return new WaitForSeconds(0.2f); // 특정 시간 동안 기다림
            dialTitle.text = "   기타 연주중...";
            yield return new WaitForSeconds(0.2f);
        }
        CoverGameResult();
        yield break; // 코루틴 끝내기
    }

    public void CoverGameResult()
    {
        PlayerPrefs.SetInt("Subscribers", totalSubs); // 구독자 수 업데이트
        StatusChanger.UpdateMyFame(newFame); // 명성 증가
        // 결과창
        resultContent.text = "구독자 수: " + PlayerPrefs.GetInt("Subscribers") + "명\n나의 명성: " + PlayerPrefs.GetInt("MyFame") + " (+" + newFame + ")";
        scorePanel.SetActive(true); // 나가기 버튼 활성화 (클릭 시 Main으로)
        Debug.Log("명성 증가량: " + newFame + ", 점수판 활성화");

        int result = MGResultManager.CoverDayResult();
        if (result == 1)
        {
            resultDay.text = "인기 급상승 동영상에 올라갔다!!\n" +
                "(스트레스 -20, 나의 명성 +10, 야옹의 명성 +10)";
        }
        else if (result == 2)
        {
            resultDay.text = "응원 댓글이 많이 달렸다!\n" +
                "(스트레스 -10)";
        }
        else if (result == 3)
        {
            resultDay.text = "악플이 많이 달렸다...\n" +
                "(스트레스 +20)";
        }
        else
        {
            resultDay.text = "커버 영상을 올리니까 뿌듯하다!\n";
        }
    }

    public void ChangeCoverSprite(int index)
    {
        SpriteRenderer spriteRenderer = characterObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && index >= 0 && index < availableSprites.Count)
        {
            // 선택한 인덱스에 해당하는 스프라이트 할당
            spriteRenderer.sprite = availableSprites[index];
        }
        else
        {
            Debug.LogError("Sprite Renderer가 설정되지 않았거나, 인덱스가 잘못되었습니다.");
        }
    }
}