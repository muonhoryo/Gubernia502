using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class batrakMove : MonoBehaviour
{
    public int APathFindDepthLevel = 3;

    public Gubernia502.pathFindThreadState pathFindThreadState { get; private set; } = null;
    public CapsuleCollider batrakCollider;
    public float sphereCastRadius { get; private set; }
    public float moveDoneDistance = 0f;
    [SerializeReference]
    private int currentPathMoveIndex=0;
    float moveSpeedConst;
    [SerializeField]
    float MoveSpeed = 0.005f;
    public batrakBehavior batrakBehavior;
    private Gubernia502.simpleFun onDisable;
    private Gubernia502.simpleFun onMoveDone;
    private Gubernia502.simpleFun updateMove;
    private Gubernia502.simpleFun moveAction;
    private Vector3 nextMovetarget;
    private bool isNextTarget=false;
    [SerializeReference]
    private Vector3 moveTargetPos;
    [SerializeReference]
    private List<Vector3> MoveTargets = new List<Vector3>();
    public Vector3 moveTarget
    {
        get => MoveTargets[currentPathMoveIndex];
        set
        {
            Vector3 target = new Vector3(value.x, 0, value.z);
            Vector3 targetDirection = (target - transform.position).normalized;
            if (!Physics.SphereCast(new Ray(new Vector3(transform.position.x, 0, transform.position.z),
                                       targetDirection), sphereCastRadius, out RaycastHit hit,
                                       Vector3.Distance(target, transform.position)-sphereCastRadius,
                                       512,QueryTriggerInteraction.Ignore))//есть прямой путь к цели
            {
                if (pathFindThreadState != null)
                {
                    abortCurrentThread();
                }
                if (MoveTargets.Count > 1)
                {
                    disableOnPathMove();
                }
                else
                {
                    MoveTargets[0] = target;
                }
                moveTargetPos = target;
                neededDirection = (target - transform.position).normalized;
                enabled = true;
            }
            else//нет прямого пути к цели
            {
                if (pathFindThreadState == null)
                {
                    if (MoveTargets.Count > 1)
                    {
                        List<Vector3> newPath = Gubernia502.economPathFinding(MoveTargets, target, transform.position);
                        if (newPath == null)
                        {
                            startFirstPathFind(transform.position, target);
                        }
                        else
                        {
                            MoveTargets = newPath;
                            moveTargetPos = newPath[newPath.Count - 1];
                            neededDirection = (target - transform.position).normalized;
                            enabled = true;
                        }
                    }
                    else
                    {
                        startFirstPathFind(transform.position, target);
                    }
                }
                else
                {
                    nextMovetarget = target;
                    isNextTarget = true;
                }
            }
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
    public void setMoveTarget(Vector3 value)
    {
        isNextTarget = false;
        if (pathFindThreadState != null)
        {
            abortCurrentThread();
        }
        Vector3 target = new Vector3(value.x, transform.position.y, value.z);
        Vector3 targetDirection = (target - transform.position).normalized;
        if (!Physics.SphereCast(new Ray(new Vector3(transform.position.x, 0.1f, transform.position.z),
                                   targetDirection), sphereCastRadius,
                                   Vector3.Distance(target, transform.position), 512,QueryTriggerInteraction.Ignore))
        {
            if (MoveTargets.Count > 1)
            {
                disableOnPathMove();
            }
            MoveTargets[0] = target;
            moveTargetPos = target;
            neededDirection = (target - transform.position).normalized;
        }
        else
        {
            if (MoveTargets.Count > 1)
            {
                List<Vector3> newPath = Gubernia502.economPathFinding(MoveTargets, target, transform.position);
                if (newPath == null)
                {
                    startFirstPathFind(transform.position, target);
                }
                else
                {
                    MoveTargets = newPath;
                    moveTargetPos = newPath[newPath.Count - 1];
                    neededDirection = (target - transform.position).normalized;
                }
            }
            else
            {
                startFirstPathFind(transform.position, target);
            }
        }
    }
    private void abortCurrentThread()
    {
        setMoveAction();
        if (pathFindThreadState != null)
        {
            if (pathFindThreadState.temporalStart != null)
            {
                Destroy(pathFindThreadState.temporalStart.gameObject);
            }
            if (pathFindThreadState.temporalEnd != null)
            {
                Destroy(pathFindThreadState.temporalEnd.gameObject);
            }
            pathFindThreadState.currentThread.Abort();
            pathFindThreadState = null;
        }
    }
    private void startPathFindThread(Vector3 start,Vector3 end)
    {
        pathFindThreadState = new Gubernia502.pathFindThreadState(start, end);
        pathFindThreadState.currentThread= new Thread(new ThreadStart(pathFindThreadState.threadDextraPathFind));
        pathFindThreadState.currentThread.Start();
    }
    private void startFirstPathFind(Vector3 start,Vector3 end)
    {
        if (enabled)
        {
            setParalellAction();
        }
        else
        {
            setWaitAction();
            enabled = true;
        }
        startPathFindThread(start, end);
    }
    private void startSecondPathFind(Vector3 start,Vector3 end)
    {
        setWaitAction();
        enabled = true;
        startPathFindThread(start, end);
    }
    private void disableOnPathMove(Vector3 zeroMoveTarget)
    {
        MoveTargets = new List<Vector3> { zeroMoveTarget };
        currentPathMoveIndex = 0;
        moveDoneDistance = 0f;
        batrakBehavior.batrakAnim.SetFloat("Move", 0);
        setDefaultMove();
        setDeafaulOnDisable();
        onMoveDone = defaultOnMoveDone;
    }
    private void disableOnPathMove()
    {
        disableOnPathMove(new Vector3(transform.position.x, 0, transform.position.z));
    }
    private void disableOnWaitPathFind()
    {
        abortCurrentThread();
    }
    public void setSpeed(bool isDesactiveMove=false)
    {
        if (isDesactiveMove)
        {
            MoveSpeed = 0;
        }
        else
        {
            MoveSpeed = moveSpeedConst;
        }
    }
    public void setPeacefulSpeed()
    {
        MoveSpeed= moveSpeedConst = Gubernia502.constData.batrakPassiveMoveSpeed;
    }
    public void setAggressiveSpeed()
    {
        MoveSpeed=moveSpeedConst = Gubernia502.constData.batrakActiveMoveSpeed;
    }
    private void setDisableOnTargetMove()
    {
        onDisable = delegate ()
        {
            moveDoneDistance = 0f;
            batrakBehavior.batrakAnim.SetFloat("Move", 0);
            setDefaultMove();
        };
    }
    private void setDeafaulOnDisable()
    {
        onDisable = delegate ()
        {
            moveDoneDistance = 0f;
            batrakBehavior.batrakAnim.SetFloat("Move", 0);
        };
    }
    private void defaultOnMoveDone()
    {
        enabled = false;
        batrakBehavior.onRotateMoveDone();
    }
    private void onMoveDoneOnPath()
    {
        disableOnPathMove();
        defaultOnMoveDone();
    }
    private void onMoveDoneOnWaitPath()
    {
        onDisable -= disableOnWaitPathFind;
        setWaitAction();
        onMoveDone = defaultOnMoveDone;
    }
    private void setMoveOnPath()
    {
        onDisable = disableOnPathMove;
        onMoveDone = onMoveDoneOnPath;
        updateMove = delegate ()
        {
            moveAction();
        };
    }
    private void setDefaultMove()
    {
        updateMove = delegate ()
         {
             moveAction();
         };
        setDeafaulOnDisable();
    }
    public void setTargetMove()
    {
        updateMove = delegate () 
        {
            if (batrakBehavior.targetEnemy != null)
            {
                moveTarget = batrakBehavior.targetEnemy.transform.position;
            }
        };
        updateMove +=delegate ()
        {
            moveAction();
        };
        setDisableOnTargetMove();
    }
    private bool searchMoveble()
    {
        Vector3 rayStartPos = new Vector3(transform.position.x, moveTarget.y, transform.position.z);
        if (Physics.SphereCast(new Ray(rayStartPos,(moveTarget - rayStartPos).normalized), sphereCastRadius,
            Vector3.Distance(moveTarget, rayStartPos), 512,QueryTriggerInteraction.Ignore))
        {
            if (currentPathMoveIndex <= 0)
            {
                moveTarget = moveTargetPos;
                return false;
            }
            currentPathMoveIndex--;
            return false;
        }
        return true;
    }
    /*private void addIntermediantePath()
    {
        List<Vector3> newPath = Gubernia502.APathFinding(MoveTargets[0], transform.position, APathFindDepthLevel);
        if (newPath == null)
        {
            batrakBehavior.onPathNotFound();
        }
        else
        {
            newPath.RemoveAt(newPath.Count - 1);
            newPath.AddRange(MoveTargets);
            MoveTargets = newPath;
        }
    }*/
    public Vector3 setMoveTraectory()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereCastRadius , DirectionMove,
            out hit, MoveSpeed, 8704,QueryTriggerInteraction.Ignore))
        {
            if (hit.distance > MoveSpeed)
            {
                return DirectionMove * MoveSpeed;
            }
            else
            {
                if (hit.distance > Gubernia502.constData.moveMinRayHitDistance)
                {
                    return DirectionMove * (hit.distance - Gubernia502.constData.moveMinRayHitDistance);
                }

                return Vector3.zero;
            }
        }
        return DirectionMove * MoveSpeed;
    }
    private void setMoveAction()
    {
        moveAction = move;
    }
    private void setWaitAction()
    {
        onDisable += disableOnWaitPathFind;
        moveAction = waitPathFind;
    }
    private void setParalellAction()
    {
        onMoveDone = onMoveDoneOnWaitPath;
        onDisable += disableOnWaitPathFind;
        moveAction = waitPathFind;
        moveAction += move;
    }
    private void move()
    {
        if (!searchMoveble())
        {
            batrakBehavior.batrakAnim.SetFloat("Move", 0);
            neededDirection = (moveTarget - transform.position).normalized;
            return;
        }
        Vector3 dir = setMoveTraectory();
        if(dir != Vector3.zero)
        {
            if (Vector3.Distance(moveTarget, transform.position) > moveSpeedConst + moveDoneDistance)
            {
                transform.position += dir;
                batrakBehavior.batrakAnim.SetFloat("Move", 1);
            }
            else
            {
                if (moveDoneDistance == 0)
                {
                    transform.position = MoveTargets[currentPathMoveIndex];
                }
                else
                {
                    transform.position += dir;
                }
                if (MoveTargets.Count == 1||currentPathMoveIndex>=MoveTargets.Count-1)
                {
                    onMoveDone();
                }
                else
                {
                    currentPathMoveIndex++;
                    neededDirection = (moveTarget - transform.position).normalized;
                }
                return;
            }
        }
        else
        {
            batrakBehavior.batrakAnim.SetFloat("Move", 0);
        }
        neededDirection = (moveTarget - transform.position).normalized;
    }
    private void waitPathFind()
    {
        if (pathFindThreadState == null)
        {
            onDisable -= disableOnWaitPathFind;
            if (isNextTarget)
            {
                setMoveTarget(nextMovetarget);
            }
            else
            {
                setMoveAction();
                pathFindThreadState = null;
            }
            return;
        }
        if (pathFindThreadState.pathFindDone)
        {
            if (pathFindThreadState.finalPath == null)//нет пути до точки
            {
                batrakBehavior.onPathNotFound();
            }
            else//есть путь до точки
            {
                if (MoveTargets.Count <= 1)//в первый раз выставляется движение по пути
                {
                    if (onDisable != disableOnPathMove)
                    {
                        batrakBehavior.setMoveOnPath();
                        setMoveOnPath();
                    }
                }
                Vector3 target;
                bool pathIsActual = false;
                for (int i = pathFindThreadState.finalPath.Count - 1; i >= 0; i--)
                {
                    target = new Vector3(pathFindThreadState.finalPath[i].x, transform.position.y, pathFindThreadState.finalPath[i].z);
                    if (!Physics.SphereCast(new Ray(new Vector3(transform.position.x, 0.1f, transform.position.z),
                                           (target - transform.position).normalized), sphereCastRadius,
                                           Vector3.Distance(target, transform.position), 512,QueryTriggerInteraction.Ignore))
                    {
                        currentPathMoveIndex = i;
                        pathIsActual = true;
                        break;
                    }
                }
                if (pathIsActual)
                {
                    MoveTargets = pathFindThreadState.finalPath;
                    moveTargetPos = pathFindThreadState.finalPath[pathFindThreadState.finalPath.Count - 1];
                    neededDirection = (MoveTargets[currentPathMoveIndex] - transform.position).normalized;
                    enabled = true;
                }
                else
                {
                    onDisable -= disableOnWaitPathFind;
                    startSecondPathFind(transform.position, pathFindThreadState.finalPath[pathFindThreadState.finalPath.Count - 1]);
                    return;
                }
            }
            if (pathFindThreadState != null)
            {
                pathFindThreadState.currentThread.Abort();
            }
            onDisable -= disableOnWaitPathFind;
            if (isNextTarget)
            {
                setMoveTarget(nextMovetarget);
            }
            else
            {
                setMoveAction();
                pathFindThreadState = null;
            }
        }
    }
    private void OnDisable()
    {
        onDisable();
    }
    private void LateUpdate()
    {
        updateMove();
    }
    private void Awake()
    {
        sphereCastRadius = batrakCollider.radius * transform.localScale.y;
        MoveTargets.Add(Vector3.zero);
        setDeafaulOnDisable();
        onMoveDone = defaultOnMoveDone;
        setMoveAction();
        setDefaultMove();
    }
    private void Start()
    {
        moveSpeedConst = Gubernia502.constData.batrakPassiveMoveSpeed;
    }
}
