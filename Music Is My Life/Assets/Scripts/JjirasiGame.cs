using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JjirasiGame : MonoBehaviour
{
    public int ClickNum;
    public int FameNum;
    private int totalFame;
    public TMP_Text clickTxt;
    public TMP_Text fameTxt;
    public TMP_Text incfameTxt;
    public float LimitTime;
    public TMP_Text text_Timer;

    public Button Jjirasi;
    public Button Main;
    public GameObject jjirasi;
    private bool isClicked = false;
    public Image jjirasiImage; // 스프라이트 변경을 위한 Image 변수
    public Sprite normalSprite; // 기본 스프라이트
    public Sprite clickSprite; // 클릭 시 보여질 스프라이트
    public GameObject DuelPanel;



    private float eventTriggerTime;
    private bool eventTriggered = false;
    public GameObject DuelimagePrefab; // 미리 준비된 Image 프리팹
    public Transform imagesParent; // 이미지들이 배치될 부모 객체 (예: Canvas)

    public GameObject eventUI; // 이벤트 UI
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    public Slider duelSlider; // 대결 진행 바
    public float systemIncreaseRate = 1000.0f; // 시스템 증가율
    private bool duelInProgress = false; // 대결 진행 중 플래그
     private bool duelStarted = false; // 듀얼 시작 여부를 추적하는 변수


    void Start()
    {
        ClickNum = 0;
        FameNum = 0;
        clickTxt.text = "클릭 수 : 0";
        fameTxt.text = "명성 : 0";
        text_Timer.text = "남은 시간: 30";

        LimitTime = 30;
        incfameTxt.gameObject.SetActive(false);
        Main.gameObject.SetActive(false);
        jjirasiImage.sprite = normalSprite;
        UpdateEventTriggerTime(); // eventTriggerTime을 초기화하는 함수 호출
        DuelPanel.SetActive(false);
        DuelimagePrefab.SetActive(false);

    }

    // eventTriggerTime 초기화 함수 추가
    private void UpdateEventTriggerTime()
    {
        eventTriggerTime = Random.Range(10, 20);
        Debug.Log("Event Trigger Time: " + eventTriggerTime); // eventTriggerTime 값 확인을 위한 디버그 로그 추가
    }

    private void TriggerEvent()
    {
      if (!duelStarted)
  {


      StartCoroutine(StartDuelSequence());
      duelStarted = true; // 듀얼이 시작되었음을 표시
  }
    }
    IEnumerator StartDuelSequence()
{
    eventTriggered = true;


    // 스프라이트 순차적 변경
    // 스프라이트 1을 사용하여 새 이미지 생성 및 표시
    GameObject image1 = InstantiateImage(sprite1);
    yield return new WaitForSeconds(1.0f); // 1초 대기

    GameObject image2 = InstantiateImage(sprite2);
    yield return new WaitForSeconds(1.0f); // 1초 대기

    GameObject image3 = InstantiateImage(sprite3);
    yield return new WaitForSeconds(1.0f); // 1초 대기

   Destroy(image1);
   Destroy(image2);
   Destroy(image3);
   DuelimagePrefab.gameObject.SetActive(false);

   StartDuel();
 }
 void StartDuel()
{
  Jjirasi.gameObject.SetActive(true);
  jjirasiImage.gameObject.SetActive(true);
  duelInProgress = true;
    eventUI.SetActive(true);
    duelSlider.gameObject.SetActive(true);


    clickTxt.gameObject.SetActive(false);
    fameTxt.gameObject.SetActive(false);
    text_Timer.gameObject.SetActive(false);
      DuelPanel.SetActive(true);
    duelSlider.value = 0.5f;


}
    // 듀얼 시작 로직

    GameObject InstantiateImage(Sprite sprite)
    {
        GameObject newImageObj = Instantiate(DuelimagePrefab, imagesParent);
        newImageObj.GetComponent<Image>().sprite = sprite;
        newImageObj.SetActive(true);
        return newImageObj;
    }
    private void UpdateFame()
{
    // 기본 클릭에 의한 명성 계산
    int baseFame = ClickNum / 10;
    // 전체 명성은 기본 명성 + 대결 승리 등으로 얻은 추가 명성
    totalFame = baseFame + FameNum;
    fameTxt.text = "명성 : " + totalFame.ToString();
    Debug.Log("Total Fame: " + totalFame);
}

    public void Click()
    {
        if (duelInProgress)
        {
            duelSlider.value += 0.05f;
        }
        else
        {
            ClickNum++;
            clickTxt.text = "클릭 수 : " + ClickNum.ToString();
            Debug.Log("ClickNum: " + ClickNum);
            // GameManager의 InitializeGamedata에 PlayerPrefs.SetInt("JjirasiClick", 0); 추가
            PlayerPrefs.SetInt("JjirasiClick", PlayerPrefs.GetInt("JjirasiClick") + 1);
            UpdateFame();
        }

        if (!isClicked)
        {
            jjirasiImage.sprite = clickSprite; // 클릭 시 스프라이트 변경
            isClicked = true;
            StartCoroutine(ResetSpriteAfterDelay(0.1f)); // 0.1초 후 스프라이트 원래대로
        }
    }

    IEnumerator ResetSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        jjirasiImage.sprite = normalSprite;
        isClicked = false;
    }

    private void EndDuel(bool won)
    {
        duelInProgress = false;
        eventUI.SetActive(false);
        duelSlider.gameObject.SetActive(false);


        if (won)
        {
          FameNum += 10; // 대결 승리 시 명성 10 증가
          fameTxt.text = "명성 : " + FameNum.ToString(); // 명성 점수 업데이트
        }

      eventTriggered = false;
      duelInProgress = false;

      LimitTime = Mathf.Max(LimitTime, 0); // 남은 시간 확인 및 조정

      // 원래 게임 상태로 UI 복원
      clickTxt.gameObject.SetActive(true);
      fameTxt.gameObject.SetActive(true);
      text_Timer.gameObject.SetActive(true);
      Jjirasi.gameObject.SetActive(true);
    }

    void Update()
    {

        // 대결 대기 시간이 끝나고, 타이머가 남아 있으며, 대결이 진행 중이 아닐 때
        if (LimitTime > 0 && !eventTriggered)
        {
            // 타이머 감소
            LimitTime -= Time.deltaTime;
            text_Timer.text = "남은 시간 : " + Mathf.Round(LimitTime);

            if (LimitTime > 0 && !eventTriggered && LimitTime <= eventTriggerTime) // 이벤트 트리거 조건 체크
          {
              TriggerEvent();
             // 이벤트를 트리거했음을 표시
          }
        }

        // 대결 이벤트가 트리거되었고, 대결이 진행 중일 때
        else if (eventTriggered && duelInProgress)
        {
            // 클릭 대결 로직
            duelSlider.value -= systemIncreaseRate * Time.deltaTime; // 시스템 바 감소
            if (duelSlider.value <= 0)
            {
                EndDuel(false); // 시스템 승리
            }
            else if (duelSlider.value >= 0.99)
            {
                EndDuel(true); // 사용자 승리
            }
        }

        // 그 외의 상황 (대결 이벤트가 끝남)
         else if (!duelInProgress)
        {

          duelInProgress = false;

          LimitTime = Mathf.Max(LimitTime, 0);

     // 원래 게임 상태로 UI 복원
        clickTxt.gameObject.SetActive(true);
        fameTxt.gameObject.SetActive(true);
        text_Timer.gameObject.SetActive(true);
        Jjirasi.gameObject.SetActive(true);
        }
          if (LimitTime <= 0 && !duelInProgress)
          {

            Hide();

          }
    }

    private void Hide()
    {
      incfameTxt.text = "증가한 명성: " + totalFame.ToString();
        incfameTxt.text = "증가한 명성: " + totalFame.ToString();
        StatusChanger.UpdateBandFame(totalFame); // 게임 로직에 맞게 수정 필요
        jjirasiImage.gameObject.SetActive(false);

        clickTxt.gameObject.SetActive(false);
        fameTxt.gameObject.SetActive(false);
        text_Timer.gameObject.SetActive(false);
        Jjirasi.gameObject.SetActive(false);
        Main.gameObject.SetActive(true);
        incfameTxt.gameObject.SetActive(true);
    }

}
