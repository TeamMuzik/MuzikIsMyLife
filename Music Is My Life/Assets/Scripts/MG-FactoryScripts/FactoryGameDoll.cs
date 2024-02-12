using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryGameDoll : MonoBehaviour
{
    public static float moveSpeed = 3f;
    private FactoryGame FactoryGameInstance;
    private FactoryGameTimer FactoryGameTimerInstance;

    void Start()
    {
        FactoryGameInstance = FindObjectOfType<FactoryGame>();
        FactoryGameTimerInstance = FindObjectOfType<FactoryGameTimer>();
    }

    void Update()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        if (transform.position.x >= 9)
        {
            StartCoroutine(FactoryGameTimerInstance.BlinkText(FactoryGameTimer.totalTime));
            FactoryGameTimer.totalTime -= 4f;
            Destroy(gameObject);
            FactoryGameInstance.SpawnKeyBoards();
        }
    }
}
