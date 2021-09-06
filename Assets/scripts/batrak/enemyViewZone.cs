using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyViewZone : MonoBehaviour
{
    public batrakBehavior batrakBehavior;
    private Gubernia502.simpleFun updateAction;
    public LayerMask ignoringLayers;
    private List<alifeDmgSystem> objInViewZone=new List<alifeDmgSystem>();
    private void OnTriggerEnter(Collider other)
    {
        alifeDmgSystem dmgSystem;
        other.TryGetComponent(out dmgSystem);
        if (dmgSystem != null && Gubernia502.constData.batrakEnemyFractions.Contains(dmgSystem.Fraction))
        {
            objInViewZone.Add(dmgSystem);
            enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out alifeDmgSystem dmgSystem) &&objInViewZone.Contains(dmgSystem))
        {
            objInViewZone.RemoveAt(objInViewZone.IndexOf(dmgSystem));
            
                if (other.transform == batrakBehavior.targetEnemy)
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
                batrakBehavior.targetEnemy = objInViewZone[i];
                objInViewZone[i].becameTarget(batrakBehavior.dmgSystem);
                return true;
            }
        }
        return false;
    }
    private void enemyWasLosted()
    {
        updateAction = trackEnemies;
        batrakBehavior.onLostVisibleEnemy();
        batrakBehavior.targetEnemy.huntEnd(batrakBehavior.dmgSystem);
        batrakBehavior.targetEnemy = null;
    }
    protected virtual void enemyWasFounded(hitPointSystem targetEnemy)
    {
        updateAction = trackVisibleTarget;
        batrakBehavior.targetEnemy = targetEnemy;
        targetEnemy.becameTarget(batrakBehavior.dmgSystem);
        batrakBehavior.onVisibleFoundEnemy(targetEnemy);
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
    }
    private void trackVisibleTarget()
    {
        if (batrakBehavior.targetEnemy != null)
        {
            RaycastHit hit;
            Physics.Linecast(new Vector3(transform.position.x, 0.8f, transform.position.z),
                new Vector3(batrakBehavior.targetEnemy.transform.position.x,
                            0.8f,
                            batrakBehavior.targetEnemy.transform.position.z), out hit, 512, QueryTriggerInteraction.Ignore);
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
