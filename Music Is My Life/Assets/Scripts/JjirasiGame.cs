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
    public TMP_Text clickTxt;
    public TMP_Text fameTxt;
    public TMP_Text incfameTxt;
    public float LimitTime;
    public TMP_Text text_Timer;
    public TMP_Text DuelClickTxt;
    public Button Jjirasi;
    public Button Main;
    public GameObject jjirasi;
    private bool isClicked = false;
    public Image jjirasiImage; // 스프라이트 변경을 위한 Image 변수
    public Sprite normalSprite; // 기본 스프라이트
    public Sprite clickSprite; // 클릭 시 보여질 스프라이트

    private int duelClicks;

    private float eventTriggerTime;
    private bool eventTriggered = false;
    public GameObject eventUI; // 이벤트 UI
    public Slider duelSlider; // 대결 진행 바
    public float systemIncreaseRate = 0.7f; // 시스템 증가율
    private bool duelInProgress = false; // 대결 진행 중 플래그
    private float nextDuelTime = 0f; // 다음 대결까지 남은 시간

    void Start()
    {
        ClickNum = 0;
        FameNum = 0;
        clickTxt.text = "클릭 수 : 0";
        fameTxt.text = "명성 : 0";
        text_Timer.text = "남은 시간: 30";
        DuelClickTxt.text = "0";
        LimitTime = 30;
        incfameTxt.gameObject.SetActive(false);
        Main.gameObject.SetActive(false);
        jjirasiImage.sprite = normalSprite;
        eventTriggerTime = Random.Range(0, 30);
        eventUI.SetActive(false);
        duelSlider.gameObject.SetActive(false);
        DuelClickTxt.gameObject.SetActive(false);
    }



    private void TriggerEvent()
    {
        eventTriggered = true;
        duelInProgress = true;
        duelClicks = 0;
        DuelClickTxt.text = "0";
        eventUI.SetActive(true);
        duelSlider.gameObject.SetActive(true);
        DuelClickTxt.gameObject.SetActive(true);
        clickTxt.gameObject.SetActive(false);
        fameTxt.gameObject.SetActive(false);
        text_Timer.gameObject.SetActive(false);
        duelSlider.value = 0.5f;
    }

    public void Click()
    {
        if (duelInProgress)
        {
            duelClicks++;
            duelSlider.value += 0.1f;
            int reward = duelClicks * 3;
            DuelClickTxt.text = reward.ToString(); // 대결 중 클릭 시 바 증가
        }
        else
        {
            ClickNum++;
            clickTxt.text = "클릭 수 : " + ClickNum.ToString();
            Debug.Log("ClickNum: " + ClickNum);

            FameNum = ClickNum / 10;
            fameTxt.text = "명성 : " + FameNum.ToString();
            Debug.Log("FameNum: " + FameNum);
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
        DuelClickTxt.gameObject.SetActive(false); // 슬라이더 비활성화


        if (won)
        {
            int reward = duelClicks * 3; // 대결 중 클릭 수를 기반으로 보상 계산
            ClickNum += reward;
            clickTxt.text = "클릭 수 : " + ClickNum.ToString();
            FameNum += reward;
        }

        eventTriggerTime = LimitTime - Random.Range(3, Mathf.Max(3, (int)LimitTime));
        eventTriggered = false;
        duelInProgress = false;
        nextDuelTime = 0; // 대결 대기 시간을 0으로 설정
        LimitTime = Mathf.Max(LimitTime, 0);

        // 원래 게임 상태로 UI 복원
        clickTxt.gameObject.SetActive(true);
        fameTxt.gameObject.SetActive(true);
        text_Timer.gameObject.SetActive(true);
        Jjirasi.gameObject.SetActive(true);
    }
    void Update()
{
    // 대결 대기 시간 감소
    if (nextDuelTime > 0)
    {
        nextDuelTime -= Time.deltaTime;
    }

    // 대결 대기 시간이 끝나고, 타이머가 남아 있으며, 대결이 진행 중이 아닐 때
    else if (LimitTime > 0 && !eventTriggered && !duelInProgress)
    {
        // 타이머 감소
        LimitTime -= Time.deltaTime;
        text_Timer.text = "남은 시간 : " + Mathf.Round(LimitTime);

        // 대결 이벤트 트리거 조건 확인
        if (LimitTime <= eventTriggerTime)
        {
            TriggerEvent();
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
    else
    {
        Hide();
    }
}


    private void Hide()
    {
        incfameTxt.text = "증가한 명성: " + FameNum.ToString();
        StatusChanger.UpdateBandFame(FameNum); // 게임 로직에 맞게 수정 필요
        jjirasiImage.gameObject.SetActive(false);

        clickTxt.gameObject.SetActive(false);
        fameTxt.gameObject.SetActive(false);
        text_Timer.gameObject.SetActive(false);
        Jjirasi.gameObject.SetActive(false);
        Main.gameObject.SetActive(true);
        incfameTxt.gameObject.SetActive(true);
    }
}
