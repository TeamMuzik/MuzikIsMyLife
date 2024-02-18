using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    float time;
    float moveSpeed = 4f;

    void Update()
    {
        transform.localScale = new Vector3 (20, 20, 0) * (1+time);
        transform.position += Vector3.up * moveSpeed * Time.deltaTime; 
        time += Time.deltaTime;
    }

    public void resetAnim()
    {
        time = 0;
        transform.localScale = Vector3.one;
    }
}
