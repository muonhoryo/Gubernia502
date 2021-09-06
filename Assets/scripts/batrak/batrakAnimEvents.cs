using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakAnimEvents : MonoBehaviour
{
    [SerializeField]
    batrakBehavior batrakBehavior;
    public void onAnimExit()
    {
        batrakBehavior.onRotateMoveDone();
    }
}
