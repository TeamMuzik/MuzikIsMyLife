using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    public bool isDestroyedByAnswer = false;
    public bool isHardWord = false; // True if the word length is more than 5 letters

    public void SetWordProperties(bool isHard)
    {
        isHardWord = isHard;
    }
}
