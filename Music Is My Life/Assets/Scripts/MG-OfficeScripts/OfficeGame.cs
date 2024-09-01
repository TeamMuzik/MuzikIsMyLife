using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class OfficeGame : MonoBehaviour
{
    public TMP_InputField WordInputField;
    public TMP_Text WordInputFieldText;
    public TMP_Text timer;
    public TMP_Text scoreText;
    public TMP_Text resultText;
    public TMP_Text stressText;
    public TMP_Text finalText;
    public GameObject EndPanel, StartPanel, TutorialPanel;
    public GameObject pBlockText;
    public Transform BlockParent;
    public GameObject[] blockers; // 방해 요소 배열
    public Button StartButton;

    private GameObject canvasGameObject; // Added declaration here
    private TextAsset textAsset;
    private List<string> wordList = new List<string>();
    private List<GameObject> blockTextList = new List<GameObject>();
    private float gameTimer = 60.0f;
    private int score = 0;
    private bool gameEnded = false;
    private float blockerInterval = 15f; // 방해 요소 활성화 간격
    private HashSet<string> activeWords = new HashSet<string>(); // 사용 중인 단어 집합
    private int correctWordCount = 0;

    private AudioSource audioSource;
    public AudioClip successSound;

    private static int playCount = 0; // 플레이 횟수 변수
    private static int highScore = 0; // 최고 점수 변수

    private bool isGameStarted = false;

    private int fortuneId; //오늘의 운세
    private string isFortune; //운세 적용 여부

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        TutorialPanel.SetActive(true);
        StartPanel.SetActive(false);
        EndPanel.SetActive(false);
        canvasGameObject = GameObject.Find("Canvas"); // Initialize canvasGameObject
        if (!canvasGameObject)
        {
            Debug.LogError("Canvas GameObject not found!");
        }
        WordInputField.ActivateInputField();
        canvasGameObject = GameObject.Find("Canvas");
        if (!canvasGameObject)
        {
            Debug.LogError("Canvas GameObject not found!");
        }

        textAsset = Resources.Load<TextAsset>("OfficeText");
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');
            foreach (string line in lines)
            {
                wordList.Add(line.Trim());
            }
        }

        playCount = PlayerPrefs.GetInt("OfficeGamePlayCount"); // 플레이 횟수 불러오기
        playCount++; // 플레이 횟수 증가
        PlayerPrefs.SetInt("OfficeGamePlayCount", playCount); // 플레이 횟수 저장
        PlayerPrefs.Save();
        Debug.Log("Current playCount: " + playCount);

        highScore = PlayerPrefs.GetInt("OfficeGameHighScore", 0); // 최고 점수 불러오기

        WordInputField.onEndEdit.AddListener(delegate { GetInputFieldText(); });

        //오늘의 운세 가져오기
        //fortuneId = DayFortune.GetTodayFortuneId();
        fortuneId = 7;
        Debug.Log("운세번호: " + fortuneId);

        isFortune = "";
    }

    public void InitializeGame()
    {
        // 게임 초기화 및 설정
        StartPanel.SetActive(true);
        EndPanel.SetActive(false);
        gameEnded = false;
        gameTimer = 60.0f;
        score = 0;
        scoreText.text = "점수: " + score;

        // 초기에 모든 블록커를 비활성화
        foreach (GameObject blocker in blockers)
        {
            blocker.SetActive(false);
        }

        // 입력 필드 활성화
        WordInputField.ActivateInputField();

        StartCoroutine(CreateBlockTextRoutine());
        StartCoroutine(ActivateBlockersRoutine());
        InvokeRepeating("UpdateGameTimer", 1f, 1f);

        timer.text = "남은 시간: 01:00";

        // 나머지 게임 시작 로직
    }

    IEnumerator CreateBlockTextRoutine()
    {
        float nextLongWordTime = 0f; // 긴 단어 생성을 위한 타이머
        while (!gameEnded)
        {
            if (wordList.Count > 0)
            {
                if (fortuneId == 7)
                {
                    // 긴 단어 생성 간격을 4초로 줄임
                    if (Time.time >= nextLongWordTime)
                    {
                        CreateBlockText(true); // 긴 단어 생성
                        nextLongWordTime = Time.time + 2f; // 다음 긴 단어 생성 시간 갱신
                    }
                    else
                    {
                        CreateBlockText(false); // 짧은 단어 생성
                    }
                }
                else
                {
                    // 원래 긴 단어 생성 간격 6초
                    if (Time.time >= nextLongWordTime)
                    {
                        CreateBlockText(true); // 긴 단어 생성
                        nextLongWordTime = Time.time + 6f; // 다음 긴 단어 생성 시간 갱신
                    }
                    else
                    {
                        CreateBlockText(false); // 짧은 단어 생성
                    }
                }
                yield return new WaitForSeconds(1.2f); // 항상 1초 대기 (짧은 단어 대기 시간)
            }
        }
    }

    void CreateBlockText(bool isLongWord)
    {
        string selectedWord = GetUniqueWord(isLongWord);
        if (string.IsNullOrEmpty(selectedWord))
        {
            return; // 유니크한 단어를 찾지 못한 경우 생성 중단
        }

        GameObject block = Instantiate(pBlockText, BlockParent);
        TMP_Text textComponent = block.GetComponentInChildren<TMP_Text>();
        textComponent.text = selectedWord;

        textComponent.color = isLongWord ? Color.red : Color.black;

        RectTransform rectTransform = block.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(-250.0f, 200.0f), 310.0f);

        blockTextList.Add(block);
        StartCoroutine(MoveTextDown(rectTransform, block, isLongWord));
    }

    string GetUniqueWord(bool isLongWord)
    {
        List<string> possibleWords = wordList.FindAll(word => isLongWord ? word.Length > 5 : word.Length <= 5 && !activeWords.Contains(word));
        if (possibleWords.Count == 0)
        {
            return null; // 가능한 유니크한 단어가 없는 경우
        }

        string selectedWord = possibleWords[UnityEngine.Random.Range(0, possibleWords.Count)];
        activeWords.Add(selectedWord); // 선택된 단어를 활성화된 단어 목록에 추가
        return selectedWord;
    }

    IEnumerator MoveTextDown(RectTransform rectTransform, GameObject block, bool isHardWord)
    {
        float speed = isHardWord ? 60f : 80f; // 긴 단어의 경우 속도를 60f로, 짧은 단어의 경우 속도를 80f로 설정
        while (rectTransform != null && rectTransform.anchoredPosition.y > -309f && !gameEnded)
        {
            // Check if the GameObject still exists before accessing it
            if (block != null)
            {
                rectTransform.anchoredPosition += new Vector2(0, -speed * Time.deltaTime);
            }
            else
            {
                // Exit the loop if the block has been destroyed
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        if (!gameEnded && block != null)
        {
            gameTimer -= isHardWord ? 7 : 5;
            StartCoroutine(FlashTimer(1));

            DestroyBlock(block);
        }
    }

    IEnumerator FlashTimer(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            timer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            timer.color = Color.white;
            yield return new WaitForSeconds(0.2f);

            elapsed += 1f;
        }
    }

    IEnumerator ActivateBlockersRoutine()
    {
        while (!gameEnded)
        {
            yield return new WaitForSeconds(blockerInterval);
            ActivateBlockers();
        }
    }

    void ActivateBlockers()
    {
        // 활성화할 블록커의 개수를 1에서 5 사이로 랜덤하게 결정
        int blockersToActivate = UnityEngine.Random.Range(1, 6);

        // 이미 선택된 블록커를 추적하기 위한 리스트
        List<int> activatedBlockersIndexes = new List<int>();

        // 랜덤하게 선택된 블록커를 활성화
        for (int i = 0; i < blockersToActivate; i++)
        {
            int index = UnityEngine.Random.Range(0, blockers.Length);
            // 이미 활성화된 블록커를 피하기 위한 체크
            while (activatedBlockersIndexes.Contains(index))
            {
                index = UnityEngine.Random.Range(0, blockers.Length);
            }

            // 100% 확률로 해당 블록커 활성화
            if (UnityEngine.Random.Range(0, 100) < 100)
            {
                blockers[index].SetActive(true);
                StartCoroutine(DeactivateBlockerAfterTime(blockers[index], 5f));
                activatedBlockersIndexes.Add(index); // 활성화된 블록커 인덱스를 기록
            }
        }
    }

    IEnumerator DeactivateBlockerAfterTime(GameObject blocker, float delay)
    {
        yield return new WaitForSeconds(delay);
        blocker.SetActive(false);
    }

    public void GetInputFieldText()
    {
        string inputText = WordInputField.text.ToUpper().Trim();
        Debug.Log($"Input Received: {inputText}"); // 입력 받은 데이터 로그 출력
        CheckInputAgainstBlocks(inputText);
        WordInputField.text = ""; // 입력 필드 초기화
        WordInputField.ActivateInputField();
    }

    void CheckInputAgainstBlocks(string input)
    {
        foreach (GameObject block in new List<GameObject>(blockTextList))
        {
            TMP_Text textComponent = block.GetComponentInChildren<TMP_Text>();
            if (input.Equals(textComponent.text.ToUpper().Trim()))
            {
                bool isLongWord = textComponent.text.Length > 5;
                score += isLongWord ? 15 : 5;
                correctWordCount++;
                scoreText.text = "점수: " + score;

                blockTextList.Remove(block);
                Destroy(block);
                PlaySuccessSound();
                return; // 일치하는 첫 번째 블록을 찾으면 루프 종료
            }
        }
    }

    void DestroyBlock(GameObject block)
    {
        TMP_Text textComponent = block.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            activeWords.Remove(textComponent.text); // 블록 제거 시 활성화된 단어 목록에서 제거
        }

        blockTextList.Remove(block);
        Destroy(block);
    }

    void UpdateGameTimer()
    {
        if (!gameEnded)
        {
            gameTimer -= 1; // 매 호출마다 1초 감소
            if (gameTimer <= 0)
            {
                gameTimer = 0;
                gameEnded = true;
                EndGame();
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(gameTimer);
            timer.text = string.Format("남은 시간: {0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }

    void Update()
    {
        if (!gameEnded && isGameStarted) // 게임이 시작되었고, 아직 종료되지 않았을 때 타이머 업데이트
        {
            UpdateGameTimer();
        }
    }

    void EndGame()
    {
        EndPanel.SetActive(true);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("OfficeGameHighScore", highScore); // 최고 점수 저장
            PlayerPrefs.Save();
        }

        Debug.Log("Current High Score: " + highScore);
        StopAllCoroutines();
        foreach (GameObject blocker in blockers)
        {
            blocker.SetActive(false);
        }
        // 알바 결과 매핑
        (string resultRes, string stressRes) = MGResultManager.PartTimeDayResult(1);
        resultText.text = resultRes;

        finalText.gameObject.SetActive(true);
        int totalScore = score;
        int earnedMoney = totalScore / 10;

        //오늘의 운세 1번이면 번 돈 5 증가
        if (fortuneId == 1 || fortuneId == 7)
            isFortune = "(운세적용)";

        if (fortuneId == 1)
            earnedMoney += 5;

        stressText.text = stressRes + "\n" + isFortune;

        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + earnedMoney);
        StatusController statusController = FindObjectOfType<StatusController>();

        finalText.text = correctWordCount + "개의 단어를 입력했다." + "\n번 돈: " + earnedMoney + "만원";
    }

    void PlaySuccessSound()
    {
        audioSource.clip = successSound;
        audioSource.Play();
    }
}
