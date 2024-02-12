using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryGameBelt : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.right * FactoryGameDoll.moveSpeed * Time.deltaTime;
        if (transform.position.x>=13.3)
        {
            transform.position += new Vector3(-13.3f*2, 0, 0);
        }
    }
}
