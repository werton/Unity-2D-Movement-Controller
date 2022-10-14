using System;
using UnityEngine;

public class MovingPlatform : Platform
{
    [SerializeField] private Vector3[] _localWaypoints;
    [SerializeField] private float _speed = 2;
    [SerializeField] private bool _cyclic = true;
    [SerializeField] private float _waitTime = 0;
    [SerializeField] [Range(0, 2)] private float _easeAmount = 0;
    
    private PassengerMover _passengerMover;
    private Vector3[] _globalWaypoints;
    private float _nextMoveTime;
    private float _percentBetweenWaypoints;
    private int _fromWaypointIndex;
    
    private void Start()
    {
        _globalWaypoints = new Vector3[_localWaypoints.Length];
        for (var i = 0; i < _localWaypoints.Length; i++)
            _globalWaypoints[i] = _localWaypoints[i] + transform.position;

        _passengerMover = GetComponent<PassengerMover>();
    }

    private void Update()
    {
        if (_globalWaypoints.Length > 0)
        {
            var displacement = CalculatePlatformMovement();

            if (_passengerMover)
            {
                _passengerMover.CalculatePassengerMovement(displacement);

                _passengerMover.MovePassengers(true);
                transform.Translate(displacement);
                _passengerMover.MovePassengers(false);
            }
            else
            {
                transform.Translate(displacement);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_localWaypoints != null)
        {
            Gizmos.color = Color.red;
            var size = .3f;

            for (var i = 0; i < _localWaypoints.Length; i++)
            {
                var globalWaypointPos =
                    Application.isPlaying ? _globalWaypoints[i] : _localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }

    private float Ease(float x)
    {
        var a = _easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    private Vector3 CalculatePlatformMovement()
    {
        if (Time.time < _nextMoveTime)
            return Vector3.zero;

        _fromWaypointIndex %= _globalWaypoints.Length;
        var toWaypointIndex = (_fromWaypointIndex + 1) % _globalWaypoints.Length;
        var distanceBetweenWaypoints =
            Vector3.Distance(_globalWaypoints[_fromWaypointIndex], _globalWaypoints[toWaypointIndex]);
        _percentBetweenWaypoints += Time.deltaTime * _speed / distanceBetweenWaypoints;
        _percentBetweenWaypoints = Mathf.Clamp01(_percentBetweenWaypoints);
        var easedPercentBetweenWaypoints = Ease(_percentBetweenWaypoints);

        var newPos = Vector3.Lerp(_globalWaypoints[_fromWaypointIndex], _globalWaypoints[toWaypointIndex],
            easedPercentBetweenWaypoints);

        if (_percentBetweenWaypoints >= 1)
        {
            _percentBetweenWaypoints = 0;
            _fromWaypointIndex++;

            if (!_cyclic)
                if (_fromWaypointIndex >= _globalWaypoints.Length - 1)
                {
                    _fromWaypointIndex = 0;
                    Array.Reverse(_globalWaypoints);
                }

            _nextMoveTime = Time.time + _waitTime;
        }

        return newPos - transform.position;
    }
}