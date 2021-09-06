using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class secondEquipedPos : ScriptableObject
{
    public Quaternion rotationOnSelected = Quaternion.Euler(0f, 0f, 0f);
    public Vector3 positionOnSelected = Vector3.zero;
}
