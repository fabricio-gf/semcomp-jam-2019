﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    private Animator animator = null;
    private Queue<string> sentences;

    public TextMeshProUGUI BodyText;
    public float typingSpeed;

    [HideInInspector] public bool isTyping = false;
    [HideInInspector] public bool duringDialogue = false;
    bool skipDialogue = false;

    public bool isResolutionBox = false;

    public delegate void TextBoxDelegate();
    public TextBoxDelegate OnDialogueEnded;

    private void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
        sentences = new Queue<string>();
    }

    private void Update()
    {
        /*if (p1ConfirmKeyPressed && p2ConfirmKeyPressed)
        {
            if (dialogueEnded)
            {
                return;
            }
            if (!isTyping)
            {
                DisplayNextSentence();
            }
            else
            {
                SkipDialogue();
            }
        }*/

    }

    public void ClearTextBox()
    {
        BodyText.text = string.Empty;
    }

    public void StartDialogue (Dialogue dialogue)
    {
        sentences.Clear();
        duringDialogue = true;
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
            OnDialogueEnded?.Invoke();
        }
    }

    public void SkipDialogue()
    {
        print(isTyping);
        if (isTyping)
        {
            skipDialogue = true;
        }
    }

    public void EndDialogue()
    {
        duringDialogue = false;
        ClearTextBox();
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
            if (name == "ResolutionBox2") return;

            if (transform.parent.name == "RivalEventCanvas")
            {
                Debug.Log("EndRivalEvent from text box");
                animator.SetTrigger("EndRivalEvent");
            }
            else
            {
                animator.SetTrigger("EndNormalEvent");
            }
        }
    }
}
