using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryGameBelt : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.right * FactoryGameDoll.moveSpeed * Time.deltaTime;
        if (transform.position.x>=17.76f)
        {
            transform.position += new Vector3(-17.76f*2, 0, 0);
        }
    }
}
