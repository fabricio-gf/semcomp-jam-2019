using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    private Animator animator = null;
    private Queue<string> sentences;

    public TextMeshProUGUI BodyText;
    public float typingSpeed;

    bool isTyping = false;
    bool skipDialogue = false;
    bool dialogueEnded = false;

    public bool isResolutionBox = false;

    private void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (dialogueEnded) return;
            if (!isTyping)
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue (Dialogue dialogue)
    {
        sentences.Clear();
        dialogueEnded = false;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        BodyText.text = string.Empty;

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        AudioManager.instance.PlayClip("writing");
        isTyping = true;
        foreach(char letter in sentence.ToCharArray())
        {
            if (skipDialogue)
            {
                BodyText.text = sentence;
                skipDialogue = false;
                break;
            }
            BodyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;

        if (sentences.Count == 0)
        {
            EndDialogue();
        }
    }

    public void SkipDialogue()
    {
        skipDialogue = true;
    }

    public void EndDialogue()
    {
        dialogueEnded = true;
        if (!isResolutionBox)
        {
            if (transform.parent.name == "RivalEventCanvas")
            {
                animator.SetTrigger("StartCountdown");
            }
            else
            {
                animator.SetTrigger("ShowButtons");
            }
        }
        else
        {

        }
    }
}
