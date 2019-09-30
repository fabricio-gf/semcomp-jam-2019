using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;

    public delegate void InputManagerDelegate(KeyCode keyCode);
    public InputManagerDelegate OnKeyDown;

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

    public bool canPress = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (!canPress) return;

        //P1
        if (Input.GetKeyDown(p1UpKey))
        {
            OnKeyDown?.Invoke(p1UpKey);
            player1.PressUP();
        }
        if (Input.GetKeyDown(p1DownKey))
        {
            OnKeyDown?.Invoke(p1DownKey);
            player1.PressDown();
        }
        if (Input.GetKeyDown(p1LeftKey))
        {
            OnKeyDown?.Invoke(p1LeftKey);

            player1.PressLeft();
        }
        if (Input.GetKeyDown(p1RightKey))
        {
            OnKeyDown?.Invoke(p1RightKey);
            player1.PressRight();
        }
        //P2
        if (Input.GetKeyDown(p2UpKey))
        {
            OnKeyDown?.Invoke(p2RightKey);
            player2.PressUP();
        }
        if (Input.GetKeyDown(p2DownKey))
        {
            OnKeyDown?.Invoke(p2RightKey);
            player2.PressDown();
        }
        if (Input.GetKeyDown(p2LeftKey))
        {
            OnKeyDown?.Invoke(p2RightKey);
            player2.PressLeft();
        }
        if (Input.GetKeyDown(p2RightKey))
        {
            OnKeyDown?.Invoke(p2RightKey);
            player2.PressRight();
        }
    }
}
