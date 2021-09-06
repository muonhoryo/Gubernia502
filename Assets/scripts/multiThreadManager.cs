using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class multiThreadManager : MonoBehaviour
{
    object syns = new object();
    public delegate void pathFindThreadAction(Gubernia502.pathFindThreadState pathFindState);
    public List<Gubernia502.simpleFun> simpleActions { get; private set; } = new List<Gubernia502.simpleFun> { };
    public List<pathFindThreadAction> lateActions { get; private set; } = new List<pathFindThreadAction> { };
    public List<Gubernia502.pathFindThreadState> lateActionsHandler { get; private set; } = new List<Gubernia502.pathFindThreadState> { };
    public void addLateAction(pathFindThreadAction lateAction, Gubernia502.pathFindThreadState pathFindState)
    {
        lock (syns)
        {
            lateActions.Add(lateAction);
            lateActionsHandler.Add(pathFindState);
        }
    }
    public void addSimpleAction(Gubernia502.simpleFun action)
    {
        lock (syns)
        {
            simpleActions.Add(action);
        }
    }
    void Update()
    {
        lock (syns)
        {
            while (lateActions.Count > 0)
            {
                lateActions[0].Invoke(lateActionsHandler[0]);
                lateActionsHandler[0].waitHandler.Set();
                lateActions.RemoveAt(0);
                lateActionsHandler.RemoveAt(0);
            }
            while (simpleActions.Count > 0)
            {
                simpleActions[0].Invoke();
                simpleActions.RemoveAt(0);
            }
        }
    }
}
