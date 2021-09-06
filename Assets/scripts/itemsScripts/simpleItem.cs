using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class simpleItem :ScriptableObject
{
    public Texture icon;
    [Range(0,20)]
    public int id;
    public GameObject prefab;
    [Range(0,1000)]
    public int stack;

    public const int maxCount = 1000;
}
