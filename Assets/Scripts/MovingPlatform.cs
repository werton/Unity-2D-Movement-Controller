using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3[] localWaypoints;

    public float speed;
    public bool cyclic;
    public float waitTime;
    [Range(0, 2)] public float easeAmount;

    private int fromWaypointIndex;
    private Vector3[] globalWaypoints;
    private float nextMoveTime;

    private PassengerMover passengerMover;
    private float percentBetweenWaypoints;

    private void Start()
    {
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (var i = 0; i < localWaypoints.Length; i++)
            globalWaypoints[i] = localWaypoints[i] + transform.position;

        passengerMover = GetComponent<PassengerMover>();
    }

    private void Update()
    {
        if (globalWaypoints.Length > 0)
        {
            var displacement = CalculatePlatformMovement();

            if (passengerMover)
            {
                passengerMover.CalculatePassengerMovement(displacement);

                passengerMover.MovePassengers(true);
                transform.Translate(displacement);
                passengerMover.MovePassengers(false);
            }
            else
            {
                transform.Translate(displacement);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            var size = .3f;

            for (var i = 0; i < localWaypoints.Length; i++)
            {
                var globalWaypointPos =
                    Application.isPlaying ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }

    private float Ease(float x)
    {
        var a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    private Vector3 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime)
            return Vector3.zero;

        fromWaypointIndex %= globalWaypoints.Length;
        var toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        var distanceBetweenWaypoints =
            Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        var easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        var newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex],
            easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if (!cyclic)
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    Array.Reverse(globalWaypoints);
                }

            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }
}