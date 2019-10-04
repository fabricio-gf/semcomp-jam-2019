﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassIcon : MonoBehaviour
{
    public World.Faction faction;
    public bool isStatChange;

    [Range(0f,1f)]
    public float changeIconLowerThreshold = 0.2f;
    [Range(0f, 1f)]
    public float changeIconUpperThreshold = 0.6f;
    public Sprite badIcon;
    public Sprite mediumBadIcon;
    public Sprite mediumIcon;
    public Sprite mediumGoodIcon;
    public Sprite goodIcon;

    //int currentIcon = 1;

    public Image faceIcon;

    public bool isPopulationGambiarra;

    Animator animator;

    private Player ownerPlayer;
    private List<GameEvent.PlayerAnswer> lastResolution = null;

    private void Awake()
    {
        //EventHandler.Instance.OnEventResolved += ResolveEvent; //WARNING! - Fabrício, a instance do EventHandler (e do GameFlow) são instanciadas no Awake: usa o Start pra settar os listeners
        animator = GetComponent<Animator>();
        ownerPlayer = GetComponentInParent<Player>();
    }

    private void Start()
    {
        EventHandler.Instance.OnEventResolved += RecordLastResolution;
        GameFlow.Instance.OnEventEnd += DisplayChanges;
    }

    public void RecordLastResolution (List<GameEvent.PlayerAnswer> playerAnswers)
    {
        lastResolution = playerAnswers;
    }

    public void DisplayChanges()
    {
        float valueChange;
        if (!isPopulationGambiarra) // FIXME: Gambiarra sinistra
        {
            World.PopulationGroups influenceChange = (ownerPlayer.influence - ownerPlayer.EventStartInfluence);
            valueChange = influenceChange.GetGroupValueAt((int)faction);
        }
        else
        {
            valueChange =  World.Instance.groups.GetGroupValueAt((int)faction)
                - World.Instance.EventStartGroups.GetGroupValueAt((int)faction);
            // valueChange = (int)popChange.GetGroupValueAt((int)faction);
            Debug.Log("Pop change: " + valueChange);
        }

        if (valueChange > 0)
        {
            //PlayAnimation(3);
            animator.SetTrigger("LargeIncrement");
            //if (play)
        }
        else if (valueChange < 0)
        {
            //PlayAnimation(1);
            animator.SetTrigger("LargeDecrement");
        }

        Invoke("ChangeIcon", 0.5f);
    }

    public void ChangeIcon()
    {
        float influence;
        if (!isPopulationGambiarra)
        {
            influence = ownerPlayer.influence.GetGroupValueAt((int)faction);
        }
        else
        {
            influence = World.Instance.groups.GetGroupValueAt((int)faction);
        }

        if (influence < changeIconLowerThreshold)
        {
            faceIcon.sprite = badIcon;
            //currentIcon = 0;
        }
        else if (influence <= (changeIconUpperThreshold - changeIconLowerThreshold))
        {
            faceIcon.sprite = mediumBadIcon;
            //currentIcon = 1;
        }
        else if (influence <= changeIconUpperThreshold)
        {
            faceIcon.sprite = mediumIcon;
            //currentIcon = 2;
        }
        else if (influence <= (changeIconLowerThreshold + changeIconUpperThreshold))
        {
            faceIcon.sprite = mediumGoodIcon;
            //currentIcon = 3;
        }
        else
        {
            faceIcon.sprite = goodIcon;
            //currentIcon = 4;
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
