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
    public GameObject EndPanel, StartPanel,TutorialPanel;
    public GameObject pBlockText;
    public Transform BlockParent;
    public GameObject[] blockers; // 방해 요소 배열

    private GameObject canvasGameObject; // Added declaration here
    private TextAsset textAsset;
    private List<string> wordList = new List<string>();
    private List<GameObject> blockTextList = new List<GameObject>();
    private float gameTimer = 60.0f;
    private int score = 0;
    private bool gameEnded = false;
    private float blockerInterval = 15f; // 방해 요소 활성화 간격
    private HashSet<string> activeWords = new HashSet<string>(); // 사용 중인 단어 집합
    private int shortWordCount = 0; // 짧은 단어 카운터
    private int longWordInterval = 5; // 긴 단어가 나타나는 간격
    private float shortWordCreationRate = 1f; // 짧은 단어 생성 속도
    private float longWordCreationRate = 4f; // 긴 단어 생성 속도

    void Start()
    {
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

        WordInputField.onEndEdit.AddListener(delegate { GetInputFieldText(); });
        EndPanel.SetActive(false);
        StartPanel.SetActive(true);
        StartCoroutine(CreateBlockTextRoutine());
        StartCoroutine(ActivateBlockersRoutine());
        InvokeRepeating("UpdateGameTimer", 1f, 1f);
        scoreText.text = "점수: 0";
        timer.text = "남은 시간: 00:00";

        // 초기에 모든 블록커를 비활성화합니다.
        foreach (GameObject blocker in blockers)
        {
            blocker.SetActive(false);
        }
    }

    IEnumerator CreateBlockTextRoutine()
{
    while (!gameEnded)
    {
        if (wordList.Count > 0)
        {
            CreateBlockText(false); // 짧은 단어 생성
            yield return new WaitForSeconds(shortWordCreationRate); // 짧은 단어 생성 속도에 따라 대기
            shortWordCount++;

            // 짧은 단어가 5개 생성되면 긴 단어 생성
            if (shortWordCount >= longWordInterval)
            {
                CreateBlockText(true); // 긴 단어 생성
                shortWordCount = 0; // 카운터 초기화
                yield return new WaitForSeconds(longWordCreationRate); // 긴 단어 생성 후 대기
            }
            else
            {
                // 짧은 단어 생성 후 일정 시간 동안 긴 단어 생성을 막습니다.
                yield return new WaitForSeconds(shortWordCreationRate);
            }
        }
    }
}


    void CreateBlockText(bool isLongWord)
    {
        string selectedWord = wordList[UnityEngine.Random.Range(0, wordList.Count)];
        GameObject block = Instantiate(pBlockText, BlockParent);
        TMP_Text textComponent = block.GetComponentInChildren<TMP_Text>();
        textComponent.text = selectedWord;

        // Set color based on word length
        textComponent.color = selectedWord.Length > 5 ? Color.red : Color.black;

        // Adjust the starting position here, for example, range for x and fixed y
        RectTransform rectTransform = block.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(-200.0f, 200.0f), 300.0f); // Example adjustment

        blockTextList.Add(block);
        StartCoroutine(MoveTextDown(rectTransform, block, selectedWord.Length > 5));
    }

    IEnumerator MoveTextDown(RectTransform rectTransform, GameObject block, bool isHardWord)
    {
        float speed = 150f; // Adjusted speed
        while (rectTransform != null && rectTransform.anchoredPosition.y > -250f && !gameEnded)
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
            DestroyBlock(block);
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
        foreach (GameObject blocker in blockers)
        {
            if (UnityEngine.Random.Range(0, 100) < 20) // 20% 확률로 활성화
            {
                blocker.SetActive(true);
                StartCoroutine(DeactivateBlockerAfterTime(blocker, 5f)); // 5초 후 비활성화
            }
        }
    }

    IEnumerator DeactivateBlockerAfterTime(GameObject blocker, float delay)
    {
        yield return new WaitForSeconds(delay);
        blocker.SetActive(false);
    }

    void GetInputFieldText()
    {
        string inputText = WordInputField.text.ToUpper().Trim();
        CheckInputAgainstBlocks(inputText);
        WordInputField.text = ""; // 입력 필드 초기화
        WordInputField.ActivateInputField();
    }

    void CheckInputAgainstBlocks(string input)
    {
        foreach (GameObject block in new List<GameObject>(blockTextList))
        {
            TMP_Text textComponent = block.GetComponentInChildren<TMP_Text>();
            if (input.Equals(textComponent.text.ToUpper()))
            {
                bool isLongWord = textComponent.text.Length > 5;
                score += isLongWord ? 10 : 5; // 긴 단어는 10점, 짧은 단어는 5점
                blockTextList.Remove(block);
                Destroy(block);
                break;
            }
        }

        scoreText.text = "점수: " + score;
    }

    void DestroyBlock(GameObject block)
    {
        blockTextList.Remove(block);
        Destroy(block);
    }

    void UpdateGameTimer()
    {
        if (!gameEnded)
        {
            gameTimer -= Time.deltaTime;
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
        if (!gameEnded)
        {
            UpdateGameTimer();
        }
    }

    void EndGame()
    {
        EndPanel.SetActive(true);
        StartPanel.SetActive(false);
        StopAllCoroutines();
        int result = MGResultManager.PartTimeDayResult();
        switch (result)
        {
            case 1:
                resultText.text = "3일 연속으로 알바를 완벽하게 성공했다!";
                stressText.text = "스트레스 -20";
                break;
            case 2:
                resultText.text = "바쁜 하루였다...";
                stressText.text = "스트레스 +20";
                break;
            default:
                resultText.text = "오늘도 열심히 알바를 했다.";
                stressText.text = "스트레스 +10";
                break;
        }
        finalText.gameObject.SetActive(true);
        int totalScore = score;
        int earnedMoney = totalScore / 10;

        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + earnedMoney);
        StatusController statusController = FindObjectOfType<StatusController>();

        finalText.text = "점수: " + score + "   얻은 돈:" + earnedMoney + "만원";
    }
}
