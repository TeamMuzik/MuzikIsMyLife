using System.Collections.Generic;
using UnityEngine;

public class FortuneManager : MonoBehaviour
{
    public List<Sprite> availableSprites; // 가구의 여러 스프라이트를 저장하는 리스트
    public GameObject fortuneImage;
    private SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = fortuneImage.GetComponent<SpriteRenderer>();

        // 이전 Fortune 효과 초기화
        DayFortune.ResetFortuneEffect();

        int fortuneId = DayFortune.GetTodayFortuneId();
        if (fortuneId != -1)
        {
            Debug.Log("오늘의 운세(이미 확인): " + fortuneId);
            SetFortuneMessageSprite(fortuneId);
        }
    }

    public void PickRandomFortune()
    {
        int fortuneId = DayFortune.GetTodayFortuneId();
        if (fortuneId == -1)
        {
            int dayNum = PlayerPrefs.GetInt("Dday");
            fortuneId = Random.Range(1, 11);
            PlayerPrefs.SetInt($"Day{dayNum}_Fortune", fortuneId);
            Debug.Log("오늘의 운세: " + fortuneId);
            DayFortune.AddOrLogFortuneEffect(fortuneId);
            SetFortuneMessageSprite(fortuneId);
        }
        else
        {
            Debug.Log("오늘의 운세를 이미 보았습니다.");
        }
    }

    public void SetFortuneMessageSprite(int fortuneId)
    {
        // 선택한 인덱스에 해당하는 스프라이트 할당
        if (spriteRenderer != null && fortuneId > 0 && fortuneId <= availableSprites.Count)
        {
            spriteRenderer.sprite = availableSprites[fortuneId - 1];
        }
        else
        {
            Debug.LogError("SetFortuneMessageSprite: Sprite Renderer가 설정되지 않았거나, 인덱스가 잘못되었습니다.");
        }
    }

    public void GoBackToMain()
   {
       PlayerPrefs.SetInt("FromFortuneScene", 1); // Main 씬으로 돌아갈 때 플래그 설정
       PlayerPrefs.Save();
       SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
       sceneMove.targetScene = "Main";
       sceneMove.ChangeScene();
   }
}
