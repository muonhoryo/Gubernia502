using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class regenData : ScriptableObject
{
    [Range(0.1f,100f)]
    public float regenDelay;
    [Range(1,100)]
    public int regenCoolDownTime;
}
