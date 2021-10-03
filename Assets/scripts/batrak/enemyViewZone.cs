using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyViewZone : MonoBehaviour
{
    public mobBehavior mobBehavior;
    private Gubernia502.simpleFun updateAction;
    public LayerMask ignoringLayers;
    private List<alifeDmgSystem> objInViewZone=new List<alifeDmgSystem>();
    public void removeEnemy(alifeDmgSystem alifeDmgSystem)
    {
        objInViewZone.Remove(alifeDmgSystem);
    }
    private void OnTriggerEnter(Collider other)
    {
        alifeDmgSystem dmgSystem;
        other.TryGetComponent(out dmgSystem);
        if (dmgSystem != null && Gubernia502.constData.mobsEnemyFractions.Contains(dmgSystem.Fraction))
        {
            objInViewZone.Add(dmgSystem);
            enabled = true;
            trackEnemies();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out alifeDmgSystem dmgSystem) &&objInViewZone.Contains(dmgSystem))
        {
            objInViewZone.Remove(dmgSystem);
                if (dmgSystem == mobBehavior.targetEnemy)
                {
                    enemyWasLosted();
                }
            if (objInViewZone.Count > 0)
            {
                enabled = false;
            }
        }
    }
    public bool foundNewTarget()
    {
        for (int i = 0; i < objInViewZone.Count; i++)
        {
            RaycastHit hit;
            Physics.Linecast(new Vector3(transform.position.x, 0.8f, transform.position.z),
                new Vector3(objInViewZone[i].transform.position.x,
                            0.8f,
                            objInViewZone[i].transform.position.z), out hit, 512, QueryTriggerInteraction.Ignore);
            if (hit.collider==null&&!objInViewZone[i].isDead)
            {
                mobBehavior.targetEnemy = objInViewZone[i];
                objInViewZone[i].becameTarget(mobBehavior.dmgSystem);
                return true;
            }
        }
        return false;
    }
    private void enemyWasLosted()
    {
        updateAction = trackEnemies;
        mobBehavior.enemyLastPosition = mobBehavior.targetEnemy.transform.position;
        mobBehavior.currentState.onLostVisibleEnemy(mobBehavior);
        mobBehavior.targetEnemy.huntEnd(mobBehavior.dmgSystem);
        mobBehavior.targetEnemy = null;
    }
    protected virtual void enemyWasFounded(alifeDmgSystem targetEnemy)
    {
        updateAction = trackVisibleTarget;
        mobBehavior.targetEnemy = targetEnemy;
        targetEnemy.becameTarget(mobBehavior.dmgSystem);
        mobBehavior.currentState.onVisibleFoundEnemy(mobBehavior,targetEnemy);
    }
    private void trackEnemies()
    {
        if (objInViewZone.Count > 0)
        {
            for (int i = 0; i < objInViewZone.Count; i++)
            {
                if(!Physics.Linecast(new Vector3(transform.position.x, 0.8f, transform.position.z),
                    new Vector3(objInViewZone[i].transform.position.x,
                                0.8f,
                                objInViewZone[i].transform.position.z), 512, QueryTriggerInteraction.Ignore)
                    &&!objInViewZone[i].isDead)
                {
                    enemyWasFounded(objInViewZone[i]);
                }
            }
        }
        else
        {
            enabled = false;
        }
    }
    private void trackVisibleTarget()
    {
        if (mobBehavior.targetEnemy != null)
        {
            RaycastHit hit;
            Physics.Linecast(new Vector3(transform.position.x, 0.8f, transform.position.z),
                new Vector3(mobBehavior.targetEnemy.transform.position.x,
                            0.8f,
                            mobBehavior.targetEnemy.transform.position.z), out hit, 512, QueryTriggerInteraction.Ignore);
            if (hit.collider!=null)
            {
                enemyWasLosted();
            }
        }
    }
    private void Update()
    {
        updateAction();
    }
    private void OnDisable()
    {
        updateAction = trackEnemies;
    }
    private void Awake()
    {
        updateAction = trackEnemies;
    }
}
