using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonthChange : MonoBehaviour
{
    public TextMeshProUGUI monthText;
    public TextMeshProUGUI backgroundMonthText;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        GameFlow.Instance.OnEventStart += ChangeMonth;
    }

    public void ChangeMonth(GameEvent _gameEvent)
    {
        animator.SetTrigger("StartMonthTransition");
    }

    public void ChangeMonthText()
    {
        monthText.text = "Month " +  GameFlow.Instance.Turn;
        backgroundMonthText.text = "Month " + GameFlow.Instance.Turn;
    }
}
