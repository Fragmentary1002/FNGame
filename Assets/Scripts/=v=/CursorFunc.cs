using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFunc : MonoBehaviour
{
    private string _showCursorButton = "ShowCursor";    /* 显示光标按钮 */
    private bool _is_show;

    void Awake()
    {
        hideCursor();
    }

    void Update()
    {
        if (Input.GetButtonDown(_showCursorButton))
        {
            if (!_is_show)
                showCursor();
            else
                hideCursor();
        }
    }

    private void hideCursor()
    {
        _is_show = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void showCursor()
    {
        _is_show = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
