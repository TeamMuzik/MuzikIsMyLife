using UnityEngine;

public class FunitureController : MonoBehaviour
{
    public GameObject[] furnitureList;
    public GameObject[] replaceableList;
    public GameObject[] posterList;
    public GameObject[] cdList;
    public GameObject[] lpList;
    public GameObject[] effectorList;

    public void UpdateFurnitures()
    {
        // 랜덤 선택되어야 하는 가구들
        SelectRandomRepCurrentIndex();

        SetFurnitureObjects(furnitureList);
        SetFurnitureObjects(posterList);
        SetFurnitureObjects(cdList);
        SetFurnitureObjects(lpList);
        SetFurnitureObjects(effectorList);
        SetRepFurnitureObjects(replaceableList);
    }

    void SetFurnitureObjects(GameObject[] allFurnitureObj)
    {
        foreach (GameObject obj in allFurnitureObj)
        {
            Furniture furniture = obj.GetComponent<Furniture>();
            furniture.LoadFurnitureStatus();

            if (furniture.IsOwned)
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
    }

    void SetRepFurnitureObjects(GameObject[] allRepFurnitureObj)
    {
        foreach (GameObject obj in allRepFurnitureObj)
        {
            ReplaceableThing repThing = obj.GetComponent<ReplaceableThing>();
            repThing.SetReplaceableThing();
            obj.SetActive(true);
        }
    }

    void SelectRandomRepCurrentIndex() // RandRep 가구들 Sprite 수 바뀌면 조정해야 함
    {
        int randIndex = Random.Range(0, 4); // randIndex 뽑기: 0 ~ 개수-1

        if (randIndex == 0) // 캐릭터 & 침대: 앉은(0)인 경우 동일해야
        {
            PlayerPrefs.SetInt("BED_CURRENT", 0);
            PlayerPrefs.SetInt("CHARACTER_CURRENT", 0);
        }
        else
        {
            PlayerPrefs.SetInt("BED_CURRENT", 1);
            PlayerPrefs.SetInt("CHARACTER_CURRENT", randIndex);
        }

        // 창문
        int seasonNum = PlayerPrefs.GetInt("SeasonNum");
        if (seasonNum == 1) // 3월: 0~1번 창문
            PlayerPrefs.SetInt("WINDOW_CURRENT", Random.Range(0, 2));
        else if (seasonNum == 2) // 8월: 0~2번 창문 (+비)
            PlayerPrefs.SetInt("WINDOW_CURRENT", Random.Range(0, 3));
        else // 12월: 0~3번 창문 (+눈)
            PlayerPrefs.SetInt("WINDOW_CURRENT", Random.Range(0, 4));

        // 커튼
        PlayerPrefs.SetInt("CURTAIN_CURRENT", Random.Range(0, 2));
    }
}
