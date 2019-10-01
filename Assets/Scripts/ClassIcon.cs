﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassIcon : MonoBehaviour
{
    public World.Faction faction;
    public bool isStatChange;

    [Range(0f,1f)]
    public float changeIconThreshold = 0.4f;
    public Sprite badIcon;
    public Sprite mediumIcon;

    public Sprite goodIcon;

    //int currentIcon = 1;

    public Image faceIcon;

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
        World.PopulationGroups influenceChange = (ownerPlayer.influence - ownerPlayer.EventStartInfluence);
        float valueChange = influenceChange.groups[(int)faction];
        if (valueChange > 0)
        {
            PlayAnimation(3);
            //if (play)
        }
        else if (valueChange < 0)
        {
            PlayAnimation(1);
        }

        Invoke("ChangeIcon", 0.5f);
    }

    public void ChangeIcon()
    {
        float influence = ownerPlayer.influence.groups[(int)faction];
        if (influence < changeIconThreshold)
        {
            faceIcon.sprite = badIcon;
            //currentIcon = 0;
        }
        else if (influence <= 1 - changeIconThreshold)
        {
            faceIcon.sprite = mediumIcon;
            //currentIcon = 1;
        }
        else
        {
            faceIcon.sprite = goodIcon;
            //currentIcon = 2;
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
