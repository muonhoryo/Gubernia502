using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class takingDamageObjData : ScriptableObject
{
    public bool isAlife;
    [Range(0, globalMaxShieldDurability)]
    public int maxShieldDuability;
    [Range(1, globalMaxHPpoint)]
    public int maxHitPoint;
    [Range(0,100)]
    public int normalRezist,exploziveRezist;
    public GameObject[] onHitEffect;
    public GameObject onDeathEffect;

    public const int globalMaxShieldDurability=10000;
    public const int globalMaxHPpoint = 10000;
}
