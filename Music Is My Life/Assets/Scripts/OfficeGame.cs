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

     private bool isGameStarted = false;

    void Start()
    {   TutorialPanel.SetActive(true);
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

        WordInputField.onEndEdit.AddListener(delegate { GetInputFieldText(); });
         

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
        // 현재 시간이 다음 긴 단어 생성 시간을 넘었는지 확인
        if (Time.time >= nextLongWordTime)
        {
            CreateBlockText(true); // 긴 단어 생성
            nextLongWordTime = Time.time + 6f; // 다음 긴 단어 생성 시간 갱신
        }
        else
        {
            CreateBlockText(false); // 짧은 단어 생성
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
rectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(-250.0f, 200.0f), 300.0f);

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
        float speed = 120f; // Adjusted speed
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
            StartCoroutine(FlashTimer(2));

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

          // 20% 확률로 해당 블록커 활성화
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
            score += isLongWord ? 10 : 5;
            scoreText.text = "점수: " + score;

            blockTextList.Remove(block);
            Destroy(block);
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
