using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeGameFruits : MonoBehaviour
{
    private CafeGame cafeGameInstance;
    void Start()
    {
        cafeGameInstance = FindObjectOfType<CafeGame>();
    }

    void Update()
    {
         if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity);

            for (int i = 0; i < 12; i ++)
            {
                if (hit.collider != null)
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    if (clickedObject.name == cafeGameInstance.TotalOrder[i].name)
                    {
                        Destroy(gameObject);                        
                    }
                }
            }
        }       
    }
}
