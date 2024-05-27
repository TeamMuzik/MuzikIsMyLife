using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryGameBelt : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.down * FactoryGameDoll.moveSpeed * Time.deltaTime;
        if (transform.position.y<=-14f)
        {
            transform.position += new Vector3(0, 28f, 0);
        }
    }
}
