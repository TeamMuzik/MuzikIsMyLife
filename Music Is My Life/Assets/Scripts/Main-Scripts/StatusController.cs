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

    public void UpdateStatus()
    {
        NameText();
        DdayText();
        DateText();
        StatusText();
        SetAppsByYesterdayBehavior();
        SetFloorThing(floorThing);
    }

    private void NameText()
    {
        playerName.text = PlayerPrefs.GetString("PlayerName");
    }

    private void DdayText()
    {
        dday.text = PlayerPrefs.GetInt("Dday") + "일차";
    }
    private void DateText()
    {
        date.text = PlayerPrefs.GetString("Date");
    }

    private void StatusText()
    {
        status.text = "돈: " + PlayerPrefs.GetInt("Money") + "만원"
                    + "\n나의 명성: " + PlayerPrefs.GetInt("MyFame")
                    + "\n야옹의 명성: " + PlayerPrefs.GetInt("BandFame")
                    + "\n스트레스: " + PlayerPrefs.GetInt("Stress");
    }

    // 스마트폰에서 알바 앱 아이콘 비활성화
    private void SetAppsByYesterdayBehavior()
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
    private void SetFloorThing(GameObject floorThing)
    {
        int yesterday = PlayerPrefs.GetInt("Dday") - 1;
        if (yesterday < 1)
            return;
        int ydBehaviorId = PlayerPrefs.GetInt("Day" + yesterday + "_Behavior"); // 어제 한 행동 확인
        if (ydBehaviorId != -1 && ydBehaviorId < 6)
        {
            ReplaceableThing thing = floorThing.GetComponent<ReplaceableThing>(); // 스프라이트 가져오기
            SpriteRenderer spriteRenderer = floorThing.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null && ydBehaviorId >= 0 && ydBehaviorId < thing.availableSprites.Count)
            {
                // 선택한 인덱스에 해당하는 스프라이트 할당
                spriteRenderer.sprite = thing.availableSprites[ydBehaviorId];
                // 교체할 때 데이터도 교체 필요
            }
            else
            {
                Debug.LogError("Sprite Renderer가 설정되지 않았거나, 인덱스가 잘못되었습니다.");
            }
        }
    }
}