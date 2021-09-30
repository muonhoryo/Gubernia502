using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakMove : MonoBehaviour
{
    public bool isActive = true;
    public ermakLockControl ermakLockControl;
    public Animator ermakAnim;
    public int animModifier=0;
    private Vector3 MoveTraectory;
    public Vector3 moveTraectory 
    {
        get => MoveTraectory;
        set
        {
            if (isActive)
            {
                enabled = true;
                MoveTraectory = setMoveTraectory(value*Time.fixedDeltaTime);
            }
        }
    }
    public CapsuleCollider hitBox;
    public float colliderRadius;
    private void move()
    {
        transform.Translate(MoveTraectory);
    }
    private void zAxisSphereCast(Vector3 rayStartPos,float colliderRadius,Vector3 direction,float rayMaxDistance,
                                 ref Vector3 newDirection,float minHitDistance)
    {
        RaycastHit hit;
        if (Physics.SphereCast(rayStartPos, colliderRadius,new Vector3(0, 0, direction.z),out hit,
            rayMaxDistance, 8960,QueryTriggerInteraction.Ignore))
        {
            if (hit.distance > minHitDistance)
            {
                if (newDirection.z > 0)
                {
                    newDirection.z = hit.distance-minHitDistance;
                }
                else
                {
                    newDirection.z = -hit.distance+minHitDistance;
                }
            }
            else
            {
                newDirection.z = 0;
            }
        }
    }
    private void xAxisSphereCast(Vector3 rayStartPos, float colliderRadius, Vector3 direction, float rayMaxDistance,
                                 ref Vector3 newDirection, float minHitDistance )
    {
        RaycastHit hit;
        if (Physics.SphereCast(rayStartPos, colliderRadius,new Vector3(direction.x, 0, 0), out hit,
            rayMaxDistance, 8960,QueryTriggerInteraction.Ignore))
        {
            if (hit.distance > minHitDistance)
            {
                if (newDirection.x > 0)
                {
                    newDirection.x = hit.distance-minHitDistance;
                }
                else
                {
                    newDirection.x = -hit.distance+minHitDistance;
                }
            }
            else
            {
                newDirection.x = 0;
            }
        }
    }
    public Vector3 setMoveTraectory(Vector3 direction)
    {
        float rayMaxDistance = Gubernia502.constData.ermakMoveSpeed*Time.fixedDeltaTime;
        Vector3 newDirection = direction* Gubernia502.constData.ermakMoveSpeed;
        Vector3 rayStartPos = new Vector3(transform.position.x,
                                          0f,
                                          transform.position.z);
        if (direction.x != 0 && direction.z != 0 )
        {
            RaycastHit hit;
            if (Physics.SphereCast(rayStartPos, colliderRadius, new Vector3(direction.x, 0, direction.z),
                                                               out hit,rayMaxDistance, 8960,QueryTriggerInteraction.Ignore))
            {
                if (hit.distance <= Gubernia502.constData.moveMinHitDistance)
                {
                    zAxisSphereCast(rayStartPos, colliderRadius, direction, rayMaxDistance, ref newDirection,
                        Gubernia502.constData.moveMinHitDistance);
                    xAxisSphereCast(rayStartPos, colliderRadius, direction, rayMaxDistance, ref newDirection,
                        Gubernia502.constData.moveMinHitDistance);
                }
                else
                {
                    return direction * (hit.distance - Gubernia502.constData.moveMinHitDistance);
                }
            }
            else
            {
                return newDirection;
            }
        }
        else
        {
            if (direction.z != 0)
            {
                zAxisSphereCast(rayStartPos, colliderRadius, direction, rayMaxDistance, ref newDirection,
                    Gubernia502.constData.moveMinHitDistance);
            }
            if (direction.x != 0)
            {
                xAxisSphereCast(rayStartPos, colliderRadius, direction, rayMaxDistance, ref newDirection,
                    Gubernia502.constData.moveMinHitDistance);
            }
        }
        return newDirection;
    }
    void Update()
    {
        if (moveTraectory != Vector3.zero)
        {
            move();
            if (moveTraectory.z > 0)
            {
                if (moveTraectory.x > 0)
                {
                    ermakAnim.SetFloat("move", (9 - animModifier) % 8);
                }//Forward-Right
                else if (moveTraectory.x < 0)
                {
                    ermakAnim.SetFloat("move", (15 - animModifier) % 8);
                }//Forward-Left
                else
                {
                    ermakAnim.SetFloat("move", (8 - animModifier) % 8);
                }//Forward
            }
            else if (moveTraectory.z < 0)
            {
                if (moveTraectory.x > 0)
                {
                    ermakAnim.SetFloat("move", (11 - animModifier) % 8);
                }//Backward-Right
                else if (moveTraectory.x < 0)
                {
                    ermakAnim.SetFloat("move", (13 - animModifier) % 8);
                }//Backward-Left
                else
                {
                    ermakAnim.SetFloat("move", (12 - animModifier) % 8);
                }//Backward
            }
            else
            {
                if (moveTraectory.x > 0)
                {
                    ermakAnim.SetFloat("move", (10 - animModifier) % 8);
                }//RIGHT
                else if (moveTraectory.x < 0)
                {
                    ermakAnim.SetFloat("move", (14 - animModifier) % 8);
                }//LEFT
            }
            ermakLockControl.soundGenerator.soundLevel = Gubernia502.constData.ermakMoveSoundVolume;
            MoveTraectory = Vector3.zero;
        }
        else
        {
            enabled = false;
            ermakAnim.SetFloat("move", 8); 
        }
    }
    private void Awake()
    {
        colliderRadius = hitBox.radius * transform.localScale.y;
    }
}
