using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartEnding()
    {
        animator.SetTrigger("OpenEnding");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    } 

    public void MuteGame()
    {
        if (Input.GetKeyDown(KeyCode.M)){
            AudioManager.instance.MuteAll();
        }
    }
}
