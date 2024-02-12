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
    public Button Next;
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
        Next.gameObject.SetActive(false);
        isDialogue = false;
    }

    private void NextDialogue()
    {
        if (count < dialogues.Length)
        {
            txt_Dialogue.text = dialogues[count].dialogue;
            count++;
        }
        else
        {
            HideDialogue();
        }
    }

    // 추가된 함수: 버튼을 통해 호출될 함수
    public void OnNextButtonClick()
    {
        if (isDialogue)
        {
            NextDialogue();
        }
    }

    void Start()
    {
        ShowDialogue();
    }

    // Update 함수는 사용하지 않음
}
