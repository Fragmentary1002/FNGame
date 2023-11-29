using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class CPRBehaviour : GenericBehaviour
{
    private const string BUTTON_KNEELDOWN = "Test";    // 跪下
    private const string BUTTON_TAP = "Num1";          // 拍打
    private const string BUTTON_PRESS = "Num2";        // 胸部按压
    private const string BUTTON_RESPIRATION = "Num3";  // 人工呼吸

    private Transform matchTarget;      // 跪下时移动到的目标点
    private bool isKneel = false;       // 当前是否处于跪下状态
    private bool isPress = false;
    [SerializeField]
    private GameObject mainCam;         // 主相机
    [SerializeField]
    private GameObject CPRCam;          // CPR视角相机 

    void Start()
    {

    }

    void Update()
    {
        // 动作：跪下
        if (!isKneel && Input.GetButtonDown(BUTTON_KNEELDOWN))
        {
            behaviourManager.GetAnim.SetTrigger("KneelDown");
        }

        // 后续的动作都是在跪下后才能触发
        if (!isKneel)
        {
            return;
        }

        // 按移动键站起
        if (behaviourManager.IsMoving())
        {
            pressExit();
            behaviourManager.GetAnim.SetTrigger("StandUp");
        }
        // 动作：胸部按压
        else if (Input.GetButtonDown(BUTTON_PRESS))
        {
            press();
        }
        // 动作：拍打
        else if (Input.GetButtonDown(BUTTON_TAP))
        {
            pressExit();
            behaviourManager.GetAnim.SetTrigger("Tap");
        }
        // 动作：人工呼吸
        else if (Input.GetButtonDown(BUTTON_RESPIRATION))
        {
            pressExit();
            behaviourManager.GetAnim.SetTrigger("Respiration");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 找到可匹配位置的对象并记录位置
        IMatchable tar = other.GetComponent<IMatchable>();
        if (tar != null)
        {
            matchTarget = tar.GetMatchTarget();
            Debug.Log("Matched:" + matchTarget);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 检测被匹配的对象是否离开范围
        if (matchTarget != null)
        {
            IMatchable tar = other.GetComponent<IMatchable>();
            // 若离开范围则将matchTarget重置为null
            if (tar != null && matchTarget == tar.GetMatchTarget())
            {
                Debug.Log("Miss match:" + matchTarget);
                matchTarget = null;
            }
        }
    }

    /* 协程：玩家移动到匹配位置 */
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
            for (int i = 0; i < cnt && !isInTransition; i++)
            {
                isInTransition = behaviourManager.GetAnim.IsInTransition(i);
            }

            // 若某个layer处于Transition状态，则等到下一帧
            if (isInTransition)
            {
                yield return null;
            }

        } while (isInTransition);

        // Transition结束后执行MatchTarget
        behaviourManager.GetAnim.MatchTarget(matchTarget.position, matchTarget.rotation, AvatarTarget.Root,
                                                new MatchTargetWeightMask(Vector3.one, 1f), 0f, 0.5f);

    }

    /* 动作：胸部按压 */
    private void press()
    {
        if (!isPress)
        {
            behaviourManager.GetAnim.SetTrigger("C1");
            isPress = true;
        }
        else
        {
            behaviourManager.GetAnim.SetTrigger("C2");
        }
    }

    /* 动作：结束胸部按压 */
    private void pressExit()
    {
        if (isPress)
        {
            // behaviourManager.GetAnim.SetTrigger("C3");
            isPress = false;
        }
    }

    /* 动画事件：开始下跪 */
    private void AnimEvent_KneelStart()
    {
        // 设置为不可移动和瞄准
        behaviourManager.movable = false;
        // 触发协程将玩家移动到匹配位置
        if (matchTarget != null)
        {
            StartCoroutine(IMatchTarget());
        }
        // 关闭主相机，切换到CPR视角
        mainCam.SetActive(false);
        CPRCam.SetActive(true);
    }

    /* 动画事件：结束下跪 */
    private void AnimEvent_KneelEnd()
    {
        isKneel = true;
    }


    /* 动画事件：开始站立 */
    private void AnimEvent_StandStart()
    {
        isKneel = false;
    }

    /* 动画事件：结束站立 */
    private void AnimEvent_StandEnd()
    {
        behaviourManager.movable = true;
        // 从CPR视角切换回主相机
        mainCam.SetActive(true);
        CPRCam.SetActive(false);
    }
}
