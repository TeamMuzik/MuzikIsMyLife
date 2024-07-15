
using UnityEngine;
using UnityEngine.UI;

public class BlockTextManager : MonoBehaviour
{
    Text textComponent;

    void Start()
    {
        textComponent = GetComponent<Text>();
    }

    public void SetBlockText(string text, Vector2 position, int level)
    {
        // Your implementation here
        // For example, set the text of the Text component:
        textComponent.text = text;
        // Set the position or other properties based on the parameters
    }
}
