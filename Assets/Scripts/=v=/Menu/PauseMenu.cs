using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private string _pauseButton = "Cancel";		/* 暂停按钮 */
    private bool _is_open = false;
    [SerializeField]
    private GameObject _pauseMenu;

    void Awake()
    {
        _pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown(_pauseButton))
        {
            if (!_is_open)
                Open();
            else
                Close();
        }
    }

    public void Open()
    {
        _is_open = true;
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        _is_open = false;
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("main");
    }

}
