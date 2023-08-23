using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class CPRBehaviour : GenericBehaviour
{
    private Transform matchTarget;      // 跪下时移动到的目标点
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

    void OnTriggerEnter(Collider other)
    {
        // 找到可匹配位置的对象并记录位置
        IMatchable tar = other.GetComponent<IMatchable>();
        if(tar != null)
        {
            matchTarget = tar.GetMatchTarget();
            Debug.Log("Matched:" + matchTarget);
        }
    }

    void OnTriggerExit(Collider other) 
    {
        // 检测被匹配的对象是否离开范围
        if(matchTarget != null)
        {
            IMatchable tar = other.GetComponent<IMatchable>();
            // 若离开范围则将matchTarget重置为null
            if(tar != null && matchTarget == tar.GetMatchTarget())
            {
                Debug.Log("Miss match:" + matchTarget);
                matchTarget = null;
            }
        }    
    }

    // 触发协程将玩家移动到匹配位置
    private void MatchTarget()
    {
        if(matchTarget != null)
        {
            StartCoroutine(IMatchTarget());
        }
    }

    // 玩家移动到匹配位置
    IEnumerator IMatchTarget()
    {
        bool isInTransition;
        int cnt = behaviourManager.GetAnim.layerCount;

        // MatchTarget无法在Animator处于Transition状态时执行
        // 因此要先等到现有的Transition状态结束
        do
        {
            isInTransition = false;
            // 遍历每个layer的Transition状态
            for(int i = 0; i < cnt && !isInTransition; i++)
            {
                isInTransition = behaviourManager.GetAnim.IsInTransition(i);
            }

            // 若某个layer处于Transition状态，则等到下一帧
            if(isInTransition)
            {
                yield return null;
            }

        } while(isInTransition);

        // Transition结束后执行MatchTarget
        behaviourManager.GetAnim.MatchTarget(matchTarget.position, matchTarget.rotation, AvatarTarget.Root, 
                                                new MatchTargetWeightMask(Vector3.one, 1f), 0f, 0.5f);
        
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
