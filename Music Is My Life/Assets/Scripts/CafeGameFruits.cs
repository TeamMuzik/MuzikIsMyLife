using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeGameFruits : MonoBehaviour
{
    private CafeGame cafeGameInstance;
    public int objIndex;
    int ButtonState = 0;

    void Start()
    {
        cafeGameInstance = FindObjectOfType<CafeGame>();
    }

    public void GetIndex (int num)
    {
        objIndex = num;
    }

    void Update()
    {
        if (objIndex == CafeGame.turn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ButtonState = 1;
            }
            if (ButtonState == 1 && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity);
                if (hit.collider != null)
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    if (clickedObject.name == cafeGameInstance.TotalOrder[objIndex].name)
                    {
                        CafeGame.turn++;
                        Destroy(gameObject);
                        ButtonState = 0;               
                    }
                }
            }  
        }  
    }
}
