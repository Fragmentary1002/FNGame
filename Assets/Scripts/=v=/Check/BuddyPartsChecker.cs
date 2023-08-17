using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 实现查看的身体部位的功能 */
public class BuddyPartsChecker : MonoBehaviour
{
    [SerializeField]
    private float _range = 5f;					/* 查看的距离上限 */
    private GameObject _canvas;					/* Canvas 对象 */
    private string _checkButton = "Aim";		/* 检查按钮 */
    private Camera _camComponent;				/* Camera 组件 */
    private RaycastHit _hit;                    /* 射线检测信息 */
    private GameObject _lastLookedPart;         /* 上一次看向的部位 */
    private BuddyParts _partScript;             /* 看向部位的脚本 */
    private BuddyParts _lastPartScript;         /* 上一次看向部位的脚本 */

    void Awake()
    {
        _camComponent = GetComponentInChildren<Camera>();				/* 获取Camera组件 */
        _canvas = GetComponentInChildren<Canvas>().gameObject;			/* 获取Canvas对象 */
    }

    void Update()
    {
        /* 按下检查按钮并且射线检测到物体 */
        if (Input.GetButton(_checkButton)
        && Physics.Raycast(getCameraCentralViewportRay(), out _hit, _range))
        {
            /* 比较是否与上次看到的对象相同 */
            if (_hit.collider.gameObject != _lastLookedPart)
            {
                _lastLookedPart = _hit.collider.gameObject;                 /* 记录查看对象 */
                _partScript = _lastLookedPart.GetComponent<BuddyParts>();   /* 获取对象脚本 */
                /* 关闭上次观察部位的面板 */
                if (_lastPartScript != null)
                {
                    _lastPartScript.CloseInfoPanel();
                }
                /* 打开本次查看部位的面板 */
                if (_partScript != null)
                {
                    _partScript.OpenInfoPanel(_canvas);
                }

                _lastPartScript = _partScript;      /* 记录查看部位脚本 */
            }
        }
        /* 移开视线 */
        else if (_lastPartScript != null)
        {
            /* 关闭面板 */
            _lastPartScript.CloseInfoPanel();
            /* 重置记录 */
            _lastPartScript = null;
            _lastLookedPart = null;
        }
    }

    /* 获取Camera中心视点射线 */
    private Ray getCameraCentralViewportRay()
    {
        return _camComponent.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    }
}
