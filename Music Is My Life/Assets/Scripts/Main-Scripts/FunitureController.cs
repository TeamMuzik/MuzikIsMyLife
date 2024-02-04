using UnityEngine;


public class FunitureController : MonoBehaviour
{
    public GameObject[] furnitureList;
    public GameObject[] replaceableList;
    public GameObject[] posterList;
    public GameObject[] cdList;
    public GameObject[] lpList;

    void Start()
    {
        SetFurnitureObjects(furnitureList);
        SetReplaceableFurnitureObjects(replaceableList);
        SetFurnitureObjects(posterList);
        SetFurnitureObjects(cdList);
        SetFurnitureObjects(lpList);
    }

    public void SetFurnitureObjects(GameObject[] allFurnitureObj)
    {
        foreach (GameObject furnitureObj in allFurnitureObj)
        {
            Furniture furniture = furnitureObj.GetComponent<Furniture>();
            furniture.LoadFurnitureStatus();

            if (furniture.IsOwned && furniture.IsEquipped)
            {
                furnitureObj.SetActive(true);
            }
            else
            {
                furnitureObj.SetActive(false);
            }
        }
    }

    public void SetReplaceableFurnitureObjects(GameObject[] allRepFurnitureObj)
    {
        foreach (GameObject furnitureObj in allRepFurnitureObj)
        {
            ReplaceableFurniture furniture = furnitureObj.GetComponent<ReplaceableFurniture>();
            furniture.LoadFurnitureStatus();
            furnitureObj.SetActive(true);
            // 교체는 따로 진행 - 상점 등...
        }
    }
}
