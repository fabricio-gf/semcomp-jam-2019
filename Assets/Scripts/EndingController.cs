using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class EndingController : MonoBehaviour
{
    Animator animator;

    public TextMeshProUGUI playertext;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    public void Start()
    {
        GameFlow.Instance.OnGameOver += StartEnding;
    }

    public void StartEnding(Player player)
    {
        playertext.text = "By decree of the king and the kingdom's people, " + player.playername + "is the new ruler.\n\n< size = 140 > Huzzah!</ size >";
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
