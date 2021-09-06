using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class breakableItem : ScriptableObject
{
    [Range(1,1000)]
    public int maxDurability;
}