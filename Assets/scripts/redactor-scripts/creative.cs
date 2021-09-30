using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creative : MonoBehaviour//singltone
{
    static creative singltone = null;
    public Object creatingObject;
    public Transform redactorObject;
    public Transform redactor;
    public createSide side;
    public enum createSide
    {
        up,
        down,
        right,
        left,
        stay
    }
    [ContextMenu("create")]
    
    public void create()
    {
        switch(side)
        {
            case createSide.up: redactor.position += Vector3.forward; break;
            case createSide.down: redactor.position -= Vector3.forward; break;
            case createSide.left: redactor.position -= Vector3.right; break;
            case createSide.right: redactor.position += Vector3.right; break;
            default:break;

        }
        Instantiate(creatingObject, redactorObject.position,Quaternion.identity);
    }
    private void Awake()
    {
        if (singltone != null)
        {
            Destroy(this);
        }
        else
        {
            singltone = this;
        }
    }
}
