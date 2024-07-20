using System.Collections.Generic;
using UnityEngine;

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
    public GameObject dayEndPanel; // 하루 종료 패널
    public GameObject spriteImage; // 패널 안의 스프라이트 이미지 오브젝트

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

        if (spriteRenderer == null)
        {
            Debug.LogError("spriteRenderer가 null입니다.");
            return;
        }

        if (currentDay >= 2 && currentDay <= 8)
        {
            displayedSprite = daySprites[currentDay - 2];
            shouldShowPanel = true;
        }
        else if (currentDay >= 10 && currentDay <= 14)
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
