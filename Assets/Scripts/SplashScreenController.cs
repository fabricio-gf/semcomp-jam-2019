using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    Animator animator;

    public bool canStart = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        InputManager.instance.OnPressConfirm += PressPlay;
        InputManager.instance.canPressConfirm = true;
    }

    public void SetCanStart()
    {
        canStart = true;
    }

    public void PressPlay()
    {
        animator.SetTrigger("PressPlay");
        AudioManager.instance.PlayClip("play");
        InputManager.instance.canPressConfirm = false;
        InputManager.instance.OnPressConfirm -= PressPlay;
        GameFlow.Instance.StartEvent();
    }

    public void PressPlay(int player)
    {
        if (canStart)
        {
            PressPlay();
        }
    }
}
