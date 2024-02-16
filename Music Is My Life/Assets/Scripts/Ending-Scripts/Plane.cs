using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    float rightMoveSpeed = 4.7475f;
    float UpMoveSpeed = 3f;

    void Update()
    {
        transform.position += Vector3.right * rightMoveSpeed * Time.deltaTime;
        transform.position += Vector3.up * UpMoveSpeed * Time.deltaTime;
        if(transform.position.x > 540f)
        {
            Destroy(gameObject);
        }
    }
    
}
