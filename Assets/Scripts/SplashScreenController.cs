using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    Animator animator;

    public bool canStart = false;

    bool beforeTutorial = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        InputManager.instance.OnPressConfirm += ShowTutorial;
        InputManager.instance.canPressConfirm = true;
    }

    public void SetCanStart()
    {
        canStart = true;
    }

    public void ShowTutorial()
    {
        beforeTutorial = false;
        animator.SetTrigger("ShowTutorial");
        AudioManager.instance.PlayClip("play");
        InputManager.instance.OnPressConfirm -= ShowTutorial;
        InputManager.instance.OnPressConfirm += PressPlay;
        canStart = false;
    }

    public void ShowTutorial(int player)
    {
        if (canStart)
        {
            ShowTutorial();
        }
    }

    public void PressPlay()
    {
        if (beforeTutorial)
        {
            ShowTutorial();
        }
        else
        {
            animator.SetTrigger("PressPlay");
            AudioManager.instance.PlayClip("play");
            InputManager.instance.canPressConfirm = false;
            InputManager.instance.OnPressConfirm -= PressPlay;
            GameFlow.Instance.StartEvent();
        }
    }

    public void PressPlay(int player)
    {
        if (canStart)
        {
            PressPlay();
        }
    }
}
