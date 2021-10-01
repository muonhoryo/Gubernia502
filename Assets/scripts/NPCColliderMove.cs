using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCColliderMove : MonoBehaviour
{
    [SerializeField]
    private NPCLockControl NPCLockControl;
    [SerializeField]
    private Vector3 colliderOffset;
    public CapsuleCollider colliderObj;
    public Transform colliderPos;
    void Update()
    {
        colliderObj.center = colliderPos.position-transform.position+colliderOffset;
    }
}
