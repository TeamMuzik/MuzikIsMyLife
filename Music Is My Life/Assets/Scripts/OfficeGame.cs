using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class OfficeGame : MonoBehaviour
{
    public TMP_InputField WordInputField;
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

    private GameObject canvasGameObject;
    private TextAsset textAsset;
    private List<string> wordList = new List<string>();
    private List<GameObject> blockTextList = new List<GameObject>();
    private float gameTimer = 60.0f;
    private int score = 0;
    private bool gameEnded = false;
    private float blockerInterval = 15f;
    private HashSet<string> activeWords = new HashSet<string>();
    private int correctWordCount = 0;

    private AudioSource audioSource;
    public AudioClip successSound;

    private bool isGameStarted = false;

    private float keyboardHeight = 0;
    private bool isKeyboardVisible = false;

    private int maxConcurrentWords = 3; // 동시에 뜰 수 있는 최대 단어 개수
    private Queue<string> wordQueue = new Queue<string>(); // 떠있는 단어를 관리하기 위한 큐

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait; // 세로로 변환

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found!");
        }

        TutorialPanel.SetActive(true);
        StartPanel.SetActive(false);
        EndPanel.SetActive(false);
        canvasGameObject = GameObject.Find("Canvas");
        if (!canvasGameObject)
        {
            Debug.LogError("Canvas GameObject not found!");
        }

        WordInputField.ActivateInputField();

        textAsset = Resources.Load<TextAsset>("OfficeText");
        if (textAsset == null)
        {
            Debug.LogError("Text asset not found!");
        }
        else
        {
            string[] lines = textAsset.text.Split('\n');
            foreach (string line in lines)
            {
                wordList.Add(line.Trim());
            }
        }

        WordInputField.onEndEdit.AddListener(delegate { GetInputFieldText(); WordInputField.ActivateInputField(); });
        WordInputField.onSelect.AddListener(delegate { OnInputFieldSelect(); });
        WordInputField.onDeselect.AddListener(delegate { OnInputFieldDeselect(); });

        // 초기화 시 모든 blockers 비활성화
        foreach (GameObject blocker in blockers)
        {
            if (blocker != null)
            {
                blocker.SetActive(false);
            }
        }

        // 게임 시작 시 랜덤하게 단어 생성
        for (int i = 0; i < maxConcurrentWords; i++)
        {
            CreateRandomWord();
        }


    }

    IEnumerator KeepKeyboardActive()
    {
        while (!gameEnded)
        {
            if (TouchScreenKeyboard.visible == false)
            {
                ShowKeyboard();
                Debug.Log("Keyboard activated"); // 키보드 활성화 시 콘솔 출력
            }
            yield return new WaitForSeconds(0.5f); // 주기적으로 키보드가 활성화되어 있는지 체크
        }
    }

    void ShowKeyboard()
    {
        WordInputField.ActivateInputField();
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "", 0);
    }

    void OnInputFieldSelect()
    {
        if (canvasGameObject != null)
        {
            RectTransform rt = canvasGameObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, keyboardHeight); // 필요에 따라 값을 조정
        }
        ShowKeyboard();
    }

    void OnInputFieldDeselect()
    {
        if (canvasGameObject != null)
        {
            RectTransform rt = canvasGameObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, 0); // 필요에 따라 값을 조정
        }
    }

    public void InitializeGame()
    {     // 게임 시작 시 키보드 활성화
        StartCoroutine(KeepKeyboardActive());
        StartPanel.SetActive(true);
        EndPanel.SetActive(false);
        gameEnded = false;
        gameTimer = 60.0f;
        score = 0;
        scoreText.text = "점수: " + score;

        foreach (GameObject blocker in blockers)
        {
            if (blocker != null)
            {
                blocker.SetActive(false);
            }
        }

        WordInputField.ActivateInputField();

        StartCoroutine(ActivateBlockersRoutine());
        InvokeRepeating("UpdateGameTimer", 1f, 1f);

        timer.text = "남은 시간: 01:00";

        // 단어 생성 초기화
        for (int i = 0; i < maxConcurrentWords; i++)
        {
            CreateRandomWord();
        }

        // 게임 시작 시 키보드 활성화
        StartCoroutine(KeepKeyboardActive());
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
        // 화면에 이미 존재하는 블로커 개수 세기
        int activeBlockers = 0;
        foreach (GameObject blocker in blockers)
        {
            if (blocker.activeSelf) activeBlockers++;
        }

        // 최대 3개의 블로커만 활성화
        int blockersToActivate = Mathf.Min(UnityEngine.Random.Range(1, blockers.Length + 1), 3 - activeBlockers);

        List<int> activatedBlockersIndexes = new List<int>();

        for (int i = 0; i < blockersToActivate; i++)
        {
            int index = UnityEngine.Random.Range(0, blockers.Length);
            while (activatedBlockersIndexes.Contains(index) || blockers[index].activeSelf)
            {
                index = UnityEngine.Random.Range(0, blockers.Length);
            }

            blockers[index].SetActive(true);
            StartCoroutine(DeactivateBlockerAfterTime(blockers[index], UnityEngine.Random.Range(3f, 7f))); // 랜덤 시간 내에 비활성화
            activatedBlockersIndexes.Add(index);
        }
    }

    IEnumerator DeactivateBlockerAfterTime(GameObject blocker, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (blocker != null)
        {
            blocker.SetActive(false);
        }
    }

    public void GetInputFieldText()
    {
        string inputText = WordInputField.text.ToUpper().Trim();
        Debug.Log($"Input Received: {inputText}");
        CheckInputAgainstBlocks(inputText);
        WordInputField.text = "";
        WordInputField.ActivateInputField();
    }

    void CheckInputAgainstBlocks(string input)
    {
        bool foundWord = false;
        foreach (GameObject block in new List<GameObject>(blockTextList))
        {
            TMP_Text textComponent = block.GetComponentInChildren<TMP_Text>();
            if (input.Equals(textComponent.text.ToUpper().Trim()))
            {
                foundWord = true;
                bool isLongWord = textComponent.text.Length > 5;
                score += isLongWord ? 15 : 5;
                correctWordCount++;
                scoreText.text = "점수: " + score;

                blockTextList.Remove(block);
                Destroy(block);
                PlaySuccessSound();
                if (gameObject.activeInHierarchy) // 코루틴 실행 전 활성화 상태인지 확인
                {
                    StartCoroutine(CreateNewWordWithDelay());
                }
                break;
            }
        }

        if (!foundWord)
        {
            // 오답을 입력해도 시간은 깎지 않음
            if (gameObject.activeInHierarchy) // 코루틴 실행 전 활성화 상태인지 확인
            {
                StartCoroutine(CreateNewWordWithDelay());
            }
        }
    }

    IEnumerator CreateNewWordWithDelay()
    {
        yield return new WaitForSeconds(1f);
        if (!gameEnded && blockTextList.Count < maxConcurrentWords)
        {
            CreateRandomWord();
        }
    }

    void CreateRandomWord()
    {
        if (blockTextList.Count >= maxConcurrentWords)
        {
            return;
        }

        string selectedWord = GetUniqueWord(UnityEngine.Random.Range(0, 2) == 0);
        if (string.IsNullOrEmpty(selectedWord))
        {
            return;
        }

        GameObject block = Instantiate(pBlockText, BlockParent);
        TMP_Text textComponent = block.GetComponentInChildren<TMP_Text>();
        textComponent.text = selectedWord;

        // 긴 단어일 경우 빨간색, 그렇지 않은 경우 검정색으로 설정
        textComponent.color = selectedWord.Length > 5 ? Color.red : Color.black;

        RectTransform startPanelRectTransform = StartPanel.GetComponent<RectTransform>();
        RectTransform rectTransform = block.GetComponent<RectTransform>();

        float startPanelWidth = startPanelRectTransform.rect.width;
        float startPanelHeight = startPanelRectTransform.rect.height;

        // 단어들 사이의 거리를 넓히기 위해 추가적인 범위 설정 및 단어가 패널 밖으로 나가지 않도록 조정
        bool isPositionValid;
        float xPos, yPos;

        do
        {
            isPositionValid = true;
            xPos = UnityEngine.Random.Range(-startPanelWidth / 2 + rectTransform.rect.width / 2, startPanelWidth / 2 - rectTransform.rect.width / 2);
            yPos = UnityEngine.Random.Range(-startPanelHeight / 2 + rectTransform.rect.height / 2 + 400, startPanelHeight / 2 - rectTransform.rect.height / 2 - 400); // 추가적인 여유 공간을 둠

            // Check if the new position overlaps with any existing blocks
            foreach (GameObject existingBlock in blockTextList)
            {
                if (existingBlock == null)
                {
                    continue;
                }

                RectTransform existingRectTransform = existingBlock.GetComponent<RectTransform>();
                if (Mathf.Abs(yPos - existingRectTransform.anchoredPosition.y) < (rectTransform.rect.height + 50)) // y 좌표 간 최소 간격 설정
                {
                    isPositionValid = false;
                    break;
                }
            }
        } while (!isPositionValid);

        rectTransform.anchoredPosition = new Vector2(xPos, yPos);

        blockTextList.Add(block);
        wordQueue.Enqueue(selectedWord);

        if (gameObject.activeInHierarchy) // 코루틴 실행 전 활성화 상태인지 확인
        {
            StartCoroutine(FadeOutAndDestroy(rectTransform, block, selectedWord.Length > 5));
        }
    }

    string GetUniqueWord(bool isLongWord)
    {
        List<string> possibleWords = wordList.FindAll(word => isLongWord ? word.Length > 5 : word.Length <= 5 && !activeWords.Contains(word));
        if (possibleWords.Count == 0)
        {
            return null;
        }

        string selectedWord = possibleWords[UnityEngine.Random.Range(0, possibleWords.Count)];
        activeWords.Add(selectedWord);
        return selectedWord;
    }

    IEnumerator FadeOutAndDestroy(RectTransform rectTransform, GameObject block, bool isHardWord)
    {
        float duration = 10f;
        float elapsedTime = 0f;

        TMP_Text textComponent = block.GetComponentInChildren<TMP_Text>();
        Color originalColor = textComponent.color;

        while (!gameEnded && elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        if (!gameEnded && block != null)
        {
            DestroyBlock(block);
            gameTimer -= 3f; // 타이머에서 3초를 깎음
            if (gameTimer < 0) gameTimer = 0; // 타이머가 음수가 되지 않도록 조정
            timer.text = string.Format("남은 시간: {0:D2}:{1:D2}", (int)gameTimer / 60, (int)gameTimer % 60);

            // FlashTimer 호출
            StartCoroutine(FlashTimer(1));

            if (gameObject.activeInHierarchy) // 코루틴 실행 전 활성화 상태인지 확인
            {
                StartCoroutine(CreateNewWordWithDelay());
            }
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

            elapsed += 0.4f;
        }
    }

    void DestroyBlock(GameObject block)
    {
        if (block == null) return;

        TMP_Text textComponent = block.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            activeWords.Remove(textComponent.text);
        }

        blockTextList.Remove(block);
        Destroy(block);
    }

    void UpdateGameTimer()
    {
        if (!gameEnded)
        {
            gameTimer -= 1;
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
        if (!gameEnded && isGameStarted)
        {
            UpdateGameTimer();

            if (TouchScreenKeyboard.visible)
            {
                if (!isKeyboardVisible)
                {
                    isKeyboardVisible = true;
                    keyboardHeight = TouchScreenKeyboard.area.height;
                    MoveUIUp();
                }
            }
            else
            {
                if (isKeyboardVisible)
                {
                    isKeyboardVisible = false;
                    MoveUIDown();

                }
            }
        }
    }

    void MoveUIUp()
    {
        if (canvasGameObject != null)
        {
            RectTransform rt = canvasGameObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, keyboardHeight);
        }
    }

    void MoveUIDown()
    {
        if (canvasGameObject != null)
        {
            RectTransform rt = canvasGameObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, 0);
        }
    }

    void EndGame()
{
    EndPanel.SetActive(true);

    StopAllCoroutines();
    foreach (GameObject blocker in blockers)
    {
        if (blocker != null)
        {
            blocker.SetActive(false);
        }
    }

    // 게임 결과 처리
    (string resultRes, string stressRes) = MGResultManager.PartTimeDayResult(1);
    resultText.text = resultRes;
    stressText.text = stressRes;

    finalText.gameObject.SetActive(true);
    int totalScore = score;
    int earnedMoney = totalScore / 10;

    PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + earnedMoney);
    StatusController statusController = FindObjectOfType<StatusController>();

    finalText.text = correctWordCount + "개의 단어를 입력했다." + "\n번 돈: " + earnedMoney + "만원";

    // 입력 필드 비활성화 및 키보드 숨기기
    WordInputField.DeactivateInputField();
    TouchScreenKeyboard.hideInput = true;

    // Debug: 게임 종료 시 콘솔에 키보드 비활성화 메시지 출력
    Debug.Log("Keyboard deactivated at end of game");

    // 키보드가 사라지도록 입력 필드를 비활성화한 후 다시 활성화
    TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "", 0).active = false;
}

    void PlaySuccessSound()
    {
        if (audioSource != null && successSound != null)
        {
            audioSource.clip = successSound;
            audioSource.Play();
        }
    }
}
