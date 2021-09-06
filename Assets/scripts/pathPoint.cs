using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathPoint : temporalPathPoint
{
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Gubernia502.pathFindingMap.Remove(this);
    }
    private void OnDisable()
    {
        if (Gubernia502.pathFindingMap.Contains(this))
        {
            OnDestroy();
        }
    }
    protected override void OnEnable()
    {
        if (Gubernia502.gameIsActive)
        {
            Gubernia502.recalculateTemporalPathPointsWays(this);
        }
        Gubernia502.pathFindingMap.Add(this);
        base.OnEnable();
    }
}
