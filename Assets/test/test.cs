using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.Rotate(Vector3.up);
        Debug.Log(transform.eulerAngles);
    }
}
