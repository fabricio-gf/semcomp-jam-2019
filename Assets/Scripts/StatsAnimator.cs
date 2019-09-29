using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsAnimator : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("Merchant");
            animator.SetTrigger("IncrementMerchant");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("Noble");

            animator.SetTrigger("IncrementNoble");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print("Commoner");

            animator.SetTrigger("IncrementCommoner");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            print("Alchemist");

            animator.SetTrigger("IncrementAlchemist");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            print("Cleric");

            animator.SetTrigger("IncrementCleric");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            print("Guard");

            animator.SetTrigger("IncrementGuard");
        }
    }
}
