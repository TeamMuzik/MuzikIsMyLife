using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]

public class Dialogue
{
    [TextArea]
    public string dialogue;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite_DialogueBox;
    [SerializeField] private TMP_Text txt_Dialogue;

    private bool isDialogue = false;

    private int count = 0;

    [SerializeField] private Dialogue[] dialogues;

    public void ShowDialogue()
    {
        sprite_DialogueBox.gameObject.SetActive(true);
        txt_Dialogue.gameObject.SetActive(true);

        count = 0;
        isDialogue = true;
        NextDialogue();
    }

    private void HideDialogue()
    {
        sprite_DialogueBox.gameObject.SetActive(false);
        txt_Dialogue.gameObject.SetActive(false);

        isDialogue = false;
    }

    private void NextDialogue()
    {
        txt_Dialogue.text = dialogues[count].dialogue;
        count++;
    }

    void Start()
    {
      ShowDialogue();
    }

    void Update()
    {
        if (isDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (count < dialogues.Length)
                    NextDialogue();
                else
                    HideDialogue();
            }
        }
    }
}
