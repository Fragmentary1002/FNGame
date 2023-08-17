using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
表示可被检查的身体部位
挂载到表示身体不同部位的对象上，并加上碰撞体使之可以被射线检测
*/
public abstract class BuddyParts : MonoBehaviour
{

    [SerializeField]
    protected BuddyPartsInfoPanel _panelScript;         /* 信息面板脚本 */

    public void OpenInfoPanel(GameObject canvas)
    {
        _panelScript.Open(canvas, this);
    }

    public void CloseInfoPanel()
    {
        _panelScript.Close();
    }
}
