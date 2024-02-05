using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class OfficeGame : MonoBehaviour
{


    TextAsset textFile;
    string[] lines;
    List<string> wordList = new List<string>();

    public GameObject canvasGameObject;
    public TMP_InputField WordInputField;
    public TMP_Text WordInputFieldText;
    public TMP_Text timer;

    public GameObject pBlockText;
    public GameObject Square;
    public Transform BlockParent;
    public Button Main;
    List<GameObject> blockTextList = new List<GameObject>();
    List<string> tempList = new List<string>();

    int level = 1;
    int score = 0;
    int lives = 5;
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public TMP_Text finalText;

    Coroutine createBlockTextCoroutine;
    Coroutine checkInputFieldCoroutine;

    bool canInput = true;
    bool canDestroy = true;
    bool gameOver = false;

    float gameTimer = 60.0f;

    public GameObject[] blockers; // 스프라이트 객체들을 위한 배열
    private float nextBlockerTime = 0f;
    private float blockerDuration = 5f; // 스프라이트가 화면에 머무는 시간
    private bool blockerActive = false;

    void Start()
    {

      WordInputField.ActivateInputField();
      canvasGameObject = GameObject.Find("Canvas");
      if (canvasGameObject != null)
        {
            canvasGameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Canvas GameObject not found!");
        }

      foreach (GameObject blocker in blockers)
    {
        blocker.SetActive(false);
    }

        textFile = Resources.Load("OfficeGameText") as TextAsset;
        if (textFile != null)
        {
            lines = textFile.text.Split("\n");
            foreach (string words in lines)
            {
                string temp = words.Replace("\r", string.Empty);
                wordList.Add(temp);
            }

            WordInputField.ActivateInputField();
            createBlockTextCoroutine = StartCoroutine(CreateBlockText());
            scoreText.text = "점수 : " + score.ToString();
            InvokeRepeating("UpdateGameTimer", 1.0f, 1.0f);
            timer.text = "남은 시간: 1:00";
            Main.gameObject.SetActive(false);
            finalText.gameObject.SetActive(false);
            livesText.text = "생명: " + lives;
        }
        else
        {
            Debug.LogError("Failed to load 'OfficeGameText' file.");
        }

        WordInputField.onEndEdit.AddListener(delegate { GetInputFieldText(); });
        foreach (GameObject blocker in blockers)
   {
       blocker.SetActive(false);
   }
        SetNextBlockerTime();
    }

    void UpdateGameTimer()
    {
        gameTimer -= 1.0f;
        TimeSpan timeSpan = TimeSpan.FromSeconds(gameTimer);
        string timerText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        timer.text = "남은 시간: " + timerText;

        if (gameTimer <= 0)
        {
            StopGame();
        }
    }
    IEnumerator CreateBlockText()
    {
        while (true)
        {
            if (wordList.Count > 0)
            {
                GameObject tempBlock = Instantiate(pBlockText, BlockParent);
                TMP_Text tempTextComponent = tempBlock.GetComponentInChildren<TMP_Text>();

                RectTransform tempRectTransform = tempBlock.GetComponent<RectTransform>();
                tempRectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(-300.0f, 300.0f), 400.0f);
                tempTextComponent.text = wordList[UnityEngine.Random.Range(0, wordList.Count)];
                tempBlock.tag = "TextBlock";

                blockTextList.Add(tempBlock);

                for (int i = 0; i < wordList.Count; i++)
                {
                    if (wordList[i] == tempTextComponent.text)
                    {
                        tempList.Add(tempTextComponent.text);
                        wordList.Remove(tempTextComponent.text);
                        break;
                    }
                }

                StartCoroutine(MoveTextDown(tempRectTransform, tempBlock));

                yield return new WaitForSeconds(3.0f / level);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator MoveTextDown(RectTransform rectTransform, GameObject block)
{
  if (rectTransform == null || block == null)
   {
     Debug.LogError("MoveTextDown: rectTransform or block is null");
      yield break;
   }

    BlockBehavior blockBehavior = block.GetComponent<BlockBehavior>();
    if (blockBehavior == null)
    {
        Debug.LogError("BlockBehavior component not found on the block");
        yield break;
    }
    if (rectTransform == null)
        yield break;

    float duration = 3.0f;
    float elapsed = 0f;

    Vector2 startPosition = rectTransform.anchoredPosition;
    Vector2 targetPosition = new Vector2(startPosition.x, -250.0f);

    // 생명이 이미 깎였는지 나타내는 변수
    bool lifeReduced = false;

    while (elapsed < duration)
    {
        if (rectTransform == null)
            yield break;

        rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
        elapsed += Time.deltaTime;

        // 단어가 화면 아래로 사라질 때만 생명을 깎습니다.
        if (!block.GetComponent<BlockBehavior>().isDestroyedByAnswer && rectTransform.anchoredPosition.y <= -240.0f && !lifeReduced)
        {
            ReduceLife();
            lifeReduced = true;
        }

        if (canInput && Input.GetKeyDown(KeyCode.Return) && canDestroy)
        {
            canDestroy = false;
            StartCoroutine(DestroyBlock(rectTransform.gameObject));
            yield break;
        }

        yield return null;
    }

    if (rectTransform != null)
    {
        StartCoroutine(DestroyBlock(rectTransform.gameObject));
    }
    else
    {
        // rectTransform이 null이 아니면서 여기까지 도달한 경우, 생명을 깎습니다.
        if (!lifeReduced)
        {
            ReduceLife();
        }
    }
}


    IEnumerator CheckInputField()
    {
        while (true)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GetInputFieldText();
            }
        }
    }

    IEnumerator DestroyBlock(GameObject block)
    {
        yield return null;
        BlockBehavior blockBehavior = block.GetComponent<BlockBehavior>();
        if (blockBehavior != null && blockBehavior.isDestroyedByAnswer)
    {
      blockTextList.Remove(block);

      Vector2 positionBeforeDestroy = block.GetComponent<RectTransform>().anchoredPosition;

      Destroy(block);

      StartCoroutine(MoveNextBlock(positionBeforeDestroy));  // 정답에 의해 파괴된 경우 추가 처리 (생명 감소 없음)
    }
    else{


        if (block != null && !canInput)
        {
            ReduceLife();
        }

        if (canDestroy && block != null)
      {
          blockTextList.Remove(block);
          Vector2 positionBeforeDestroy = block.GetComponent<RectTransform>().anchoredPosition;
          Destroy(block);
          StartCoroutine(MoveNextBlock(positionBeforeDestroy));
      }
    }

        canInput = true;
        canDestroy = true;
    }

    void ReduceLife()
    {
      lives--;
      lives = Mathf.Max(lives, 0); // 생명이 0 이하로 떨어지지 않도록 함
      livesText.text = "생명: " + lives;

      if (lives <= 0)
      {
          GameOver();
        }
}


    void GameOver()
    {
        gameOver = true;
        StopGame();
        finalText.gameObject.SetActive(true);
        int totalScore = score;
        int earnedMoney = CalculateEarnedMoney(totalScore);

        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + earnedMoney);
        StatusController statusController = FindObjectOfType<StatusController>();

        finalText.text = "점수: " + score + "   얻은 돈:" + earnedMoney + "0000원";

        gameOver = true;
    }

    IEnumerator MoveNextBlock(Vector2 startPosition)
    {
        yield return new WaitForSeconds(0.1f);

        GameObject nextBlock = GetNextBlock(startPosition);

        if (nextBlock != null)
        {
            RectTransform nextRectTransform = nextBlock.GetComponent<RectTransform>();
            StartCoroutine(MoveTextDown(nextRectTransform, nextBlock));
        }
    }
    GameObject GetNextBlock(Vector2 position)
    {
        GameObject nextBlock = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject block in blockTextList)
        {
            if (block != null)
            {
                RectTransform rectTransform = block.GetComponent<RectTransform>();
                float distance = Vector2.Distance(rectTransform.anchoredPosition, position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nextBlock = block;
                }
            }
        }

        return nextBlock;
    }

    void GetInputFieldText()
{
    bool isCorrectWord = false;
    string inputText = WordInputField.text.ToUpper(); // 입력된 텍스트를 대문자로 변환

    for (int i = 0; i < blockTextList.Count; i++)
    {
        if (blockTextList[i] != null && string.Equals(inputText, blockTextList[i].GetComponent<TMP_Text>().text.ToUpper(), StringComparison.OrdinalIgnoreCase))
        {
            BlockBehavior blockBehavior = blockTextList[i].GetComponent<BlockBehavior>();
            if (blockBehavior != null)
            {
                blockBehavior.isDestroyedByAnswer = true; // 정답에 의한 파괴로 설정
            }

            string deleteTxt = blockTextList[i].GetComponent<TMP_Text>().text;
            wordList.Add(deleteTxt);
            tempList.Remove(deleteTxt);
            canInput = false;

            StartCoroutine(DestroyBlock(blockTextList[i]));
            score += 5;
            SetShowerScore();
            isCorrectWord = true;
            break;
        }
    }

    WordInputField.text = "";

    if (!isCorrectWord)
    {
        WordInputField.ActivateInputField();
    }
}


    void SetShowerScore()
    {
        scoreText.text = "점수 : " + score.ToString();
    }

    private void OnDisable()
    {
        if (createBlockTextCoroutine != null)
            StopCoroutine(createBlockTextCoroutine);

        if (checkInputFieldCoroutine != null)
            StopCoroutine(checkInputFieldCoroutine);
    }

    void StopGame()
    {
        WordInputField.gameObject.SetActive(false);
        WordInputFieldText.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        BlockParent.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        Main.gameObject.SetActive(true);
        Square.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (gameTimer > 0)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(gameTimer);
            string timerText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            timer.text = "남은 시간: " + timerText;
            if (!WordInputField.isFocused)
            {
       WordInputField.ActivateInputField();
     }
        }
        else
        {
            if (!gameOver)
            {
                StopGame();
                finalText.gameObject.SetActive(true);
                int totalScore = score;
                int earnedMoney = CalculateEarnedMoney(totalScore);

                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + earnedMoney);
                StatusController statusController = FindObjectOfType<StatusController>();

                finalText.text = "점수: " + score + "   얻은 돈:" + earnedMoney + "0000원";

                gameOver = true;
            }
        }
        if (Time.time >= nextBlockerTime && !blockerActive)
        {
            StartCoroutine(ActivateBlocker());
        }
    }

    private void SetNextBlockerTime()
        {
            // 다음 blocker 이벤트가 발생할 시간을 현재 시간 + 랜덤 추가 시간으로 설정
            nextBlockerTime = Time.time + UnityEngine.Random.Range(10, 20); // 예: 10초에서 20초 사이
        }

        IEnumerator ActivateBlocker()
        {
            blockerActive = true;

            // 랜덤하게 하나의 스프라이트 선택
            GameObject selectedBlocker = blockers[UnityEngine.Random.Range(0, blockers.Length)];
            selectedBlocker.SetActive(true);

            // blockerDuration 동안 대기
            yield return new WaitForSeconds(blockerDuration);

            // 스프라이트 비활성화
            selectedBlocker.SetActive(false);

            blockerActive = false;

            // 다음 blocker 이벤트 시간 설정
            SetNextBlockerTime();
        }

    int CalculateEarnedMoney(int totalScore)
    {
        if (totalScore >= 100 && totalScore < 150)
        {
            return 15;
        }
        else if (totalScore >= 150 && totalScore < 200)
        {
            return 25;
        }
        else if (totalScore >= 200 && totalScore < 250)
        {
            return 30;
        }
        else if (totalScore >= 250 && totalScore <= 300)
        {
            return 40;
        }
        else
        {
            return 0;
        }
    }
}
