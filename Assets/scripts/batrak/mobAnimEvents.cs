using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobAnimEvents : MonoBehaviour
{
    [SerializeField]
    mobBehavior mobBehavior;
    public void onAnimExit()
    {
        mobBehavior.onRotateMoveDone();
    }
}
