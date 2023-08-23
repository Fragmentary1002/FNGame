using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRBehaviour : GenericBehaviour
{
    private bool isKneel = false;       // 当前是否处于跪下状态

    void Start()
    {
        
    }

    void Update()
    {
        // TODO: 测试函数
        TestFunc();

        // 跪下时按移动键站起
        if(isKneel && behaviourManager.IsMoving())
        {
            StandUp();
        }
    }

    private void TestFunc()
    {
        if(Input.GetButtonDown("Test"))
        {
            KneelDown();
        }
    }


    // 触发下跪动作
    private void KneelDown()
    {
        behaviourManager.movable = false;
        behaviourManager.GetAnim.SetTrigger("KneelDown");
    }

    // 站立完成时执行
    public void KneelComplete()
    {
        isKneel = true;
    }

    // 触发站立动作
    private void StandUp()
    {
        isKneel = false;
        behaviourManager.GetAnim.SetTrigger("StandUp");
    }

    // 站立动作完成时执行
    private void StandComplete()
    {
        behaviourManager.movable = true;
    }
}
