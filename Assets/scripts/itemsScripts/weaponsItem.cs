using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class weaponsItem : ScriptableObject
{
    //дефолтные статы
    /// <summary>
    /// 0-right hand,1-left hand
    /// </summary>
    [Range(0, 1)]
    public int parentObj;
    [Range(0,1000)]
    public int damage;
    [Range(1,100)]
    public int fireCoolDown;
    [Range(0.1f,100)]
    public float soundVolume;
    /// <summary>
    /// default mag size without magsizebuff
    /// </summary>
    [Range(0,1000)]
    public int magSize;
    [Range(0,180)]
    public float dispersion;
    public bool isDistant;//0-ближний,1-дальний
    public bool isAuto;
    [Range(0,6)]
    public int animWeaponModifier;//анимация идла,стрельбы и прочего
    public List<int> ammotTypes = new List<int> { };
    //скрипт оружия
    public Quaternion rotationOnSelected =Quaternion.Euler(0f,0f,0f);
    public Vector3 positionOnSelected =Vector3.zero;
    public secondEquipedPos secondEquipedPos=null;
    public Vector3 magImpulseTraectory = Vector3.zero;
    public float altBulletStartZOffset;

    public GameObject magazinePrefab;
    public GameObject onDestroyEffect;
    public GameObject equippedWeaponPrefab;

    public const int maxDamageBuff=1000;
    public const int maxSpeedBuff = 1000;
    public const int maxMagSizeBuff = 1000;
    public const int maxAccuracyBuff = 1000;
    public const int maxDurabilityBuff = 1000;
    public const int maxQuality = 10000;
}
