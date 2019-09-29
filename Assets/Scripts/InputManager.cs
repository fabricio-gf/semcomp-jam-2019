using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool isListening;

    public Player player1;
    public Player player2;

    // p1 keys
    public KeyCode p1UpKey;
    public KeyCode p1RightKey;
    public KeyCode p1DownKey;
    public KeyCode p1LeftKey;

    //p2 keys
    public KeyCode p2UpKey;
    public KeyCode p2RightKey;
    public KeyCode p2DownKey;
    public KeyCode p2LeftKey;

    private void Update()
    {
        //P1
        if (Input.GetKeyDown(p1UpKey))
        {
            player1.PressUP();
        }
        if (Input.GetKeyDown(p1DownKey))
        {
            player1.PressDown();
        }
        if (Input.GetKeyDown(p1LeftKey))
        {
            player1.PressLeft();
        }
        if (Input.GetKeyDown(p1RightKey))
        {
            player1.PressRight();
        }
        //P2
        if (Input.GetKeyDown(p1UpKey))
        {
            player2.PressUP();
        }
        if (Input.GetKeyDown(p1DownKey))
        {
            player2.PressDown();
        }
        if (Input.GetKeyDown(p1LeftKey))
        {
            player2.PressLeft();
        }
        if (Input.GetKeyDown(p1RightKey))
        {
            player2.PressRight();
        }
    }
}
