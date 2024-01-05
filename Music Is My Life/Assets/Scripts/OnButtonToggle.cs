using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnButtonToggle : MonoBehaviour
{
    public GameObject targetGameObject;

    public void Start()
    {
        targetGameObject.SetActive(false);
    }
    public void ToggleObject()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(!targetGameObject.activeSelf);
        }
    }
}
