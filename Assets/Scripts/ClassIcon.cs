using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassIcon : MonoBehaviour
{
    public World.Faction faction;
    public bool isStatChange;

    public Sprite badIcon;
    public Sprite mediumIcon;
    public Sprite goodIcon;

    int currentIcon = 1;

    public Image faceIcon;

    Animator animator;

    private void Awake()
    {
        //EventHandler.Instance.OnEventResolved += ResolveEvent;
        faceIcon = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    public void ResolveEvent(List<GameEvent.PlayerAnswer> playerAnswers)
    {
        foreach (var v in playerAnswers) {
            World.PopulationGroups p = Answer.AdaptedStatChanges(playerAnswers[0].answer.statChanges);
            if (p.groups[(int)faction] > 0)
            {
                PlayAnimation(3);
                //if (play)
        }
            else
            {
                PlayAnimation(1);
            }

            Invoke("ChangeIcon", 0.5f);
        }
    }

    public void ChangeIcon(int newIcon)
    {
        switch (newIcon)
        {
            case 0:
                faceIcon.sprite = badIcon;
                currentIcon = 0;
                break;
            case 1:
                faceIcon.sprite = mediumIcon;
                currentIcon = 1;
                break;
            case 2:
                faceIcon.sprite = goodIcon;
                currentIcon = 2;
                break;
            default:
                break;
        }
    }

    public void PlayAnimation(int animationIndex)
    {
        switch (animationIndex)
        {
            case 0:
                animator.SetTrigger("SmallDecrement");
                break;
            case 1:
                animator.SetTrigger("LargeDecrement");
                break;
            case 2:
                animator.SetTrigger("SmallIncrement");
                break;
            case 3:
                animator.SetTrigger("LargeIncrement");
                break;
        }
    }
}
