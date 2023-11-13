using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    int loopIndex = 0;
    int loopDirection = 1;
    [SerializeField]
    private Transform[] patrolPath;
    [SerializeField]
    private float threshold = 0.2f;     // 判断是否到达目标点的最大平面距离
    [SerializeField]
    private float rotateScale = 10f;    // 人物转向速度缩放

    void FixedUpdate()
    {
        patrol();
    }

    // 控制巡逻方向
    private void patrol()
    {
        // 至少需要两个目标点
        if (patrolPath.Length < 2)
        {
            Debug.LogError("Patrol: 至少需要两个巡逻目标点");
            return;
        }

        Vector3 target = patrolPath[loopIndex].position;

        // 根据平面距离判断是否到达目标点
        if (Math.Abs((new Vector2(transform.position.x, transform.position.z) - new Vector2(target.x, target.z)).magnitude) <= threshold)
        {
            // 边界判断
            if (loopIndex + loopDirection < 0 || loopIndex + loopDirection >= patrolPath.Length)
            {
                loopDirection = -loopDirection;
            }

            // 切换下一个
            loopIndex += loopDirection;
            target = patrolPath[loopIndex].position;

            Debug.Log("switch to " + loopIndex);
        }

        // 朝向目标
        Vector3 dir = target - transform.position;
        dir.y = 0; // Y轴不旋转
        Quaternion rotate = Quaternion.LookRotation(dir);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, rotateScale * Time.fixedDeltaTime);
    }
}
