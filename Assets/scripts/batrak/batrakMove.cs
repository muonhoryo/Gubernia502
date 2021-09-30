using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class batrakMove : MonoBehaviour
{
    public Gubernia502.getFloatFun moveSpeed { get; private set; }
    public CapsuleCollider batrakCollider;
    public float sphereCastRadius { get; private set; }
    public batrakBehavior batrakBehavior;
    private Gubernia502.simpleFun onDisable=delegate() { };
    private Gubernia502.simpleFun updateMove;
    [SerializeReference]
    private Vector3 MoveTarget;
    public Vector3 moveTarget
    {
        get => MoveTarget;
        set
        {
            MoveTarget = value;
            enabled = true;
            neededDirection = (value - transform.position).normalized;
        }
    }
    private Vector3 NeededDirection = Vector3.zero;
    public Vector3 neededDirection
    {
        get => NeededDirection;
        set
        {
            NeededDirection = value;
            batrakBehavior.bodyRotateScript.neededDirectionAngle = Gubernia502.angleFromDirection(value);
        }
    }
    [SerializeField]
    private Vector3 DirectionMove = Vector3.zero;
    public float directionAngle
        {
        set
        {
            DirectionMove = Gubernia502.directionFromAngle(value);
        }
    }
    private void DISsetDefaultSpeed()
    {
        moveSpeed = delegate () { return batrakBehavior.currentState.moveSpeed(); };
    }
    private void DISsetDefaultMove()
    {
        updateMove=move;
    }
    public void setDefaultSettings()
    {
        onDisable();
        onDisable = delegate () { };
    }
    public void setSpeed(bool isDesactiveMove=false)
    {
        if (isDesactiveMove)
        {
            moveSpeed = delegate () { return 0; };
            onDisable += DISsetDefaultSpeed;
        }
        else
        {
            DISsetDefaultSpeed();
            onDisable -= DISsetDefaultSpeed;
        }
    }
    public void setTargetMove()
    {
        OnDisable();
        updateMove = delegate () 
        {
            if (batrakBehavior.targetEnemy != null)
            {
                moveTarget = batrakBehavior.targetEnemy.transform.position;
                neededDirection = (moveTarget - transform.position).normalized;
            }
        };
        updateMove +=move;
        onDisable += DISsetDefaultMove;
    }
    private bool searchMoveble()
    {
        Vector3 rayStartPos = new Vector3(transform.position.x, moveTarget.y, transform.position.z);
        return !Physics.SphereCast(new Ray(rayStartPos, (moveTarget - rayStartPos).normalized), sphereCastRadius,
            Vector3.Distance(moveTarget, rayStartPos), 512, QueryTriggerInteraction.Ignore);
    }
    public Vector3 setMoveTraectory()
    {
        RaycastHit hit;
        float MoveSpd = moveSpeed();
        if (Physics.SphereCast(transform.position, sphereCastRadius , DirectionMove,
            out hit, MoveSpd, 8704,QueryTriggerInteraction.Ignore))
        {
            if (hit.distance > MoveSpd)
            {
                return DirectionMove * MoveSpd;
            }
            else
            {
                if (hit.distance > Gubernia502.constData.moveMinHitDistance)
                {
                    return DirectionMove * (hit.distance - Gubernia502.constData.moveMinHitDistance);
                }
                else
                {
                    return Vector3.zero;
                }
            }
        }
        return DirectionMove * MoveSpd;
    }
    private void move()
    {
        if (!searchMoveble())
        {
            batrakBehavior.currentState.onCantMoveToPoint(batrakBehavior);
            batrakBehavior.batrakAnim.SetFloat("Move", 0);
            neededDirection = (moveTarget - transform.position).normalized;
            return;
        }
        Vector3 dir = setMoveTraectory();
        if(dir != Vector3.zero)
        {
            if (Vector3.Distance(moveTarget, transform.position) > moveSpeed() + batrakBehavior.currentState.moveDoneDistance())
            {
                transform.position += dir;
                batrakBehavior.batrakAnim.SetFloat("Move", 1);
            }
            else
            {
                enabled = false;
                if (batrakBehavior.currentState.moveDoneDistance() <= 0)
                {
                    transform.position = moveTarget;
                    batrakBehavior.onMovePointDone();
                }
                else
                {
                    transform.position += dir;
                    batrakBehavior.currentState.onBodyMoveDone(batrakBehavior);
                }
            }
        }
        else
        {
            batrakBehavior.batrakAnim.SetFloat("Move", 0);
        }
        neededDirection = (moveTarget - transform.position).normalized;
    }
    private void OnDisable()
    {
        batrakBehavior.batrakAnim.SetFloat("Move", 0);
        onDisable();
        onDisable = delegate () { };
    }
    private void Update()
    {
        updateMove();
    }
    private void Awake()
    {
        sphereCastRadius = batrakCollider.radius * transform.localScale.y;
        DISsetDefaultMove();
    }
    private void Start()
    {
        DISsetDefaultSpeed();
    }
}
