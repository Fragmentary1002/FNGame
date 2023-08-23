using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 可匹配的对象
public interface IMatchable
{
    public Transform GetMatchTarget();
}


public class PatientBehavior : MonoBehaviour, IMatchable
{
    [SerializeField] private Transform MatchTarget;

    public Transform GetMatchTarget()
    {
        return MatchTarget;
    }
}
