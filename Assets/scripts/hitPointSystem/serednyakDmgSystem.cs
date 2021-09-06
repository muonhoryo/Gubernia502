using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class serednyakDmgSystem : ermakDmgSystem
{
    [SerializeField]
    Image healthIndicator;
    [SerializeField]
    Image activeRegen;
    [SerializeField]
    Image dangerIndicator;
    [SerializeField]
    Image regenIndicator;
    IEnumerator hideHealthBar()
    {
        yield return new WaitForSeconds(Gubernia502.constData.hideGuiDelayTime);
        regenIndicator.gameObject.SetActive(false);
        healthIndicator.gameObject.SetActive(false);
        yield break;
    }
    protected override void takeDamage(int dmgResult, float hitAngle, Vector3 hitPos)
    {
        if (!healthIndicator.gameObject.activeSelf)
        {
            healthIndicator.gameObject.SetActive(true);
            regenIndicator.gameObject.SetActive(true);
        }
        base.takeDamage(dmgResult, hitAngle, hitPos);
    }
    public override int shieldDurability 
    {
        get => base.shieldDurability;
        set
        {
            if (value <= 0)
            {
                healthIndicator.material.SetFloat("fullness",0);
                dangerIndicator.gameObject.SetActive(true);
            }
            else
            {
                healthIndicator.material.SetFloat("fullness", (float)value / takingDamageObjData.maxShieldDuability);
            }
            base.shieldDurability = value;
        }
    }
    private void OnEnable()
    {
        if (dangerIndicator.gameObject.activeSelf)
        {
            dangerIndicator.gameObject.SetActive(false);
        }
        activeRegen.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        activeRegen.gameObject.SetActive(false);
        if (shieldDurability >= takingDamageObjData.maxShieldDuability)
        {
            StartCoroutine(hideHealthBar());
        }
    }
}
