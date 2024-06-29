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

    // 일관된 위치를 위한 오프셋 값
    public Vector3 positionOffset = new Vector3(0, 0, 0);

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
    }

    public void ShowDayStartPanel(System.Action onClosed = null)
    {
        if (spriteImage == null || spriteRenderer == null || dayEndPanel == null)
        {
            Debug.LogError("필수 설정이 누락되었습니다.");
            return;
        }

        Transform spriteTransform = spriteImage.transform;
        spriteTransform.localPosition = positionOffset;

        int currentDay = PlayerPrefs.GetInt("Dday");

        bool shouldShowPanel = false;
        Sprite displayedSprite = null;

        if (currentDay >= 2 && currentDay <= 8)
        {
            displayedSprite = daySprites[currentDay - 2];
            spriteRenderer.sprite = displayedSprite;
            shouldShowPanel = true;
        }
        else if (currentDay >= 10 && currentDay <= 14)
        {
            int yesterday = currentDay - 1;
            int behaviorId = PlayerPrefs.GetInt("Day" + yesterday + "_Behavior");

            if (behaviorSprites != null)
            {
                BehaviorSprite behaviorSprite = behaviorSprites.Find(bs => bs.behaviorId == behaviorId);

                if (behaviorSprite != null)
                {
                    displayedSprite = GetRandomSprite(behaviorSprite);
                    if (displayedSprite != null)
                    {
                        spriteRenderer.sprite = displayedSprite;
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
            else
            {
                Debug.LogError("behaviorSprites가 null입니다.");
            }
        }

        if (shouldShowPanel && displayedSprite != null)
        {
            SpriteUtils.SaveSprite(currentDay, displayedSprite); // 스프라이트 저장
            Debug.Log($"Saving sprite for day {currentDay}: {displayedSprite.name}"); // 디버그 로그 추가
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
    }
}
