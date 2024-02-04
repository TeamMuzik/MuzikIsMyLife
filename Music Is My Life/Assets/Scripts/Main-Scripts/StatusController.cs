using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text dday;
    public TMP_Text date;
    public TMP_Text status;
    public GameObject[] allAppsObj; // 알바만 넣었음
    public GameObject floorThing;

    void Start()
    {
        StatusChanger.UpdateDay(); // 날짜 업데이트
        // 14일이 지나면 엔딩으로 이동
        if (PlayerPrefs.GetInt("Dday") > 14)
        {
            GoToEnding();
        }
        // 스트레스가 100 이상일 경우 게임오버
        if (PlayerPrefs.GetInt("Stress") >= 100)
        {
            GoToGameOver();
        }
        NameText(); // 플레이어 이름
        DdayText();
        DateText();
        StatusText();
        SetAppsByYesterdayBehavior();
        SetFloorThing(floorThing);
    }

    public void GoToEnding() // 엔딩으로
    {
        int money = PlayerPrefs.GetInt("Money");
        int myFame = PlayerPrefs.GetInt("MyFame");
        int bandFame = PlayerPrefs.GetInt("Fame");

        SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
        if (money > 2500000)
        {
            sceneMove.targetScene = "Ending-Expedition";
        }
        else if (myFame > 100)
        {
            sceneMove.targetScene = "Ending-OpeningBand";
        }
        else if (bandFame > 15)
        {
            sceneMove.targetScene = "Ending-ConcertInKorea";
        }
        else
        {
            sceneMove.targetScene = "Ending-Normal";
        }
        sceneMove.ChangeScene();
    }

    public void GoToGameOver()
    {
        SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
        sceneMove.targetScene = "Ending-GameOver";
        sceneMove.ChangeScene();
    }

    public void NameText()
    {
        playerName.text = PlayerPrefs.GetString("PlayerName");
    }

    public void DdayText()
    {
        dday.text = PlayerPrefs.GetInt("Dday") + "일차";
    }
    public void DateText()
    {
        date.text = PlayerPrefs.GetString("Date");
    }

    public void StatusText()
    {
        status.text = "돈: " + PlayerPrefs.GetInt("Money") + "만원"
                    + "\n나의명성: " + PlayerPrefs.GetInt("MyFame")
                    + "\n야옹의명성: " + PlayerPrefs.GetInt("BandFame")
                    + "\n스트레스: " + PlayerPrefs.GetInt("Stress");
    }

    // 스마트폰에서 알바 앱 아이콘 비활성화
    void SetAppsByYesterdayBehavior()
    {
        int yesterday = PlayerPrefs.GetInt("Dday") - 1;
        if (yesterday > 0)
        {
            int ydBehaviorId = PlayerPrefs.GetInt("Day" + yesterday + "_Behavior");
            foreach (GameObject appObj in allAppsObj)
            {
                Button button = appObj.GetComponent<Button>();
                DayBehavior dayBehavior = appObj.GetComponent<DayBehavior>();
                GameObject childObject = appObj.transform.Find("loading-image").gameObject; 
                if (dayBehavior.behaviorId == ydBehaviorId)
                {
                    if (childObject != null)
                        childObject.SetActive(true);
                    button.interactable = false;
                }
                else
                {
                    if (childObject != null)
                        childObject.SetActive(false);
                    button.interactable = true;
                }
                Debug.Log("appObj:" + dayBehavior.behaviorId +" " + appObj.name);
            }
        }
    }

    // 전날 한 행동에 따라 바닥 물건 변경
    public void SetFloorThing(GameObject floorThing)
    {
        int yesterday = PlayerPrefs.GetInt("Dday") - 1;
        if (yesterday < 1)
            return;
        int ydBehaviorId = PlayerPrefs.GetInt("Day" + yesterday + "_Behavior"); // 어제 한 행동 확인
        if (ydBehaviorId < 6)
        {
            ReplaceableFurniture furniture = floorThing.GetComponent<ReplaceableFurniture>(); // 스프라이트 가져오기
            SpriteRenderer spriteRenderer = floorThing.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null && ydBehaviorId >= 0 && ydBehaviorId < furniture.availableSprites.Count)
            {
                // 선택한 인덱스에 해당하는 스프라이트 할당
                spriteRenderer.sprite = furniture.availableSprites[ydBehaviorId];
                // 교체할 때 데이터도 교체 필요
            }
            else
            {
                Debug.LogError("Sprite Renderer가 설정되지 않았거나, 인덱스가 잘못되었습니다.");
            }
        }
    }
}