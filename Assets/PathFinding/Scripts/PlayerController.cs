using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Transform target; // 目标物体
    public float distanceToTarget = 5f; // 距离目标多远时触发寻路
    public float range = 10f; // 玩家与目标之间的最大距离
    public float walkSpeed = 1f; // 玩家走路速度
    public float runSpeed = 2f; // 玩家跑步速度

    private Animator animator; // 玩家动画控制器
    private NavMeshAgent agent; // 导航代理
    private bool isMoving; // 是否正在移动
    private float currentSpeed; // 当前速度
    private bool hasReachedTarget; // 是否已经到达过目标位置
    private bool isTargetInRange; // 目标物体是否在可寻路范围内

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // 关闭默认旋转
        CheckTargetDistance(); // 检测目标距离
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        if (hasReachedTarget) return; // 如果到达了目标，直接返回

        if (isTargetInRange)
        {
            if (!isMoving)
            {
                isMoving = true;
                StartMoving();
            }

            agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }
            animator.SetFloat("Speed", currentSpeed);
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                StopMoving();
            }
        }
    }

    void CheckTargetDistance()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < range)
        {
            isTargetInRange = true;
        }
        else
        {
            isTargetInRange = false;
            StopMoving();
            hasReachedTarget = false;
        }
    }

    void StartMoving()
    {
        //animator.SetBool("IsMoving", true);
        animator.SetFloat("Speed", 0.01F);
        agent.isStopped = false;
    }

    void StopMoving()
    {
        //animator.SetBool("IsMoving", false);
        animator.SetFloat("Speed", 0);
        agent.isStopped = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            hasReachedTarget = true;
            StopMoving();
        }
    }
}
