using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abstractClassesStart : MonoBehaviour
{
    public debugConsole dbgConsole;
    public MainCamera mainCam;
    public multiThreadManager threadManager;
    //Gubernia502ConstData.cs
    public Gubernia502ConstData constData;
    void Awake()
    {
        Gubernia502.threadManager = threadManager;
        Gubernia502.constData = constData;
        Gubernia502.debugConsole = dbgConsole;
        Gubernia502.mainCamera = mainCam;
        Gubernia502.playerController = mainCam.playerController;
        saveSystem.mainHero = Gubernia502.playerController.gameObject;
        Destroy(GetComponent<abstractClassesStart>());
    }
}
