using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 身体各部位的信息面板 */
public abstract class BuddyPartsInfoPanel : MonoBehaviour
{
    /* 根据部位信息设置显示内容 */
    protected abstract void setPanelInfo(BuddyParts parts);

    void Awake()
    {
        // gameObject.SetActive(false);
    }

    /*
    设置Panel显示位置
    将左上角至于canvas的中心
    */
    protected void setPanelPosition(GameObject canvas)
    {
        RectTransform panelRect = gameObject.GetComponent<RectTransform>();

        /* 设为Canvas子物体 */
        gameObject.transform.SetParent(canvas.transform);
        /* 设置Panel的锚点为Canvas的中心 */
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        /* 设置Panel的偏移量为零 */
        panelRect.anchoredPosition = Vector2.zero;
        /* 设置Panel的支点为左上角 */
        panelRect.pivot = new Vector2(0, 1f);
    }

    /* 打开面板 */
    public void Open(GameObject canvas, BuddyParts parts)
    {
        if (!gameObject.activeSelf)
        {
            setPanelPosition(canvas);
            setPanelInfo(parts);
            gameObject.SetActive(true);
        }
    }

    /* 关闭面板 */
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
