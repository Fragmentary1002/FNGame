using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* 身体部位信息面板样例 */
public class SampleInfoPanel : BuddyPartsInfoPanel
{
    [SerializeField]
    private TMP_Text _name;
    [SerializeField]
    private TMP_Text _status;
    private SampleBuddyPart _parts;     /* 身体部位样例 */

    /* 根据部位信息设置显示内容 */
    protected override void setPanelInfo(BuddyParts parts)
    {
        /* 转换为要显示的具体部位 */
        _parts = parts as SampleBuddyPart;
        /* 设置UI显示内容 */
        _name.text = _parts.Name;
        _status.text = _parts.Status;
    }
}
