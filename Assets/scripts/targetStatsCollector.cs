using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetStatsCollector : MonoBehaviour
{
    Vector3 lastUpdatePos;
    public Vector3 targetMoveDirection { get; private set; }
    public float targetMoveAngle { get => Gubernia502.angleFromDirection(targetMoveDirection); }
    float dirCollectorSensivity = 0.05f;
    private void OnDisable()
    {
        targetMoveDirection = Vector3.zero;
    }
    private void Update()
    {
        if (Vector3.Distance( transform.position , lastUpdatePos)<dirCollectorSensivity)
        {
            targetMoveDirection = Vector3.zero;
        }
        else
        {
            targetMoveDirection = (transform.position - lastUpdatePos).normalized;
            lastUpdatePos = transform.position;
        }
    }
}
