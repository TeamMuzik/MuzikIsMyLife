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
    public TMP_Text resultText;
    public TMP_Text stressText;
    public GameObject ScorePanel;
    public TMP_Text duelTimerText;
    public GameObject duelText;

    public Button Jjirasi;

    public GameObject jjirasi;
    private bool isClicked = false;
    public Image jjirasiImage;
    public Sprite normalSprite;
    public Sprite clickSprite;
    public GameObject DuelPanel;
    public GameObject DuelImageContainer;

    private float eventTriggerTime;
    private bool eventTriggered = false;
    public GameObject DuelimagePrefab;
    public Transform imagesParent;
    public TMP_Text startMessageText;
    public bool isGameStarted = false;

    public GameObject eventUI;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    public GameObject[] villains;
    public Slider duelSlider;
    public float systemIncreaseRate = 2000.0f;
    private bool duelInProgress = false;
    private bool duelStarted = false;
    private bool isDuelWon = false;
    private float duelTimeLimit = 5.0f;
    private bool duelTimerActive = false;
    private float remainingDuelTime;

    public AudioClip duelSound1;
    public AudioClip duelSound2;
    public AudioClip duelSound3;
    public AudioClip VsSound;
    public AudioClip SuccessSound;

    private AudioSource audioSource;

    private static int highScore = 0;
    private static int playCount = 0; //플레이 횟수

    private int fortuneId;
    private string isFortune;

    // Variables for duel difficulty based on fortune
    public float easyDuelIncrement = 0.06f;
    public float hardDuelIncrement = 0.02f;
    private float duelIncrement;

    private bool villainIndex;

    //주인공 및 찌라시 버튼 위치 변수
    private RectTransform charRect;
    private RectTransform buttonRect;

    //빌런 대사
    public TMP_Text vilainTxt;

    public GameObject howToPlay;
    public GameObject ready;
    public GameObject start;

    void Start()
    {
        ClickNum = 0;
        FameNum = 0;
        clickTxt.text = "클릭 수 : 0";
        fameTxt.text = "명성 : 0";
        text_Timer.text = "남은 시간 : 30";
        duelTimerText.text = "";

        LimitTime = 30;
        incfameTxt.gameObject.SetActive(false);

        jjirasiImage.sprite = normalSprite;

        DuelPanel.SetActive(false);
        ScorePanel.SetActive(false);
        DuelimagePrefab.SetActive(false);
        duelText.gameObject.SetActive(false);
        howToPlay.SetActive(false);
        Jjirasi.interactable = false; //찌라시 클릭 버튼 처음에는 비활성화
        //PrepareGameStart();

        audioSource = GetComponent<AudioSource>();
        highScore = PlayerPrefs.GetInt("JjirasiGameHighScore", 0);

        playCount = PlayerPrefs.GetInt("JjirasiGamePlayCount");
        playCount++;
        PlayerPrefs.SetInt("JjirasiGamePlayCount", playCount);
        PlayerPrefs.Save();
        Debug.Log("Current playCount: " + playCount);

        // Fetch and set fortune, and adjust duel difficulty accordingly
        fortuneId = DayFortune.GetTodayFortuneId();
        Debug.Log("운세번호: " + fortuneId);


        if (fortuneId == 3)
        {
            duelIncrement = easyDuelIncrement;  // Easier duel
        }
        else if (fortuneId == 8)
        {
            duelIncrement = hardDuelIncrement;  // Harder duel
        }
        else
        {
            duelIncrement = 0.04f;  // Default increment
        }
        isFortune = "";

        foreach (GameObject v in villains)
        {
            v.SetActive(false);
        }

        //주인공 이미지 위치
        charRect = jjirasi.GetComponent<RectTransform>();

        //찌라시 버튼 위치
        buttonRect = Jjirasi.GetComponent<RectTransform>();

        // 처음 실행할 때는 게임 방법이 나오게
        if (playCount == 1)
        {
            howToPlay.SetActive(true);
        }
        else
        {
            Tutorial();
        }

    }

    public void Tutorial()
    {
        StartCoroutine(ReadyStart());
    }

    public IEnumerator ReadyStart()
    {
        ready.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        start.SetActive(true);
        yield return new WaitForSeconds(1f);

        PrepareGameStart();
        Jjirasi.interactable = true;
        ready.SetActive(false);
        start.SetActive(false);
    }

    void StartGameplay()
    {
        isGameStarted = true;
        startMessageText.gameObject.SetActive(false);
        UpdateEventTriggerTime();

        LimitTime = 30;
    }

    void PrepareGameStart()
    {
        startMessageText.text = "시작하려면 주인공 클릭...";
        startMessageText.gameObject.SetActive(true);
    }

    private void UpdateEventTriggerTime()
    {
        eventTriggerTime = Random.Range(10, 20);
        Debug.Log("Event Trigger Time: " + eventTriggerTime);
    }

    private void TriggerEvent()
    {
        if (!duelStarted)
        {
            StartCoroutine(StartDuelSequence());
            duelStarted = true;
        }
    }

    IEnumerator StartDuelSequence()
    {
        eventTriggered = true;

        DuelimagePrefab.gameObject.SetActive(true);

        GameObject image1 = InstantiateImage(sprite1);
        audioSource.clip = duelSound1;
        audioSource.Play();
        yield return new WaitForSeconds(1.0f);

        GameObject image2 = InstantiateImage(sprite2);
        audioSource.clip = duelSound2;
        audioSource.Play();
        yield return new WaitForSeconds(1.0f);

        GameObject image3 = InstantiateImage(sprite3);
        audioSource.clip = VsSound;
        audioSource.Play();
        yield return new WaitForSeconds(1.0f);

        duelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);

        Destroy(image1);
        Destroy(image2);
        Destroy(image3);
        DuelimagePrefab.gameObject.SetActive(false);
        duelText.gameObject.SetActive(false);

        villainIndex = true;
        StartDuel();
    }

    void StartDuel()
    {
        audioSource.clip = duelSound3;
        audioSource.Play();
        duelTimerActive = true;
        remainingDuelTime = duelTimeLimit;
        Jjirasi.gameObject.SetActive(true);
        jjirasiImage.gameObject.SetActive(true);
        duelInProgress = true;
        eventUI.SetActive(true);
        duelSlider.gameObject.SetActive(true);

        clickTxt.gameObject.SetActive(false);
        fameTxt.gameObject.SetActive(false);
        text_Timer.gameObject.SetActive(false);
        DuelPanel.SetActive(true);
        villains[0].SetActive(true); //첫번쨰 빌런 이미지만 활성화
        duelSlider.value = 0.5f;
        StartCoroutine(DuelTimer());

        //주인공 위치 변경
        charRect.anchoredPosition = new Vector2(-240, -180);

        //찌라시 버튼 위치 변경
        buttonRect.anchoredPosition = new Vector2(-180, -110);

        //빌런 대사
        villainDialogue();

    }

    IEnumerator DuelTimer()
    {
        while (duelTimerActive && remainingDuelTime > 0)
        {
            duelTimerText.text = "대결 시간: " + remainingDuelTime.ToString("F0") + "초";
            yield return new WaitForSeconds(1.0f);
            remainingDuelTime -= 1.0f;
            if (remainingDuelTime <= 0)
            {
                duelTimerText.text = "";
                EndDuel(false);
            }
        }
    }

    GameObject InstantiateImage(Sprite sprite)
    {
        GameObject newImageObj = Instantiate(DuelimagePrefab, DuelImageContainer.transform);
        newImageObj.GetComponent<Image>().sprite = sprite;
        newImageObj.SetActive(true);
        return newImageObj;
    }

    private void UpdateFame()
    {
        int baseFame = ClickNum / 10;
        totalFame = baseFame + FameNum;
        fameTxt.text = "명성 : " + totalFame.ToString();
        Debug.Log("Total Fame: " + totalFame);
    }

    public void Click()
    {
        if (!isGameStarted)
        {
            StartGameplay();
        }
        else if (duelInProgress)
        {
            duelSlider.value += duelIncrement;  // Adjusted for fortune
            villainIndex = !villainIndex;
            if (villainIndex)
            {
                villains[0].SetActive(true);
                villains[1].SetActive(false);
            }
            else
            {
                villains[1].SetActive(true);
                villains[0].SetActive(false);
            }

        }
        else
        {
            ClickNum++;
            clickTxt.text = "클릭 수 : " + ClickNum.ToString();
            Debug.Log("ClickNum: " + ClickNum);
            PlayerPrefs.SetInt("JjirasiClick", PlayerPrefs.GetInt("JjirasiClick") + 1);
            UpdateFame();
        }

        if (!isClicked)
        {
            jjirasiImage.sprite = clickSprite;
            isClicked = true;
            StartCoroutine(ResetSpriteAfterDelay(0.1f));
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
        audioSource.Stop();
        duelInProgress = false;
        duelTimerActive = false;
        eventUI.SetActive(false);
        duelSlider.gameObject.SetActive(false);
        DuelPanel.gameObject.SetActive(false);

        if (won)
        {
            FameNum += 10;
            UpdateFame();
            StartCoroutine(ShowFameIncrease());
            PlaySound();
        }

        eventTriggered = false;
        duelInProgress = false;

        LimitTime = Mathf.Max(LimitTime, 0);

        clickTxt.gameObject.SetActive(true);
        fameTxt.gameObject.SetActive(true);
        text_Timer.gameObject.SetActive(true);
        Jjirasi.gameObject.SetActive(true);

        //주인공 원위치
        charRect.anchoredPosition = new Vector2(-27.93f, -180.48f);
        //버튼 원위치
        buttonRect.anchoredPosition = new Vector2(-6.58f, -113.94f);
        //빌런 대사 초기화
        vilainTxt.text = null;

    }

    IEnumerator ShowFameIncrease()
    {
        fameTxt.color = Color.blue;
        for (int i = 0; i < 4; i++)
        {
            fameTxt.text = "명성 : " + totalFame.ToString() + "\n\n(+10)";
            yield return new WaitForSeconds(0.5f);
            fameTxt.text = "명성 : " + totalFame.ToString();
            yield return new WaitForSeconds(0.5f);
        }
        fameTxt.color = Color.black;
        UpdateFame();
    }

    void Update()
    {
        if (LimitTime > 0 && !eventTriggered && isGameStarted)
        {
            LimitTime -= Time.deltaTime;
            text_Timer.text = "남은 시간 : " + Mathf.Round(LimitTime);

            if (LimitTime > 0 && !eventTriggered && LimitTime <= eventTriggerTime)
            {
                TriggerEvent();
            }
        }
        else if (eventTriggered && duelInProgress)
        {
            duelSlider.value -= systemIncreaseRate * Time.deltaTime;
            if (duelSlider.value <= 0)
            {
                EndDuel(false);
            }
            else if (duelSlider.value >= 0.99)
            {
                EndDuel(true);
                isDuelWon = true;
            }
        }
        else if (!duelInProgress)
        {
            duelInProgress = false;

            LimitTime = Mathf.Max(LimitTime, 0);

            clickTxt.gameObject.SetActive(true);
            fameTxt.gameObject.SetActive(true);
            text_Timer.gameObject.SetActive(true);
            Jjirasi.gameObject.SetActive(true);
        }
        if (LimitTime <= 0 && !duelInProgress)
        {
            Hide();
            if (ClickNum > highScore)
            {
                highScore = ClickNum;
                PlayerPrefs.SetInt("JjirasiGameHighScore", highScore);
                PlayerPrefs.Save();
            }
            Debug.Log("Current High Score: " + highScore);
        }
    }

    void Hide()
    {
        ScorePanel.SetActive(true);
        StatusChanger.UpdateBandFame(totalFame);

        incfameTxt.text = "클릭 수 : " + ClickNum.ToString() +
        "\n\n야옹의 명성 : " + PlayerPrefs.GetInt("BandFame") + " (+" + totalFame.ToString() + ")";
        (string resultRes, string stressRes) = MGResultManager.JjirasiDayResult(isDuelWon);
        resultText.text = resultRes;
        if (fortuneId == 3 || fortuneId == 8)
            isFortune = "(운세적용)";

        stressText.text = stressRes + "\n" + isFortune;

        jjirasiImage.gameObject.SetActive(false);

        clickTxt.gameObject.SetActive(false);
        fameTxt.gameObject.SetActive(false);
        text_Timer.gameObject.SetActive(false);
        Jjirasi.gameObject.SetActive(false);

        incfameTxt.gameObject.SetActive(true);
    }

    void PlaySound()
    {
        audioSource.clip = SuccessSound;
        audioSource.Play();
    }

    void villainDialogue()
    {
        string[] Dialogue = {"악의 무리는 죗값을 치르리라!!!",
        "죄 지은 자는 고통을 받을지어다!!!",
        "내게 자비란 없다!!!",
        "이제 모든 걸 끝낼 시간이다!!!",
        "이게 전력인가? 약해 빠진 놈!!!"};
        vilainTxt.text = Dialogue[Random.Range(0, 5)];
    }
}
