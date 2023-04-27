using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{


    public WaypointManager waypointManager;
    public float speed;
    private int _targetPointIndex;
    private Transform _previousStop;
    private Transform _nextStop;
    private float _timeToStop;
    private float _elapsed;


    // Start is called before the first frame update
    void Start()
    {
        TargetWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _elapsed += Time.deltaTime;

        float platformJourneyCompletion = _elapsed / _timeToStop;
        platformJourneyCompletion = Mathf.SmoothStep(0, 1, platformJourneyCompletion);
        transform.position = Vector3.Lerp(_previousStop.position, _nextStop.position, platformJourneyCompletion);
        transform.rotation = Quaternion.Lerp(_previousStop.rotation, _nextStop.rotation, platformJourneyCompletion);

        if (platformJourneyCompletion >=1)
        {
            TargetWaypoint();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }

    private void TargetWaypoint()
    {
        _previousStop = waypointManager.GetWayPoint(_targetPointIndex);
        _targetPointIndex = waypointManager.NextWayPointIndex(_targetPointIndex);
        _nextStop = waypointManager.GetWayPoint(_targetPointIndex);

        _elapsed = 0;

        float disanceToWaypoint = Vector3.Distance(_previousStop.position, _nextStop.position);
        _timeToStop = disanceToWaypoint / speed;
    }


}
