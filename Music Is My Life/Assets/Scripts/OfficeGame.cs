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

    public TMP_InputField WordInputField;
    public TMP_Text WordInputFieldText;
    public TMP_Text timer;

    public GameObject pBlockText;
    public GameObject Square;
    public Transform BlockParent;
    public Button Main;
    List<GameObject> blockTextList = new List<GameObject>();
    List<string> tempList = new List<string>();

    int level = 1; // Assuming a default value for level
    int score = 0;
    public TMP_Text scoreText;
    public TMP_Text finalText;

    // Coroutine references
    Coroutine createBlockTextCoroutine;
    Coroutine checkInputFieldCoroutine;

    bool canInput = true;
    bool canDestroy = true;
    bool gameOver = false;

    float gameTimer = 90.0f;// New variable to track whether the block can be destroyed

    void Start()
    {
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
            checkInputFieldCoroutine = StartCoroutine(CheckInputField());
            scoreText.text = "점수 : " + score.ToString();
            InvokeRepeating("UpdateGameTimer", 1.0f, 1.0f);
            timer.text = "남은 시간: 1:30";
            Main.gameObject.SetActive(false);
            finalText.gameObject.SetActive(false);

        }
        else
        {
            Debug.LogError("Failed to load 'OfficeGameText' file.");
        }
    }

    void UpdateGameTimer()
   {
       gameTimer -= 1.0f;
       TimeSpan timeSpan = TimeSpan.FromSeconds(gameTimer);
       string timerText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

       // Assuming you have a Text component to display the timer
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

                // Set position randomly
                RectTransform tempRectTransform = tempBlock.GetComponent<RectTransform>();
                tempRectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(-300.0f, 300.0f), 400.0f); // Starting position at the top

                // Assuming BlockTextManager script is attached to the TMP_Text component
                tempTextComponent.text = wordList[UnityEngine.Random.Range(0, wordList.Count)];

                // Set tag
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

                StartCoroutine(MoveTextDown(tempRectTransform)); // Start moving the text down

                yield return new WaitForSeconds(3.0f / level);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator MoveTextDown(RectTransform rectTransform)
{
    if (rectTransform == null)
        yield break; // Skip if RectTransform is already destroyed

    float duration = 3.0f; // Change this value to set a fixed duration for the movement
    float elapsed = 0f;

    Vector2 startPosition = rectTransform.anchoredPosition;
    Vector2 targetPosition = new Vector2(startPosition.x, -200.0f);

    while (elapsed < duration)
    {
        if (rectTransform == null)
            yield break; // Exit if RectTransform is destroyed during the Coroutine

        rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
        elapsed += Time.deltaTime;

        // 입력이 가능하고 파괴 가능한 상태인 경우 처리
        if (canInput && Input.GetKeyDown(KeyCode.Return) && canDestroy)
        {
            canDestroy = false; // 파괴 플래그 비활성화
            // 정답 처리
            StartCoroutine(DestroyBlock(rectTransform.gameObject));
            yield break;
        }

        yield return null;
    }

    // y 값이 -200 이하이면 파괴
    if (rectTransform != null && rectTransform.anchoredPosition.y <= -199.0f)
    {
        StartCoroutine(DestroyBlock(rectTransform.gameObject));
    }
    else if (rectTransform != null)
    {
        rectTransform.anchoredPosition = targetPosition;
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
    yield return null; // 한 프레임 대기

    // 파괴 가능한 상태일 때만 파괴
    if (canDestroy && block != null)
    {
        // Remove the reference from the list before destroying
        blockTextList.Remove(block);

        // Store the position before destroying
        Vector2 positionBeforeDestroy = block.GetComponent<RectTransform>().anchoredPosition;

        Destroy(block); // Destroy the block

        // Continue moving the next block from the stored position
        StartCoroutine(MoveNextBlock(positionBeforeDestroy));
    }

    canInput = true; // 입력 가능하도록 설정
    canDestroy = true; // 파괴 가능하도록 설정
}

IEnumerator MoveNextBlock(Vector2 startPosition)
{
    // Wait for a short duration before moving the next block
    yield return new WaitForSeconds(0.1f);

    // Get the next block (if any) after a short delay
    GameObject nextBlock = GetNextBlock(startPosition);

    // If there is a next block, move it down
    if (nextBlock != null)
    {
        RectTransform nextRectTransform = nextBlock.GetComponent<RectTransform>();
        StartCoroutine(MoveTextDown(nextRectTransform));
    }
}

GameObject GetNextBlock(Vector2 position)
{
    // Find the block closest to the position before destruction
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

    for (int i = 0; i < blockTextList.Count; i++)
    {
        if (blockTextList[i] != null &&
            string.Equals(WordInputField.text, blockTextList[i].GetComponent<TMP_Text>().text, StringComparison.OrdinalIgnoreCase))
        {
            string deleteTxt = blockTextList[i].GetComponent<TMP_Text>().text;
            wordList.Add(deleteTxt);
            tempList.Remove(deleteTxt);
            canInput = false;

            // Correct word, so we don't set canDestroy to true here
            StartCoroutine(DestroyBlock(blockTextList[i])); // Destroy the block
            score += 5;
            SetShowerScore();
            isCorrectWord = true;
            break;
        }
    }

    // Clear the text input field regardless of whether the word is correct or not
    WordInputField.text = "";

    // Activate the input field only if the word is incorrect
    if (!isCorrectWord)
    {
        WordInputField.ActivateInputField();

        // 오답이 입력되었을 때 다음 블록으로 이동
        StartCoroutine(MoveNextBlock(Vector2.zero));
    }
}





    void SetShowerScore()
    {
        scoreText.text = "점수 : " + score.ToString();
    }

    // Stop coroutines when the script is disabled
    private void OnDisable()
    {
        if (createBlockTextCoroutine != null)
            StopCoroutine(createBlockTextCoroutine);

        if (checkInputFieldCoroutine != null)
            StopCoroutine(checkInputFieldCoroutine);
    }






    void StopGame(){
      WordInputField.gameObject.SetActive(false);
      WordInputFieldText.gameObject.SetActive(false);
      timer.gameObject.SetActive(false);
      BlockParent.gameObject.SetActive(false);
      scoreText.gameObject.SetActive(false);
      Main.gameObject.SetActive(true);
      Square.gameObject.SetActive(false);
    }

    // ... (rest of the existing code)


    // Update is called once per frame

    void Update()
{
    if (gameTimer > 0)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(gameTimer);
        string timerText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        // Assuming you have a Text component to display the timer
        timer.text = "남은 시간: " + timerText;
    }
    else
    {
        if (!gameOver) // 추가: 게임이 종료되지 않은 경우에만 처리
        {
            StopGame();
            finalText.gameObject.SetActive(true);
            int totalScore = score;
            int earnedMoney = CalculateEarnedMoney(totalScore);

            // StatusController의 정보 업데이트
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + earnedMoney);
            StatusController statusController = FindObjectOfType<StatusController>();

            // 결과를 화면에 표시
            finalText.text = "점수: " + score + "   얻은 돈:" + earnedMoney+"0000원";

            gameOver = true; // 추가: 게임 종료 상태로 설정
        }
    }
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
