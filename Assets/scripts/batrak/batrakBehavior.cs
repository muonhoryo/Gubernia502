using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakBehavior : MonoBehaviour
{
    [SerializeReference]
    string test;

    ~batrakBehavior()
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
    public Rigidbody batrakRGbody;
    public Animator batrakAnim;
    public batrakMeleeShoot meleeShoot;
    public batrakWeaponHitBox rightHand;
    public batrakWeaponHitBox leftHand;
    public batrakDmgSystem dmgSystem;
    public batrakMove moveScript;
    public batrakBodyRotateForView bodyRotateScript;
    public soundHear soundHear;
    public batrakMeleeFrontHitBox meleeFrontHitBox;

    private float repeatPathFindDelay = 3f;

    public bool isAggressive = false;
    public float enemyDirection;
    public Vector3 enemyLastPosition;
    public hitPointSystem targetEnemy=null;
    public List<Vector3> patrulPoint=new List<Vector3> { };
    public delegate void onDamageReaction(float hitAngle);
    public delegate void onHearSoundReaction(Vector3 soundPos);
    public delegate void onFoundEnemyReaction(hitPointSystem targetEnemy);
    private Gubernia502.simpleFun[] onMoveDoneActions;
    [SerializeField]
    private int patrulPointNum;
    public onDamageReaction onDeath;
    public onDamageReaction onGetStuned;
    public onDamageReaction onTakeDamage;
    public onHearSoundReaction onHearedSound;
    public onHearSoundReaction onLostHearedSound;
    public onFoundEnemyReaction onVisibleFoundEnemy;
    public Gubernia502.simpleFun onLostVisibleEnemy;
    public Gubernia502.simpleFun onPathNotFound;
    Gubernia502.simpleFun defaultState;
    Gubernia502.simpleFun BehaviorState;
    Gubernia502.simpleFun NextState;
    /// <summary>
    /// Следующее действие(вызывается при выключении скрипта движения)
    /// </summary>
    public Gubernia502.simpleFun nextState
    {
        get
        {
            Gubernia502.simpleFun state = NextState;
            NextState =delegate() { };
            return state;
        }
        set
        {
            NextState = value; 
        }
    }
    /// <summary>
    /// Текущее действие(вызывается автоматически при назначении)
    /// </summary>
    public Gubernia502.simpleFun behaviorState
    {
        get => BehaviorState;
        set
        {
            test = value.Method.Name;
            BehaviorState = value;
            value();
        }
    }
    /// <summary>
    /// Передаваемая в качестве второго параметра 
    /// функция автоматически сработает по истечению 
    /// заданного времени
    /// </summary>
    /// <param name="delayTime"></param>
    /// <param name="actionOnExit"></param>
    /// <returns></returns>
    /// 
    IEnumerator idleDelay(float delayTime, Gubernia502.simpleFun actionOnExit)
    {
        yield return new WaitForSeconds(delayTime);
        behaviorState=actionOnExit;
        yield break;
    }
    private void disableMoveScripts()
    {
        if (!dmgSystem.isDead)
        {
            StopAllCoroutines();
        }
        moveScript.enabled = false;
        bodyRotateScript.enabled = false;
    }
    public void delayNewAction(int actionNum=0)
    {
        disableMoveScripts();
        nextState = onMoveDoneActions[actionNum];
    }
    public void newAction(int actionNum = 0)
    {
        disableMoveScripts();
        behaviorState = onMoveDoneActions[actionNum];
    }
    private void RApathNotFoundOnHunt()
    {
        disableMoveScripts();
        behaviorState = BHtraсkEnemy;
    }
    private void RAdeath(float hitAngle)
    {
        onTakeDamage = onGetStuned=onDeath = delegate (float i) { };
        onHearedSound = onLostHearedSound = delegate (Vector3 i) { };
        onVisibleFoundEnemy = delegate (hitPointSystem targetEnemy) {  };
        onLostVisibleEnemy = delegate () { };
        Destroy(moveScript.batrakCollider);
        disableMoveScripts();
        StartCoroutine(idleDelay(Gubernia502.constData.batrakTimeToDeath, delegate () { Destroy(gameObject); }));
        if (Gubernia502.differenceOf2Angle(hitAngle,bodyRotateScript.rotatedBody.eulerAngles.y) > 90)
        {
            batrakAnim.SetInteger("stan", 4);
        }
        else
        {
            batrakAnim.SetInteger("stan", 3);
        }
    }
    private void RAgetStunned(float hitAngle)
    {
        if (targetEnemy != null)
        {
            delayNewAction(1);
            RAPassiveFoundEnemy(targetEnemy);
        }
        else if(soundHear.soundHears.Count>0)
        {
            delayNewAction(3);
            RAPassiveHearSound(soundHear.soundHears[0].transform.position);
        }
        else
        {
            delayNewAction();
            RAPassiveTakeDmg(hitAngle);
        }
        batrakAnim.SetInteger("stan", 2);
        bodyRotateScript.rotatedBody.rotation = Quaternion.Euler(0f, hitAngle, 0f);
        enemyDirection = hitAngle;
    }//get stunned
    private void RAonActiveTakeDmg(float hitAngle)
    {
        enemyDirection = hitAngle;
        newAction(0);
    }//take damage on active(not hear sound and not see enemy)
    private void RAonActivizationTakeDmg(float hitAngle)
    {
        enemyDirection = hitAngle;
    }//take damage on activization(not hear sound and not see enemy)
    private void RAPassiveTakeDmg(float hitAngle)
    {
        enemyDirection = hitAngle;
        onTakeDamage = RAonActivizationTakeDmg;
        onHearedSound = RAonActivizationHearSound;
        onVisibleFoundEnemy = RAonActivizationFoundEnemy;
        onLostHearedSound  = delegate(Vector3 i) { Debug.Log("RAPassiveTakeDmg-onLostHearedSound"); };
        onLostVisibleEnemy = delegate () { Debug.Log("RAPassiveTakeDmg-onLostVisibleEnemy"); };
    }//take damage on passive(not hear sound and not see enemy)
    private void RAonHearSoundOnMoveLastEnPos(Vector3 lastSoundPos)
    {
        onTakeDamage = delegate (float i) { };
        onHearedSound = delegate (Vector3 soundPos) { moveScript.moveTarget = enemyLastPosition = soundPos; };
        onLostHearedSound = RAonLostHearedSounOonMoveLastEnPos;
        onLostHearedSound += delegate (Vector3 soundPosition) { moveScript.moveTarget = enemyLastPosition = soundPosition; };
    }
    private void RAonActiveHearSound(Vector3 soundPosition)
    {
        enemyLastPosition = soundPosition;
        newAction(3);
    }
    private void RAreturnOnActiviztionHearSound(Vector3 soundPosition)
    {
        enemyLastPosition = soundPosition;
    }//hear sound on activization,when sound are been heared(not see enemy)
    private void RAonActivizationHearSound(Vector3 soundPosition)
    {
        RAPassiveHearSound(soundPosition);
        nextState = onMoveDoneActions[3];
    }//hear sound on activization(not see enemy)
    private void RAPassiveHearSound(Vector3 soundPosition)
    {
        enemyLastPosition = soundPosition;
        onTakeDamage = delegate (float i) {  };
        onHearedSound = RAreturnOnActiviztionHearSound;
        onVisibleFoundEnemy = RAonActivizationFoundEnemy;
        onLostHearedSound = RAreturnOnActiviztionHearSound;
        onLostVisibleEnemy = delegate() { Debug.Log("RAPassiveHearSound-onLostVisibleEnemy"); };
    }//hear sound on passive(not see enemy)
    private void RAonActiveFoundEnemy(hitPointSystem targetEnemy)
    {
        newAction(1);
    }
    private void RAonActivizationFoundEnemy(hitPointSystem targetEnemy)
    {
        RAPassiveFoundEnemy(targetEnemy);
        nextState = onMoveDoneActions[1];
    }//saw enemy on activization
    private void RAPassiveFoundEnemy(hitPointSystem i)
    {
        onTakeDamage = delegate (float i) {  };
        onHearedSound = delegate (Vector3 i) {  };
        onLostHearedSound = delegate (Vector3 i) { Debug.Log("RAPassiveFoundEnemy-onLostHearedSound"); };
        onVisibleFoundEnemy = delegate (hitPointSystem targetEnemy) {  };
        onLostVisibleEnemy = RAonActivizationLostVisibleEnemy;
    }//saw enemy on passive
    private void RAonLostHearedSounOonMoveLastEnPos(Vector3 lastSoundPos)
    {
        onTakeDamage = RAonActiveTakeDmg;
        onHearedSound = RAonHearSoundOnMoveLastEnPos;
        onHearedSound +=delegate (Vector3 soundPosition) { moveScript.moveTarget = enemyLastPosition = soundPosition; };
        onLostHearedSound = delegate (Vector3 i) { Debug.Log("RAonLostHearedSounOonMoveLastEnPos-onLostHearedSound"); };
    }
    private void RAonActivizationLostVisibleEnemy()
    {
        if (!viewZone.foundNewTarget())
        {
            RAPassiveHearSound(targetEnemy.transform.position);
            nextState = onMoveDoneActions[3];
        }
    }//lost visible enemy on activization
    private void RAonActiveLostEnemy()
    {
        if (!viewZone.foundNewTarget())
        {
            moveScript.moveTarget = enemyLastPosition = targetEnemy.transform.position;
            behaviorState = BHmoveToLastEnemyPosition;
        }
    }
    private void RAonTrackLostEnemy()
    {
        if (!viewZone.foundNewTarget())
        {
            moveScript.moveTarget = enemyLastPosition = targetEnemy.transform.position;
            behaviorState = BHmoveToLastEnemyPosition;
            bodyRotateScript.setRotateForMove();
        }
    }
    private void RAonSimpleAttackLostEnemy()
    {
        if (!viewZone.foundNewTarget())
        {
            enemyLastPosition = targetEnemy.transform.position;
            bodyRotateScript.enabled = false;
        }
    }
    private void setOnHuntReaction()
    {
        onPathNotFound = RApathNotFoundOnHunt;
        onTakeDamage = delegate (float i) { };
        onHearedSound = onLostHearedSound = delegate (Vector3 i) { };
        onVisibleFoundEnemy = delegate (hitPointSystem targetEnemy) {  };
        moveScript.moveDoneDistance = Gubernia502.constData.batrakOnAttackMoveDoneDistance;
    }
    private void setPassiveReaction()
    {
        onTakeDamage= delegate (float hitAngle)
        {
            delayNewAction();
            RAPassiveTakeDmg(hitAngle);
            behaviorState = BHactivization;
        };
        onHearedSound = delegate(Vector3 soundPosition)
        {
            RAPassiveHearSound(soundPosition);
            delayNewAction(3);
            behaviorState = BHactivization;
        };
        onVisibleFoundEnemy = delegate (hitPointSystem targetEnemy)
        {
            RAPassiveFoundEnemy(targetEnemy);
            delayNewAction(1);
            behaviorState = BHactivization;
        };
        onLostHearedSound = delegate (Vector3 i) { Debug.Log("setPassiveReaction-onLostHearedSound"); };
        onLostVisibleEnemy = delegate () { Debug.Log("setPassiveReaction-onLostVisibleEnemy"); };
        onGetStuned = RAgetStunned;
    }
    private void setPassiveBH()
    {
        bodyRotateScript.setPeacefulSpeed();
        moveScript.setPeacefulSpeed();
        isAggressive = false;
        batrakAnim.SetBool("isAggressive", false);
        setDefaultBehavior();
    }
    private void setAggressiveBH()
    {
        bodyRotateScript.setAggressiveSpeed();
        moveScript.setAggressiveSpeed();
        isAggressive = true;
        batrakAnim.SetBool("isAggressive", true);
    }
    public void setMoveOnPath()
    {
        if (isAggressive&&behaviorState!=BHmoveToLastEnemyPosition)
        {
            behaviorState = BHmoveToPath;
        }
    }
    private void BHmoveToPath()
    {
        onPathNotFound = delegate () { behaviorState = BHfindEnemy; };
        if (soundHear.soundHears.Count > 0)//есть слышимый звук
        {
            RAonHearSoundOnMoveLastEnPos(enemyLastPosition);
        }
        else//нет слышимого звука
        {
            RAonLostHearedSounOonMoveLastEnPos(enemyLastPosition);
        }
        onVisibleFoundEnemy = RAonActiveFoundEnemy;
        onLostVisibleEnemy = delegate () { Debug.Log("BHmoveToLastEnemyPosition-onLostVisibleEnemy"); };
        nextState = BHSearchTarget;
    }
    private void BHactivization()
    {
        setAggressiveBH();
    }
    /// <summary>
    /// Движение к последнему месту противника(или к источнику шума)
    /// </summary>
    private void BHmoveToLastEnemyPosition()
    {
        BHmoveToPath();
        moveScript.moveTarget = enemyLastPosition;
    }
    /// <summary>
    /// Простая атака
    /// </summary>
    private void BHSimpleAttack()
    {
        bodyRotateScript.setTrackTarget();
        soundHear.enabled = false;
        onTakeDamage = delegate (float i) { };
        onHearedSound = onLostHearedSound = delegate (Vector3 i) { };
        onVisibleFoundEnemy = delegate (hitPointSystem targetEnemy) {  };
        onLostVisibleEnemy = RAonSimpleAttackLostEnemy;
        nextState = BHSearchTarget;
        if (batrakAnim.GetInteger("punchNum") != 1)
        {
            meleeShoot.hitBox = rightHand;
            batrakAnim.SetInteger("punchNum", 1);
        }
        else
        {
            meleeShoot.hitBox = leftHand;
            batrakAnim.SetInteger("punchNum", 2);
        }
        batrakAnim.SetTrigger("shoot");
    }
    private void BHSearchTarget()
    {
        disableMoveScripts();
        onTakeDamage = RAonActiveTakeDmg;
        onHearedSound = RAonActiveHearSound;
        onVisibleFoundEnemy = RAonActiveFoundEnemy;
        onLostHearedSound = delegate(Vector3 i) {  };
        onLostVisibleEnemy = delegate() {  };
        batrakAnim.SetTrigger("search");
        nextState = BHcirclePatrulIdle;
    }
    /// <summary>
    /// поиск цели
    /// </summary>
    private void BHfindEnemy()
    {
        bodyRotateScript.neededDirectionAngle = enemyDirection;
        onTakeDamage = delegate(float hitAngle) { bodyRotateScript.neededDirectionAngle = enemyDirection = hitAngle; };
        onHearedSound = RAonActiveHearSound;
        onVisibleFoundEnemy = RAonActiveFoundEnemy;
        onLostHearedSound = delegate(Vector3 i) { Debug.Log("BHfindEnemy-onLostHearedSound"); };
        onLostVisibleEnemy = delegate() { Debug.Log("BHfindEnemy-onLostVisibleEnemy"); };
            nextState = BHSearchTarget;
            bodyRotateScript.setOnlyRotate();
    }
    /// <summary>
    /// Преследование цели
    /// </summary>
    private void BHhunt()
    {
        moveScript.setTargetMove();
        onLostVisibleEnemy = RAonActiveLostEnemy;
        nextState = BHSimpleAttack;
        moveScript.moveTarget = targetEnemy.transform.position;
    }
    private void BHhuntEnter()
    {
        setOnHuntReaction();
        behaviorState = BHhunt;
    }
    private void BHtraсkEnemy()
    {
        setOnHuntReaction();
        onLostVisibleEnemy = RAonTrackLostEnemy;
        nextState = BHhunt;
        bodyRotateScript.setTrackTarget();
        StartCoroutine(idleDelay(repeatPathFindDelay, delegate () { behaviorState = nextState; }));
    }
    /// <summary>
    /// Задержка перед уходом в патруль по периметру
    /// </summary>
    private void BHcirclePatrulIdle()
    {
        patrulPointNum = 0;
        StartCoroutine(idleDelay(Gubernia502.constData.batrakPatrulIdleStateTime, setPassiveBH));
    }
    /// <summary>
    /// Патрулирование по кругу
    /// </summary>
    private void BHCirclePatrul()
    {
        nextState = BHCirclePatrul;
        if (patrulPointNum == patrulPoint.Count)
        {
            patrulPointNum = 0;
        }
        moveScript.moveTarget = patrulPoint[patrulPointNum++];
    }
    private void BHcirclePatrulEnter()
    {
        patrulPointNum = 0;
        onPathNotFound = delegate () { behaviorState = BHCirclePatrul; };
        setPassiveReaction();
        if (targetEnemy != null)
        {
            onVisibleFoundEnemy(targetEnemy);
        }
        else
        {
            behaviorState = BHCirclePatrul;
        }
    }
    private void BHpotatoMode()
    {
        onTakeDamage = delegate (float i) { };
        onHearedSound = onLostHearedSound = delegate (Vector3 i) { };
        onVisibleFoundEnemy = delegate (hitPointSystem targetEnemy) { };
        onLostVisibleEnemy = delegate () { };
        nextState = BHpotatoMode;
        onGetStuned = delegate (float hitAngle)
         {
             batrakAnim.SetInteger("stan", 2);
             bodyRotateScript.rotatedBody.rotation = Quaternion.Euler(0f, hitAngle, 0f);
         };
    }
    private void BHstayOnPoint()
    {
    }
    private void BHMoveToPoint()
    {
        nextState = BHstayOnPoint;
        if (patrulPoint.Count == 0)
        {
            patrulPoint = new List<Vector3> { transform.position };
            behaviorState = nextState;
        }
        else
        {
            moveScript.moveTarget = patrulPoint[0];
            if (patrulPoint.Count > 1)
            {
                patrulPoint = new List<Vector3> { patrulPoint[0] };
            }
        }
    }
    private void BHStayOnPointEnter()
    {
        onPathNotFound = delegate ()
        {
            behaviorState = BHstayOnPoint;
            StartCoroutine(idleDelay(repeatPathFindDelay, delegate () { behaviorState = BHMoveToPoint; }));
        };
        setPassiveReaction();
        if (targetEnemy != null)
        {
            onVisibleFoundEnemy(targetEnemy);
        }
        else
        {
            behaviorState = BHMoveToPoint;
        }
    }
    /// <summary>
    /// по завершению движения к цели
    /// </summary>
    public void onRotateMoveDone()
    {
        behaviorState =nextState;
    }
    public void onEliminateTarget()
    {
        if (targetEnemy != null)
        {
            onLostVisibleEnemy();
            targetEnemy = null;
        }
    }
    private void Start()
    {
        if (Gubernia502.gameIsActive)
        {
            setDefaultBehavior();
        }
        else
        {
            behaviorState = BHpotatoMode;
        }
    }
    public void disableBatrak()
    {
        newAction(4);
    }
    public void setDefaultBehavior()
    {
        disableMoveScripts();
        behaviorState = defaultState;
    }
    public void changeDefaultState()
    {
        changeDefaultState(startBehavior);
    }
    public void changeDefaultState(startMode newDefaultBehavior)
    {
        bodyRotateScript.setPeacefulSpeed();
        moveScript.setPeacefulSpeed();
        switch (newDefaultBehavior)
        {
            case startMode.passivePatrul:
                if (patrulPoint.Count > 1)
                {
                    defaultState = BHcirclePatrulEnter;
                }
                else
                {
                    startBehavior = startMode.stayOnPoint;
                    defaultState = BHStayOnPointEnter;
                }
                break;
            case startMode.desactiveIdle:
                defaultState = BHpotatoMode;
                break;
            case startMode.stayOnPoint:
                defaultState = BHStayOnPointEnter;
                break;
            default:
                patrulPoint = new List<Vector3> { transform.position };
                defaultState = BHstayOnPoint;
                break;
        }
    }
    private void Awake()
    {
        if (Gubernia502.gameIsActive)
        {
            changeDefaultState(startBehavior);
        }
        onDeath = RAdeath;

        onMoveDoneActions = new Gubernia502.simpleFun[]
        {
            BHfindEnemy,
            BHhuntEnter,
            BHSimpleAttack,
            BHmoveToLastEnemyPosition,
            BHpotatoMode
        };
        Gubernia502.enemies.Add(this);
    }
}
