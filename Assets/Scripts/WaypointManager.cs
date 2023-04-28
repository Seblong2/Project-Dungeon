using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{

    public Transform GetWayPoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public int NextWayPointIndex(int currentPointIndex)
    {
        int nextPointIndex = currentPointIndex + 1;

        if(nextPointIndex == transform.childCount)
        {
            nextPointIndex = 0;
        }

        return nextPointIndex;
    }


}
