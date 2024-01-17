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
    private IEnumerator coroutine;

    void Start()
    {
        newSubs = 0;
        newFame = 0;
        turn = 5; // 5에서 0까지 감소
        /* 장비 버프에 따라 rate를 변경하는 코드 */

        message.text = "커버곡 연주를 시작합니다.";
        subsStatus.text = "구독자수: " + totalSubs;
        fameStatus.text = "획득한 명성: " + newFame;
        finishBtn.SetActive(false); // 나가기 버튼 비활성화
    }
    
    public void AddSubcribers()
    {
        if (turn == 0) // 버튼 클릭하지 못하도록
            return;

        while (turn-- > 0)
        {
            newSubs = Random.Range(1, 11) * rate; // 랜덤하게 구독자수 증가
            totalSubs += newSubs; // 구독자수 증가
            subsStatus.text = "구독자수: " + totalSubs;
            message.text = "구독자 수가 " + newSubs + "명 증가하였습니다.";
            if (totalSubs / 3000 > (totalSubs - newSubs) / 3000) // 3000 단위가 바뀌었는지 확인하기 위해
            {
                newFame++;
                fameStatus.text = "획득한 명성: " + newFame;
            }
            coroutine = WaitAndPrint(2.0f);
            StartCoroutine(coroutine);
        }
        StatusChanger.UpdateFame(newFame); // 명성 증가

        finishBtn.SetActive(true); // 나가기 버튼 활성화 (클릭 시 Main으로)
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            // 애니메이션 반복 실행
        }
    }
}