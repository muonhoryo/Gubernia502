using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abstractClassesStart : MonoBehaviour//singltone
{
    static abstractClassesStart singltone=null;
    static bool isStarted = false;
    //Gubernia502ConstData.cs
    public Gubernia502ConstData constData;
    void Awake()
    {
        if(singltone == null)
        {
            singltone = this;
            if (!isStarted)
            {
                Gubernia502.constData = constData;
                Gubernia502.saveFileName = constData.saveFile.name;
            }
        }
        Destroy(this);
    }
}
