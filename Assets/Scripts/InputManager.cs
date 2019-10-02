using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;

    public delegate void InputManagerDelegate(int player); // 1 for player 1, 2 for player 2
    public InputManagerDelegate OnPressConfirm;
    public InputManagerDelegate OnReleaseConfirm;

    public InputManagerDelegate OnPressUp;
    public InputManagerDelegate OnPressDown;
    public InputManagerDelegate OnPressLeft;
    public InputManagerDelegate OnPressRight;
    
    public bool isListening;

    public Player player1;
    public Player player2;

    // p1 keys
    public KeyCode p1UpKey;
    public KeyCode p1RightKey;
    public KeyCode p1DownKey;
    public KeyCode p1LeftKey;
    public KeyCode p1ConfirmKey;
 

    //p2 keys
    public KeyCode p2UpKey;
    public KeyCode p2RightKey;
    public KeyCode p2DownKey;
    public KeyCode p2LeftKey;
    public KeyCode p2ConfirmKey;

    public bool canPress = false;
    public bool canPressConfirm = false;

    public bool twoControllersConnected = false;

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
        if (Input.GetKeyUp(p1ConfirmKey))
        {
            OnReleaseConfirm?.Invoke(1);
        }
        if (Input.GetKeyUp(p2ConfirmKey))
        {
            OnReleaseConfirm?.Invoke(2);
        }
        if (canPressConfirm)
        {
            if (Input.GetKeyDown(p1ConfirmKey))
            {
                OnPressConfirm?.Invoke(1);
            }
            if (Input.GetKeyDown(p2ConfirmKey))
            {
                OnPressConfirm?.Invoke(2);
            }
        }

        if (!canPress) return;

        //P1
        if (Input.GetKeyDown(p1UpKey))
        {
            OnPressUp?.Invoke(1);
            player1.PressUP();
        }
        if (Input.GetKeyDown(p1DownKey))
        {
            OnPressDown?.Invoke(1);
            player1.PressDown();
        }
        if (Input.GetKeyDown(p1LeftKey))
        {
            OnPressLeft?.Invoke(1);
            player1.PressLeft();
        }
        if (Input.GetKeyDown(p1RightKey))
        {
            OnPressRight?.Invoke(1);
            player1.PressRight();
        }
        //P2
        if (Input.GetKeyDown(p2UpKey))
        {
            OnPressUp?.Invoke(2);
            player2.PressUP();
        }
        if (Input.GetKeyDown(p2DownKey))
        {
            OnPressDown?.Invoke(2);
            player2.PressDown();
        }
        if (Input.GetKeyDown(p2LeftKey))
        {
            OnPressLeft?.Invoke(2);
            player2.PressLeft();
        }
        if (Input.GetKeyDown(p2RightKey))
        {
            OnPressRight?.Invoke(2);
            player2.PressRight();
        }
    }
}
