using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class mobBehavior : MonoBehaviour
{
    public static class mobBehaviorStateMachine
    {
        public static void SRonDeath(mobBehavior owner, float hitAngle)
        {
            owner.enemyDirection = hitAngle;
            owner.currentState = BSdeath;
        }
        public static void SRonGetStun(mobBehavior owner, float hitAngle)
        {
            owner.bodyRotateScript.rotatedBody.rotation = Quaternion.Euler(0f, hitAngle, 0f);
            owner.enemyDirection = hitAngle;
            owner.currentState = BSstun;
        }
        public static void SRonPassiveStateEnter(mobBehavior owner)
        {
            if (owner.isAggressive)
            {
                owner.disableMoveScripts();
                owner.isAggressive = false;
                owner.animator.SetBool("isAggressive", false);
            }
        }
        public static void SRonAggressiveStateEnter(mobBehavior owner)
        {
            if (!owner.isAggressive)
            {
                owner.disableMoveScripts();
                owner.isAggressive = true;
                owner.animator.SetBool("isAggressive", true);
            }
        }
        public static void SRonPatrulMoveDone(mobBehavior owner)
        {
            if (owner.patrulPointNum == owner.patrulPoint.Count)
            {
                owner.patrulPointNum = 0;
            }
            owner.setMoveTarget(owner.patrulPoint[owner.patrulPointNum++]);
        }
        public static void onCantMoveToPoint(mobBehavior owner)
        {
            if (owner.currentPathMoveIndex <= 0)
            {
                owner.moveScript.enabled = false;
                owner.setMoveTarget(owner.moveTarget);
            }
            else
            {
                owner.moveScript.moveTarget = owner.pathMove[--owner.currentPathMoveIndex];
            }
        }
        public static void SRonHuntCantMove(mobBehavior owner)
        {
            owner.setMoveTarget(owner.targetEnemy.transform.position);
            owner.currentState = BSmoveToEnemyPosition;
        }
        public delegate void RAsimple(mobBehavior owner);
        public delegate void RAonTakeDamage(mobBehavior owner, float hitAngle);
        public delegate void RAonTakeEnemyPosition(mobBehavior owner, Vector3 position);
        public delegate void RAonFoundEnemy(mobBehavior owner, hitPointSystem targetEnemy);
        public static readonly RAonTakeDamage emptyTakeDmg = delegate (mobBehavior i, float j) { };
        public static readonly RAonFoundEnemy emptyFoundEnemy = delegate (mobBehavior i, hitPointSystem j) { };
        public static readonly RAonTakeEnemyPosition emptyTakePos = delegate (mobBehavior i, Vector3 j) { };
        public static readonly RAsimple emptySimple = delegate (mobBehavior i) { };
        public static readonly Gubernia502.getFloatFun getZero = delegate () { return 0; };
        public static readonly Gubernia502.getFloatFun getPassiveMoveSpeed =
            delegate () { return Gubernia502.constData.mobPassiveMoveSpeed; };
        public static readonly Gubernia502.getFloatFun getPassiveRotSpeed =
            delegate () { return Gubernia502.constData.mobPassiveRotationSpeed; };
        public static readonly Gubernia502.getFloatFun getActiveMoveSpeed =
            delegate () { return Gubernia502.constData.mobActiveMoveSpeed; };
        public static readonly Gubernia502.getFloatFun getActiveRotSpeed =
            delegate () { return Gubernia502.constData.mobActiveRotationSpeed; };
        public static readonly Gubernia502.getFloatFun getOnAttackMoveDoneDis =
            delegate () { return Gubernia502.constData.mobOnAttackMoveDoneDistance; };
        public static primBehaviorState BSdeath { get; private set; } = new primBehaviorState
            (
                emptyTakeDmg,
                emptyTakeDmg,
                emptyTakeDmg,
                emptyTakePos,
                emptyTakePos,
                emptyFoundEnemy,
                emptySimple,
                emptySimple,
                emptySimple,
                delegate (mobBehavior owner)
                {
                    GameObject.Destroy(owner.moveScript.mobCollider);
                    owner.disableMoveScripts();
                    owner.StartCoroutine(owner.idleDelay(Gubernia502.constData.mobTimeToDeath,
                        delegate () { GameObject.Destroy(owner.gameObject); }));
                    if (Gubernia502.differenceOf2Angle(owner.enemyDirection, owner.bodyRotateScript.rotatedBody.eulerAngles.y) > 90)
                    {
                        owner.animator.SetInteger("stan", 4);
                    }
                    else
                    {
                        owner.animator.SetInteger("stan", 3);
                    }
                },
                emptySimple,
                emptySimple,
                getZero,
                getZero,
                getZero,
                "BSdeath"
            );
        public static primBehaviorState BSstun { get; private set; } = new primBehaviorState
        (
                SRonDeath,
                SRonGetStun,//on get stun
                emptyTakeDmg,//on take damage
                emptyTakePos,//on hear sound
                emptyTakePos,//on lost sound
                emptyFoundEnemy,//on found enemy
                emptySimple,//on lost enemy
                emptySimple,//on path not found
                delegate (mobBehavior owner)//on body move done
                {
                    if (owner.targetEnemy != null)
                    {
                        owner.currentState = BShunt;
                    }
                    else if (owner.soundHear.soundHears.Count > 0)
                    {
                        owner.currentState = BSmoveToSoundPosition;
                    }
                    else
                    {
                        owner.currentState = BSfindEnemy;
                    }
                },
                delegate (mobBehavior owner)//on state enter
                {
                    owner.disableMoveScripts();
                    owner.animator.SetInteger("stan", 2);
                    if (!owner.isAggressive)
                    {
                        owner.isAggressive = true;
                        owner.animator.SetBool("isAggressive", true);
                    }
                },
                emptySimple,
                onCantMoveToPoint,
                getZero,
                getZero,
                getZero,
                "BSstun"
        );
        public static primBehaviorState BSpotato { get; private set; } = new primBehaviorState
            (
                SRonDeath,
                delegate (mobBehavior owner, float hitAngle)
                {
                    owner.animator.SetInteger("stan", 2);
                    owner.bodyRotateScript.rotatedBody.rotation = Quaternion.Euler(0f, hitAngle, 0f);
                },
                emptyTakeDmg,
                emptyTakePos,
                emptyTakePos,
                emptyFoundEnemy,
                emptySimple,
                emptySimple,
                emptySimple,
                SRonPassiveStateEnter,
                emptySimple,
                onCantMoveToPoint,
                getPassiveMoveSpeed,
                getPassiveRotSpeed,
                getZero,
                "BSpotato"
            );
        public static primBehaviorState BSpatrul { get; private set; } = new primBehaviorState
                (
                    SRonDeath,
                    SRonGetStun,
                    delegate (mobBehavior owner, float hitAngle)//on take damage
                {
                        owner.enemyDirection = hitAngle;
                        owner.currentState = BSactivizationTakeDmg;
                    },
                    delegate (mobBehavior owner, Vector3 soundPosition)//on hear sound
                {
                        owner.enemyLastPosition = soundPosition;
                        owner.currentState = BSactivizationHearSound;
                    },
                    emptyTakePos,//on lost heared sound
                    delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
                {
                        owner.currentState = BSactivizationFoundEnemy;
                    },
                    emptySimple,//on lost visible enemy
                    delegate (mobBehavior owner)//on path not found
                {
                        owner.currentState = BSstayOnPoint;
                    },
                    SRonPatrulMoveDone,//on body move done
                    delegate (mobBehavior owner)//on state enter
                {
                        SRonPassiveStateEnter(owner);
                        SRonPatrulMoveDone(owner);
                    },
                    emptySimple,//on state exit
                    onCantMoveToPoint,
                    getPassiveMoveSpeed,
                    getPassiveRotSpeed,
                    getZero,
                    "BSpatrul"
                );
        public static primBehaviorState BSstayOnPoint { get; private set; } = new primBehaviorState
                (
                    SRonDeath,
                    SRonGetStun,
                    delegate (mobBehavior owner, float hitAngle)//on take damage
                {
                        owner.enemyDirection = hitAngle;
                        owner.currentState = BSactivizationTakeDmg;
                    },
                    delegate (mobBehavior owner, Vector3 soundPosition)//on hear sound
                {
                        owner.enemyLastPosition = soundPosition;
                        owner.currentState = BSactivizationHearSound;
                    },
                    emptyTakePos,//on lost heared sound
                    delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
                {
                        owner.currentState = BSactivizationFoundEnemy;
                    },
                    emptySimple,//on lost visible enemy
                    delegate (mobBehavior owner)//on path not found
                {
                        owner.currentState = BSstayOnPoint;
                        owner.StartCoroutine(owner.idleDelay(owner.repeatPathFindDelay,
                            delegate () { owner.setMoveTarget(owner.patrulPoint[0]); }));
                    },
                    emptySimple,//on body move done
                    delegate (mobBehavior owner)//on state enter
                {
                        SRonPassiveStateEnter(owner);
                        if (owner.patrulPoint.Count == 0)
                        {
                            owner.patrulPoint = new List<Vector3> { owner.transform.position };
                        }
                        else
                        {
                            owner.setMoveTarget(owner.patrulPoint[0]);
                            if (owner.patrulPoint.Count > 1)
                            {
                                owner.patrulPoint = new List<Vector3> { owner.patrulPoint[0] };
                            }
                        }
                    },
                    emptySimple,
                    onCantMoveToPoint,
                    getPassiveMoveSpeed,
                    getPassiveRotSpeed,
                    getZero,
                    "BSstayOnPoint"
                );
        public static primBehaviorState BSactivizationTakeDmg { get; private set; } = new primBehaviorState
                (
                    SRonDeath,
                    SRonGetStun,
                    delegate (mobBehavior owner, float hitAngle)//on take damage
                {
                        owner.enemyDirection = hitAngle;
                    },
                    delegate (mobBehavior owner, Vector3 soundPosition)//on hear sound
                {
                        owner.enemyLastPosition = soundPosition;
                        owner.currentState = BSactivizationHearSound;
                    },
                    emptyTakePos,//on lost heared sound
                    delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
                {
                        owner.currentState = BSactivizationFoundEnemy;
                    },
                    emptySimple,//on lost visible enemy
                    emptySimple,//on path not found
                    delegate (mobBehavior owner)//on body move done
                {
                        owner.currentState = BSfindEnemy;
                    },
                    SRonAggressiveStateEnter,//on state enter
                    emptySimple,//on state exit
                    onCantMoveToPoint,
                    getZero,
                    getZero,
                    getZero,
                    "BSonActivizationTakeDmg"
                );
        public static primBehaviorState BSactivizationHearSound { get; private set; } = new primBehaviorState
                (
                    SRonDeath,
                    SRonGetStun,
                    emptyTakeDmg,//on take dmg
                    delegate (mobBehavior owner, Vector3 soundPosition)//on hear sound
                {
                        owner.enemyLastPosition = soundPosition;
                    },
                    delegate (mobBehavior owner, Vector3 soundPosition)//on lost heared sound
                {
                        owner.enemyLastPosition = soundPosition;
                    },
                    delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
                {
                        owner.currentState = BSactivizationFoundEnemy;
                    },
                    emptySimple,//on lost visible enemy
                    emptySimple,//on path not found
                    delegate (mobBehavior owner)//on body move done
                {
                        owner.currentState = BSmoveToSoundPosition;
                    },
                    SRonAggressiveStateEnter,//on state enter
                    emptySimple,//on state exit
                    onCantMoveToPoint,
                    getZero,
                    getZero,
                    getZero,
                    "BSonActivizationHearSoundDmg"
                );
        public static primBehaviorState BSactivizationFoundEnemy { get; private set; } = new primBehaviorState
                (
                    SRonDeath,
                    SRonGetStun,
                    emptyTakeDmg,//on take dmg
                    emptyTakePos,//on hear sound
                    emptyTakePos,//on lost heared sound
                    emptyFoundEnemy,//on found enemy
                    delegate (mobBehavior owner)//on lost visible enemy
                {
                        owner.currentState = BSactivizationHearSound;
                    },
                    emptySimple,//on path not found
                    delegate (mobBehavior owner)//on body move done
                {
                        owner.currentState = BShunt;
                    },
                    SRonAggressiveStateEnter,//on state enter
                    emptySimple,//on state exit
                    onCantMoveToPoint,
                    getZero,
                    getZero,
                    getZero,
                    "BSonActivizationFoundEnemy"
                );
        public static primBehaviorState BSfindEnemy { get; private set; } = new primBehaviorState
                (
                    SRonDeath,
                    SRonGetStun,
                    delegate (mobBehavior owner, float hitAngle)//on take dmg
                {
                        owner.enemyDirection = hitAngle;
                        owner.bodyRotateScript.neededDirectionAngle = owner.enemyDirection;
                    },
                    delegate (mobBehavior owner, Vector3 soundPosition)//on hear sound
                {
                        owner.enemyLastPosition = soundPosition;
                        owner.currentState = BSmoveToSoundPosition;
                    },
                    emptyTakePos,//on lost heared sound
                    delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
                {
                        owner.currentState = BShunt;
                    },
                    emptySimple,//on lost visible enemy
                    emptySimple,//on path not found
                    delegate (mobBehavior owner)//on body move done
                {
                        owner.currentState = BSsearchTarget;
                    },
                    delegate (mobBehavior owner)//on state enter
                {
                        owner.bodyRotateScript.neededDirectionAngle = owner.enemyDirection;
                        owner.bodyRotateScript.setOnlyRotate();
                    },
                    delegate (mobBehavior owner)//on state exit
                {
                        owner.bodyRotateScript.setRotateForMove();
                    },
                    onCantMoveToPoint,
                    getZero,
                    getActiveMoveSpeed,
                    getZero,
                    "BSfindEnemy"
                );
        public static primBehaviorState BSsearchTarget { get; private set; } = new primBehaviorState
                (
                    SRonDeath,
                    SRonGetStun,
                    delegate (mobBehavior owner, float hitAngle)//on take damage
                {
                        owner.enemyDirection = hitAngle;
                        owner.currentState = BSactivizationTakeDmg;
                    },
                    delegate (mobBehavior owner, Vector3 soundPosition)//on hear sound
                {
                        owner.enemyLastPosition = soundPosition;
                        owner.currentState = BSmoveToSoundPosition;
                    },
                    emptyTakePos,//on lost heared sound
                    delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
                {
                        owner.currentState = BShunt;
                    },
                    emptySimple,//on lost visible enemy
                    emptySimple,//on path not found
                    delegate (mobBehavior owner)//on body move done
                {
                        owner.StartCoroutine(owner.idleDelay(Gubernia502.constData.mobPatrulIdleStateTime,
                            owner.setDefaultBehavior));
                    },
                    delegate (mobBehavior owner)//on state enter
                {
                        owner.disableMoveScripts();
                        owner.animator.SetTrigger("search");
                    },
                    emptySimple,//on state exit
                    emptySimple,
                    getZero,
                    getZero,
                    getZero,
                    "BSsearchTarget"
                );
        public static primBehaviorState BSmoveToSoundPosition { get; private set; } = new primBehaviorState
               (
                   SRonDeath,
                   SRonGetStun,//on get stunned
                   emptyTakeDmg,//on take dmg
                   delegate (mobBehavior owner, Vector3 soundPosition)//on hear sound
               {
                       owner.setMoveTarget(owner.enemyLastPosition = soundPosition);
                   },
                   delegate (mobBehavior owner, Vector3 soundPosition)//on lost heared sound
               {
                       owner.setMoveTarget(owner.enemyLastPosition = soundPosition);
                       owner.currentState = BSmoveToLastEnemyPosition;
                   },
                   delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
               {
                       owner.currentState = BShunt;
                   },
                   delegate(mobBehavior owner)
                   {
                       owner.currentState = BSmoveToLastEnemyPosition;
                   },
                   delegate (mobBehavior owner)//on path not found
               {
                       owner.enemyDirection = Gubernia502.angleFromDirection((owner.enemyLastPosition - owner.transform.position).normalized);
                       owner.currentState = BSfindEnemy;
                   },
                   delegate (mobBehavior owner)//on body move done
               {
                       owner.currentState = BSsearchTarget;
                   },
                   delegate (mobBehavior owner)//on state enter
               {
                       owner.setMoveTarget(owner.enemyLastPosition);
                   },
                   emptySimple,
                   onCantMoveToPoint,
                   getActiveMoveSpeed,
                   getActiveRotSpeed,
                   getZero,
                   "BSmoveToSoundPosition"
               );
        public static primBehaviorState BSmoveToLastEnemyPosition { get; private set; } = new primBehaviorState
               (
                   SRonDeath,
                   SRonGetStun,
                   delegate (mobBehavior owner, float hitAngle)//on get stunned
               {
                       owner.enemyDirection = hitAngle;
                       owner.currentState = BSfindEnemy;
                   },
                   delegate (mobBehavior owner, Vector3 soundPosition)//on hear sound
               {
                       owner.enemyLastPosition = soundPosition;
                       owner.currentState = BSmoveToSoundPosition;
                   },
                   delegate (mobBehavior owner, Vector3 soundPosition)//on lost heared sound
                   {
                       owner.enemyLastPosition = soundPosition;
                   },
                   delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
               {
                       owner.currentState = BShunt;
                   },
                   delegate (mobBehavior owner)//on lost visible enemy
               {
                   if (owner.soundHear.soundHears.Count > 0)
                   {
                       owner.currentState = BSmoveToSoundPosition;
                   }
                   else
                   {
                       owner.currentState.onStateEnter(owner);
                   }
                   },
                   delegate (mobBehavior owner)//on path not found
               {
                       owner.enemyDirection = Gubernia502.angleFromDirection((owner.enemyLastPosition - owner.transform.position).normalized);
                       owner.currentState = BSfindEnemy;
                   },
                   delegate (mobBehavior owner)//on body move done
               {
                       owner.currentState = BSsearchTarget;
                   },
                   delegate (mobBehavior owner)//on state enter
               {
                       owner.setMoveTarget(owner.enemyLastPosition);
                   },
                   emptySimple,//on state exit
                   onCantMoveToPoint,
                   getActiveMoveSpeed,
                   getActiveRotSpeed,
                   getZero,
                   "BSmoveToLastEnemyPosition"
               );
        public static primBehaviorState BSmoveToEnemyPosition { get; private set; } = new primBehaviorState
               (
                   SRonDeath,
                   SRonGetStun,//on get stunned
                   emptyTakeDmg,//on take dmg
                   emptyTakePos,//on hear sound
                   emptyTakePos,//on lost sound
                   delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
                   {
                       owner.currentState = BShunt;
                   },
                   delegate (mobBehavior owner)//on lost enemy
                   {
                       if (owner.soundHear.soundHears.Count > 0)
                       {
                           owner.currentState = BSmoveToSoundPosition;
                       }
                       else
                       {
                           owner.currentState = BSmoveToLastEnemyPosition;
                       }
                   },
                   delegate (mobBehavior owner)//on path not found
                   {
                       owner.disableMoveScripts();
                       owner.currentState = BStrackTarget;
                   },
                   delegate (mobBehavior owner)//on body move done
                   {
                       owner.currentState = BShunt;
                   },
                   delegate (mobBehavior owner)//on state enter
                   {
                       owner.setMoveTarget(owner.targetEnemy.transform.position);
                   },
                   emptySimple,
                   onCantMoveToPoint,
                   getActiveMoveSpeed,
                   getActiveRotSpeed,
                   getZero,
                   "BSmoveToEnemyPosition"
               );
        public static primBehaviorState BShunt { get; private set; } = new primBehaviorState
               (
                   SRonDeath,
                   SRonGetStun,//on get stunned
                   emptyTakeDmg,//on take dmg
                   emptyTakePos,//on hear sound
                   emptyTakePos,//on lost heared sound
                   emptyFoundEnemy,//on found enemy
                   delegate (mobBehavior owner)//on lost enemy
               {
                       if (!owner.viewZone.foundNewTarget())
                       {
                           if (owner.soundHear.soundHears.Count > 0)
                           {
                               owner.currentState = BSmoveToSoundPosition;
                           }
                           else
                           {
                               owner.currentState = BSmoveToLastEnemyPosition;
                           }
                       }
                   },
                   delegate (mobBehavior owner)//on path not found
               {
                       owner.disableMoveScripts();
                       owner.currentState = BStrackTarget;
                   },
                   delegate (mobBehavior owner)//on body move done
               {
                       targetStatsCollector targetStats = owner.targetEnemy.GetComponent<targetStatsCollector>();
                       if (targetStats.targetMoveDirection != Vector3.zero &&
                       Mathf.Abs(targetStats.targetMoveAngle - owner.bodyRotateScript.rotatedBody.eulerAngles.y) <
                       Gubernia502.constData.mobJerkMinAngle)
                       {
                           owner.currentState = BSjerk;
                       }
                       else
                       {
                           owner.currentState = BSsimpleAttack;
                       }
                   },
                   delegate (mobBehavior owner)//on state enter
               {
                       Vector3 target = new Vector3(owner.targetEnemy.transform.position.x, owner.transform.position.y, 
                           owner.targetEnemy.transform.position.z);
                       Vector3 targetDirection = (target - owner.transform.position).normalized;
                       if (!Physics.SphereCast(new Ray(owner.transform.position, targetDirection),
                           owner.moveScript.sphereCastRadius, out RaycastHit hit,
                                                  Vector3.Distance(target, owner.transform.position) - owner.moveScript.sphereCastRadius,
                                                  512, QueryTriggerInteraction.Ignore))//есть прямой путь к цели
                   {
                           owner.moveScript.setTargetMove();
                           owner.setVisibleMoveTarget(target);
                       }
                       else
                       {
                       BShunt.onCantMoveToPoint(owner);
                       }
                   },
                   delegate (mobBehavior owner)//on state exit
               {
                       owner.moveScript.setDefaultSettings();
                   },
                   SRonHuntCantMove,
                   getActiveMoveSpeed,
                   getActiveRotSpeed,
                   getOnAttackMoveDoneDis,
                   "BShunt"
               );
        public static primBehaviorState BSsimpleAttack { get; private set; } = new primBehaviorState
                (
                    SRonDeath,
                    SRonGetStun,//on get stunned
                    emptyTakeDmg,//on take dmg
                    emptyTakePos,//on hear sound
                    emptyTakePos,//on lost heared sound
                    delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
                {
                        owner.bodyRotateScript.setTrackTarget();
                    },
                    delegate (mobBehavior owner)//on lost enemy
                {
                        if (!owner.viewZone.foundNewTarget())
                        {
                            owner.bodyRotateScript.enabled = false;
                        }
                    },
                    emptySimple,//on path not found
                    delegate (mobBehavior owner)//on body move done
                {
                        if (owner.targetEnemy != null)
                        {
                            if (Vector3.Distance(owner.transform.position, owner.targetEnemy.transform.position) >
                                Gubernia502.constData.mobMaxSimpleCombatDistance)
                            {
                                owner.currentState = BShunt;
                            }
                            else
                            {
                                owner.currentState.onStateEnter(owner);
                            }
                        }
                        else
                        {
                            owner.currentState = BSmoveToLastEnemyPosition;
                        }
                    },
                    delegate (mobBehavior owner)//on state enter
                {
                        owner.bodyRotateScript.setTrackTarget();
                        owner.soundHear.enabled = false;
                        int punchNum = owner.animator.GetInteger("punchNum");
                        if (punchNum == 2 || punchNum == 0)
                        {
                            owner.meleeShoot.hitBox = owner.rightHand;
                            owner.animator.SetInteger("punchNum", 1);
                        }
                        else
                        {
                            owner.meleeShoot.hitBox = owner.leftHand;
                            owner.animator.SetInteger("punchNum", 2);
                        }
                        owner.animator.SetTrigger("shoot");
                    },
                    delegate (mobBehavior owner)//on state exit
                {
                        owner.moveScript.setDefaultSettings();
                    },
                    SRonHuntCantMove,
                    getActiveMoveSpeed,
                    delegate () { return Gubernia502.constData.mobSimpleAtkRotSpeed; },
                    getOnAttackMoveDoneDis,
                    "BSsimpleAttack"
                );
        public static primBehaviorState BSjerk { get; private set; } = new primBehaviorState
             (
                 SRonDeath,
                 SRonGetStun,//on get stunned
                 emptyTakeDmg,//on take dmg
                 emptyTakePos,//on hear sound
                 emptyTakePos,//on lost heared sound
                 delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
             {
                     owner.bodyRotateScript.setTrackTarget();
                 },
                 delegate (mobBehavior owner)//on lost enemy
             {
                     if (!owner.viewZone.foundNewTarget())
                     {
                         owner.bodyRotateScript.enabled = false;
                     }
                 },
                 emptySimple,//on path not found
                 delegate (mobBehavior owner)//on body move done
             {
                     if (owner.targetEnemy != null)
                     {
                         if (Vector3.Distance(owner.transform.position, owner.targetEnemy.transform.position) >
                             Gubernia502.constData.mobMaxSimpleCombatDistance)
                         {
                             owner.currentState = BShunt;
                         }
                         else
                         {
                             BShunt.onBodyMoveDone(owner);
                         }
                     }
                     else
                     {
                         owner.currentState = BSmoveToLastEnemyPosition;
                     }
                 },
                 delegate (mobBehavior owner)//on state enter
             {
                     owner.meleeDmg = Gubernia502.constData.mobJerkDmg;
                     owner.isStunnedDmg = true;
                     owner.bodyRotateScript.setTrackTarget();
                     owner.soundHear.enabled = false;
                     owner.meleeShoot.hitBox = owner.rightHand;
                     owner.animator.SetInteger("punchNum", 3);
                     owner.animator.SetTrigger("shoot");
                 },
                 delegate (mobBehavior owner)//on state exit
             {
                     owner.meleeDmg = Gubernia502.constData.mobSimpleAttackDmg;
                     owner.isStunnedDmg = false;
                     owner.moveScript.setDefaultSettings();
                 },
                 SRonHuntCantMove,
                 getActiveMoveSpeed,
                 delegate () { return Gubernia502.constData.mobSimpleAtkRotSpeed; },
                 getOnAttackMoveDoneDis,
                 "BSjerk"
             );
        public static primBehaviorState BStrackTarget { get; private set; } = new primBehaviorState
            (
                SRonDeath,
                SRonGetStun,//on get stunned
                emptyTakeDmg,//on take dmg
                emptyTakePos,//on hear sound
                emptyTakePos,//on lost heared sound
                delegate (mobBehavior owner, hitPointSystem targetEnemy)//on found enemy
            {
                    owner.disableMoveScripts();
                    owner.currentState = BShunt;
                },
                delegate (mobBehavior owner)//on lost enemy
            {
                    owner.disableMoveScripts();
                    owner.enemyDirection = Gubernia502.angleFromDirection((owner.enemyLastPosition - owner.transform.position).normalized);
                    owner.currentState = BSfindEnemy;
                },
                emptySimple,//on path not found
                emptySimple,//on body move done
                delegate (mobBehavior owner)//on state enter
            {
                    owner.bodyRotateScript.setTrackTarget();
                },
                delegate (mobBehavior owner)//on state exit
            {
                    owner.bodyRotateScript.setRotateForMove();
                },
                emptySimple,
                getZero,
                getActiveRotSpeed,
                getZero,
                "BStrackTarget"
            );
        public class primBehaviorState
        {
            public primBehaviorState(RAonTakeDamage onDeath, RAonTakeDamage onGetStuned,
                RAonTakeDamage onTakeDamage, RAonTakeEnemyPosition onHearedSound,
                RAonTakeEnemyPosition onLostHearedSound, RAonFoundEnemy onVisibleFoundEnemy,
                RAsimple onLostVisibleEnemy, RAsimple onPathNotFound, RAsimple onBodyMoveDone,
                RAsimple onStateEnter, RAsimple onStateExit, RAsimple onCantMoveToPoint,
                Gubernia502.getFloatFun moveSpeed, Gubernia502.getFloatFun rotationSpeed,
                Gubernia502.getFloatFun moveDoneDistance, string name)
            {
                this.onDeath = onDeath;
                this.onGetStuned = onGetStuned;
                this.onTakeDamage = onTakeDamage;
                this.onHearedSound = onHearedSound;
                this.onLostHearedSound = onLostHearedSound;
                this.onVisibleFoundEnemy = onVisibleFoundEnemy;
                this.onLostVisibleEnemy = onLostVisibleEnemy;
                this.onPathNotFound = onPathNotFound;
                this.onBodyMoveDone = onBodyMoveDone;
                this.onStateEnter = onStateEnter;
                this.onStateExit = onStateExit;
                this.onCantMoveToPoint = onCantMoveToPoint;
                this.moveSpeed = moveSpeed;
                this.rotationSpeed = rotationSpeed;
                this.moveDoneDistance = moveDoneDistance;
                this.name = name;
            }
            public readonly RAonTakeDamage onDeath;
            public readonly RAonTakeDamage onGetStuned;
            public readonly RAonTakeDamage onTakeDamage;
            public readonly RAonTakeEnemyPosition onHearedSound;
            public readonly RAonTakeEnemyPosition onLostHearedSound;
            public readonly RAonFoundEnemy onVisibleFoundEnemy;
            public readonly RAsimple onLostVisibleEnemy;
            public readonly RAsimple onPathNotFound;
            public readonly RAsimple onBodyMoveDone;
            public readonly RAsimple onStateEnter;
            public readonly RAsimple onStateExit;
            public readonly RAsimple onCantMoveToPoint;
            public readonly Gubernia502.getFloatFun moveSpeed;
            public readonly Gubernia502.getFloatFun rotationSpeed;
            public readonly Gubernia502.getFloatFun moveDoneDistance;
            public readonly string name;
        }
    }
    public void onMovePointDone()
    {
        if (currentPathMoveIndex >= pathMove.Count-1)
        {
            if (pathFindThreadState == null)
            {
                currentState.onBodyMoveDone(this);
            }
        }
        else
        {
            moveScript.moveTarget = pathMove[++currentPathMoveIndex];
        }
    }
    public void setMoveTarget(Vector3 moveTarget)
    {
        Vector3 target = new Vector3(moveTarget.x, 0, moveTarget.z);
        Vector3 targetDirection = (target - transform.position).normalized;
        if (!Physics.SphereCast(new Ray(new Vector3(transform.position.x, 0, transform.position.z),
                                   targetDirection),moveScript.sphereCastRadius, out RaycastHit hit,
                                   Vector3.Distance(target, transform.position) - moveScript.sphereCastRadius,
                                   512, QueryTriggerInteraction.Ignore))//есть прямой путь к цели
        {
            if (pathFindThreadState != null)
            {
                abortCurrentThread();
            }
            if (pathMove.Count > 1)
            {
                pathMove = new List<Vector3> { target };
                currentPathMoveIndex = 0;
            }
            else
            {
                pathMove[0] = target;
            }
            moveScript.moveTarget = target;
            bodyRotateScript.neededDirectionAngle = Gubernia502.angleFromDirection((target - transform.position).normalized);
        }
        else//нет прямого пути к цели
        {
            if (pathFindThreadState == null)
            {
                if (pathMove.Count > 1)
                {
                    List<Vector3> newPath = Gubernia502.economPathFinding(pathMove, target, transform.position);
                    if (newPath == null)
                    {
                        startPathFindThread(transform.position, target);
                    }
                    else
                    {
                        pathMove = newPath;
                        bodyRotateScript.neededDirectionAngle =
                            Gubernia502.angleFromDirection((target - transform.position).normalized);
                    }
                }
                else
                {
                    startPathFindThread(transform.position, target);
                }
            }
            else
            {
                nextMovetarget = target;
                isNextTarget = true;
            }
        }
    }
    private void setMainMoveTarget(Vector3 moveTarget)
    {
        isNextTarget = false;
        if (pathFindThreadState != null)
        {
            abortCurrentThread();
        }
        Vector3 target = new Vector3(moveTarget.x, 0, moveTarget.z);
        Vector3 targetDirection = (target - transform.position).normalized;
        if (!Physics.SphereCast(new Ray(new Vector3(transform.position.x, 0, transform.position.z),
                                   targetDirection), moveScript.sphereCastRadius, out RaycastHit hit,
                                   Vector3.Distance(target, transform.position) - moveScript.sphereCastRadius,
                                   512, QueryTriggerInteraction.Ignore))//есть прямой путь к цели
        {
            if (pathMove.Count > 1)
            {
                pathMove = new List<Vector3> { target };
                currentPathMoveIndex = 0;
            }
            else
            {
                pathMove[0] = target;
            }
            moveScript.moveTarget = pathMove[0];
            bodyRotateScript.neededDirectionAngle = Gubernia502.angleFromDirection((target - transform.position).normalized);
        }
        else//нет прямого пути к цели
        {
            if (pathMove.Count > 1)
            {
                List<Vector3> newPath = Gubernia502.economPathFinding(pathMove, target, transform.position);
                if (newPath == null)
                {
                    startPathFindThread(transform.position, target);
                }
                else
                {
                    pathMove = newPath;
                    bodyRotateScript.neededDirectionAngle =
                        Gubernia502.angleFromDirection((target - transform.position).normalized);
                }
            }
            else
            {
                startPathFindThread(transform.position, target);
            }
        }
    }
    public void setVisibleMoveTarget(Vector3 target)
    {
        if (pathFindThreadState != null)
        {
            abortCurrentThread();
        }
        if (pathMove.Count > 1)
        {
            pathMove = new List<Vector3> { target };
            currentPathMoveIndex = 0;
        }
        else
        {
            pathMove[0] = target;
        }
        moveScript.moveTarget = pathMove[0];
        bodyRotateScript.neededDirectionAngle = Gubernia502.angleFromDirection((target - transform.position).normalized);
    }
    private void abortCurrentThread()
    {
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
        enabled = false;
    }
    private void startPathFindThread(Vector3 start, Vector3 end)
    {
        enabled = true;
        pathFindThreadState = new Gubernia502.pathFindThreadState(start, end);
        pathFindThreadState.currentThread = new Thread(new ThreadStart(pathFindThreadState.threadDextraPathFind));
        pathFindThreadState.currentThread.Start();
    }
    private void Update()
    {
        if (pathFindThreadState == null)
        {
            if (isNextTarget)
            {
                setMainMoveTarget(nextMovetarget);
            }
            enabled = false;
            return;
        }
        if (pathFindThreadState.pathFindDone)
        {
            if (pathFindThreadState.finalPath == null)//нет пути до точки
            {
                currentState.onPathNotFound(this);
            }
            else//есть путь до точки
            {
                Vector3 target;
                bool pathIsActual = false;
                for (int i = pathFindThreadState.finalPath.Count - 1; i >= 0; i--)
                {
                    target = new Vector3(pathFindThreadState.finalPath[i].x, transform.position.y, pathFindThreadState.finalPath[i].z);
                    if (!Physics.SphereCast(new Ray(transform.position,
                                           (target - transform.position).normalized), moveScript.sphereCastRadius,
                                           Vector3.Distance(target, transform.position), 512, QueryTriggerInteraction.Ignore))
                    {
                        currentPathMoveIndex = i;
                        pathIsActual = true;
                        break;
                    }
                }
                if (pathIsActual)
                {
                    pathMove = pathFindThreadState.finalPath;
                    moveTarget = pathFindThreadState.finalPath[pathFindThreadState.finalPath.Count - 1];
                    bodyRotateScript.neededDirectionAngle =
                        Gubernia502.angleFromDirection((pathMove[currentPathMoveIndex] - transform.position).normalized);
                }
                else
                {
                    startPathFindThread(transform.position, pathFindThreadState.finalPath[pathFindThreadState.finalPath.Count - 1]);
                    return;
                }
                moveScript.moveTarget = pathMove[currentPathMoveIndex];
            }
            if (pathFindThreadState != null)
            {
                pathFindThreadState.currentThread.Abort();
            }
            if (isNextTarget)
            {
                setMoveTarget(nextMovetarget);
            }
            enabled = false;
        }
    }
    public List<Vector3> pathMove { get; private set; } = new List<Vector3> { Vector3.zero};
    public Gubernia502.pathFindThreadState pathFindThreadState { get; private set; } = null;
    private int currentPathMoveIndex = 0;
    public int meleeDmg;
    public bool isStunnedDmg = false;
    private Vector3 moveTarget;
    private Vector3 nextMovetarget;
    private bool isNextTarget = false;
    private mobBehaviorStateMachine.primBehaviorState CurrentState = mobBehaviorStateMachine.BSdeath;
    public mobBehaviorStateMachine.primBehaviorState currentState
    {
        get => CurrentState;
        set
        {
            bodyRotateScript.enabled = false;
            CurrentState.onStateExit(this);
            CurrentState = value;
            value.onStateEnter(this);
        }
    }
    ~mobBehavior()
    {
        Gubernia502.enemies.Remove(this);
    }
    private void OnDestroy()
    {
        Gubernia502.enemies.Remove(this);
    }
    public enum startMode
    {
        passivePatrul=0,
        desactiveIdle=1,
        stayOnPoint=2
    }
    public startMode startBehavior=startMode.desactiveIdle;
    public enemyViewZone viewZone;
    public Rigidbody RGbody;
    public Animator animator;
    public mobMeleeShoot meleeShoot;
    public mobWeaponHitBox rightHand;
    public mobWeaponHitBox leftHand;
    public mobDmgSystem dmgSystem;
    public mobMove moveScript;
    public mobBodyRotateForView bodyRotateScript;
    public soundHear soundHear;
    public mobMeleeFrontHitBox meleeFrontHitBox;

    public readonly float repeatPathFindDelay = 3f;

    public bool isAggressive = false;
    public float enemyDirection;
    public Vector3 enemyLastPosition;
    public hitPointSystem targetEnemy=null;
    public List<Vector3> patrulPoint=new List<Vector3> { };
    public int patrulPointNum;
    mobBehaviorStateMachine.primBehaviorState defaultState;
    public IEnumerator idleDelay(float delayTime, Gubernia502.simpleFun actionOnExit)
    {
        yield return new WaitForSeconds(delayTime);
        actionOnExit();
        yield break;
    }
    public void disableMoveScripts()
    {
        if (!dmgSystem.isDead)
        {
            StopAllCoroutines();
        }
        moveScript.enabled = false;
        bodyRotateScript.enabled = false;
    }
    /// <summary>
    /// по завершению движения к цели
    /// </summary>
    public void onRotateMoveDone()
    {
        currentState.onBodyMoveDone(this);
    }
    public void onEliminateTarget()
    {
        if (targetEnemy != null)
        {
            enemyLastPosition = targetEnemy.transform.position;
            currentState.onLostVisibleEnemy(this);
            targetEnemy = null;
        }
    }
    private void Start()
    {
        if (Gubernia502.gameIsActive)
        {
            changeDefaultState(startBehavior);
        }
        if (Gubernia502.gameIsActive)
        {
            setDefaultBehavior();
        }
        else
        {
            currentState = mobBehaviorStateMachine.BSpotato;
        }
        meleeDmg = Gubernia502.constData.mobSimpleAttackDmg;
    }
    public void disableBatrak()
    {
        currentState = mobBehaviorStateMachine.BSpotato;
    }
    public void setDefaultBehavior()
    {
        disableMoveScripts();
        currentState = defaultState;
    }
    public void changeDefaultState()
    {
        changeDefaultState(startBehavior);
    }
    public void changeDefaultState(startMode newDefaultBehavior)
    {
        switch (newDefaultBehavior)
        {
            case startMode.passivePatrul:
                if (patrulPoint.Count > 1)
                {
                    defaultState = mobBehaviorStateMachine.BSpatrul;
                }
                else
                {
                    startBehavior = startMode.stayOnPoint;
                    defaultState = mobBehaviorStateMachine.BSstayOnPoint;
                }
                break;
            case startMode.desactiveIdle:
                defaultState = mobBehaviorStateMachine.BSpotato;
                break;
            case startMode.stayOnPoint:
                defaultState = mobBehaviorStateMachine.BSstayOnPoint;
                break;
            default:
                patrulPoint = new List<Vector3> { transform.position };
                defaultState = mobBehaviorStateMachine.BSstayOnPoint;
                break;
        }
    }
    private void Awake()
    {
        Gubernia502.enemies.Add(this);
    }
}
