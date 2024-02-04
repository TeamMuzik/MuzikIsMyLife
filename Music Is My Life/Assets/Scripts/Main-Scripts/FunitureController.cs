using UnityEngine;

public class FunitureController : MonoBehaviour
{
    public GameObject[] furnitureList;
    public GameObject[] replaceableList;
    // public GameObject[] audioList;
    public GameObject[] posterList;
    public GameObject[] cdList;
    public GameObject[] lpList;
    public GameObject[] effectorList;

    void Start()
    {
        SetFurnitureObjects(furnitureList);
        SetReplaceableFurnitureObjects(replaceableList);
        SetFurnitureObjects(posterList);
        SetFurnitureObjects(cdList);
        SetFurnitureObjects(lpList);
        SetFurnitureObjects(effectorList);
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
            ReplaceableThing thing = furnitureObj.GetComponent<ReplaceableThing>();
            thing.setReplaceableThing();
            furnitureObj.SetActive(true);
        }
    }
}
