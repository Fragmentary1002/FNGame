using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//场景
public class change : MonoBehaviour
{
    /// <summary>
    /// 转换到场景1
    /// </summary>
    public void SceneInpatientWard()
    {
        SceneManager.LoadScene("inpatientWard");
        //方式二 SceneManager.LoadScene(0);
    }
    /// <summary>
    /// 切换到场景2
    /// </summary>
    public void SceneChoose()
    {
        SceneManager.LoadScene("choose");
        //方式二 SceneManager.LoadScene(1);
    }
    public void SceneCity()
    {
        SceneManager.LoadScene("city");
        //方式二 SceneManager.LoadScene(1);
    }
    public void SceneMain()
    {
        SceneManager.LoadScene("main");
        //方式二 SceneManager.LoadScene(1);
    }
    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}

