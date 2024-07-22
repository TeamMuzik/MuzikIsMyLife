using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class BehaviorSprite
{
    public int behaviorId; // 행동 ID
    public List<Sprite> sprites; // 행동에 따른 스프라이트 리스트
}

public class SNSController : MonoBehaviour
{
    public List<Sprite> daySprites; // 1~7일차 스프라이트
    public List<BehaviorSprite> behaviorSprites; // 행동 기반 스프라이트 리스트
    public List<Sprite> hintSprites; // 힌트 기반 스프라이트 리스트
    public GameObject dayEndPanel; // 하루 종료 패널
    public GameObject spriteImage; // 패널 안의 스프라이트 이미지 오브젝트
    public TMP_Text playerName;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (spriteImage != null)
        {
            spriteRenderer = spriteImage.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("spriteImage에 SpriteRenderer가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("spriteImage가 설정되지 않았습니다.");
        }

        if (dayEndPanel != null)
        {
            dayEndPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("dayEndPanel이 설정되지 않았습니다.");
        }

        // PlayerPrefs 키 디버깅
        SpriteUtils.DebugPlayerPrefsKeys();
    }

    public void ShowDayStartPanel(System.Action onClosed = null)
    {
        if (spriteImage == null)
        {
            Debug.LogError("spriteImage가 null입니다.");
            return;
        }

        Transform spriteTransform = spriteImage.transform;

        // 스프라이트 교체 전 스케일 값을 저장
        Vector3 originalScale = spriteTransform.localScale;

        int currentDay = PlayerPrefs.GetInt("Dday");

        bool shouldShowPanel = false;
        Sprite displayedSprite = null;

        string PlayerName = PlayerPrefs.GetString("PlayerName");
        if (PlayerName == "연보라")
        {
      playerName.text = "연보라\n@lavender_S2";
        }
        else
        playerName.text = PlayerName;



        if (spriteRenderer == null)
        {
            Debug.LogError("spriteRenderer가 null입니다.");
            return;
        }

        if (currentDay >= 1 && currentDay <= 7)
        {
            displayedSprite = daySprites[currentDay - 1];
            shouldShowPanel = true;
        }
        else if (currentDay == 8)
        {
            int money = PlayerPrefs.GetInt("Money");
            int myFame = PlayerPrefs.GetInt("MyFame");
            int bandFame = PlayerPrefs.GetInt("BandFame");

            if (hintSprites != null && hintSprites.Count >= 8) // hintSprites가 null이 아니고, 최소 8개 이상의 요소가 있는지 확인
            {
                if (money >= 75 && myFame >= 38 && bandFame >= 75)
                {
                    displayedSprite = hintSprites[0];
                }
                else if (money >= 75 && myFame >= 38)
                {
                    displayedSprite = hintSprites[1];
                }
                else if (myFame >= 38 && bandFame >= 75)
                {
                    displayedSprite = hintSprites[2];
                }
                else if (money >= 75 && bandFame >= 75)
                {
                    displayedSprite = hintSprites[3];
                }
                else if (money >= 75)
                {
                    displayedSprite = hintSprites[4];
                }
                else if (myFame >= 38)
                {
                    displayedSprite = hintSprites[5];
                }
                else if (bandFame >= 75)
                {
                    displayedSprite = hintSprites[6];
                }
                else
                {
                    displayedSprite = hintSprites[7];
                }
                shouldShowPanel = true;
            }
            else
            {
                Debug.LogError("hintSprites가 설정되지 않았거나, 요소가 부족합니다.");
            }
        }
        else if (currentDay >= 9 && currentDay <= 13)
        {
            int yesterday = currentDay - 1;
            int behaviorId = PlayerPrefs.GetInt("Day" + yesterday + "_Behavior");

            if (behaviorSprites == null)
            {
                Debug.LogError("behaviorSprites가 null입니다.");
                return;
            }

            BehaviorSprite behaviorSprite = behaviorSprites.Find(bs => bs.behaviorId == behaviorId);

            if (behaviorSprite != null)
            {
                displayedSprite = GetRandomSprite(behaviorSprite);
                if (displayedSprite != null)
                {
                    shouldShowPanel = true;
                }
                else
                {
                    Debug.LogError("사용 가능한 스프라이트가 없습니다.");
                }
            }
            else
            {
                Debug.LogError("잘못된 behaviorId이거나 behaviorSprites에 포함되지 않았습니다.");
            }
        }
        else if (currentDay==14)
        {
          displayedSprite = daySprites[7];
          shouldShowPanel = true;
        }

        if (shouldShowPanel && displayedSprite != null)
        {
            spriteRenderer.sprite = displayedSprite;

            // 스프라이트 교체 후 원래 스케일 값을 복원
            spriteTransform.localScale = originalScale;

            SpriteUtils.SaveSprite(currentDay, displayedSprite.name); // 스프라이트 이름 저장
            if (dayEndPanel != null)
            {
                dayEndPanel.SetActive(true); // 패널 표시
            }
            else
            {
                Debug.LogError("dayEndPanel이 null입니다.");
            }
        }

        onClosed?.Invoke();
    }

    private Sprite GetRandomSprite(BehaviorSprite behaviorSprite)
    {
        if (behaviorSprite == null || behaviorSprite.sprites == null || behaviorSprite.sprites.Count == 0)
        {
            return null;
        }

        Sprite selectedSprite = behaviorSprite.sprites[Random.Range(0, behaviorSprite.sprites.Count)];
        return selectedSprite;
    }

    public void CloseDayStartPanel()
    {
        if (dayEndPanel != null)
        {
            dayEndPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("dayEndPanel이 null입니다.");
        }
    }
}
