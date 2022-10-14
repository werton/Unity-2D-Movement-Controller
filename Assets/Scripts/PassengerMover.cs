using System.Collections.Generic;
using UnityEngine;

public class PassengerMover : BoxColliderCasts
{
    public LayerMask passengerMask;
    private readonly Dictionary<Transform, Passenger> passengerDictionary = new();

    private List<PassengerMovement> passengerMovement;

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        UpdateBoxCastOrigins();
    }

    public void CalculatePassengerMovement(Vector3 displacement)
    {
        var movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        var directionX = Mathf.Sign(displacement.x);
        var directionY = Mathf.Sign(displacement.y);

        // Vertically moving platform
        if (displacement.y != 0)
        {
            var rayLength = Mathf.Abs(displacement.y);

            var boxRayOrigin = directionY == -1 ? BoxCastOrigins.bottomCenter : BoxCastOrigins.topCenter;
            var boxCastSize = new Vector2(BoundsWidth, SkinWidth);
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(passengerMask);
            var results = new List<RaycastHit2D>();

            Physics2D.BoxCast(boxRayOrigin, boxCastSize, 0, Vector2.up * directionY, contactFilter2D, results,
                rayLength);

            results.ForEach(delegate(RaycastHit2D hit)
            {
                if (hit)
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        var pushX = directionY == 1 ? displacement.x : 0;
                        var pushY = displacement.y - hit.distance * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY),
                            directionY == 1, true));
                    }
            });
        }

        // Horizontally moving platform
        if (displacement.x != 0)
        {
            var rayLength = Mathf.Abs(displacement.x);

            var boxRayOrigin = directionX == -1 ? BoxCastOrigins.leftCenter : BoxCastOrigins.rightCenter;
            var boxCastSize = new Vector2(SkinWidth, BoundsHeight);
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(passengerMask);
            var results = new List<RaycastHit2D>();

            Physics2D.BoxCast(boxRayOrigin, boxCastSize, 0, Vector2.right * directionX, contactFilter2D, results,
                rayLength);

            results.ForEach(delegate(RaycastHit2D hit)
            {
                if (hit)
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        var pushX = displacement.x - (hit.distance - SkinWidth) * directionX;
                        var pushY = -SkinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false,
                            true));
                    }
            });
        }

        // Passenger on top of a horizontally or downward moving platform
        if (directionY == -1 || (displacement.y == 0 && displacement.x != 0))
        {
            var boxCastSize = new Vector2(BoundsWidth, SkinWidth);
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(passengerMask);
            var results = new List<RaycastHit2D>();

            Physics2D.BoxCast(BoxCastOrigins.topCenter, boxCastSize, 0, Vector2.up, contactFilter2D, results,
                SkinWidth);

            results.ForEach(delegate(RaycastHit2D hit)
            {
                if (hit)
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        var pushX = displacement.x;
                        var pushY = displacement.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true,
                            false));
                    }
            });
        }
    }

    public void MovePassengers(bool beforeMovePlatform)
    {
        foreach (var passenger in passengerMovement)
        {
            if (!passengerDictionary.ContainsKey(passenger.transform))
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Passenger>());

            if (passenger.moveBeforePlatform == beforeMovePlatform)
                passengerDictionary[passenger.transform].Move(passenger.displacement, passenger.standingOnPlatform);
        }
    }

    private struct PassengerMovement
    {
        public readonly Transform transform;
        public readonly Vector3 displacement;
        public readonly bool standingOnPlatform;
        public readonly bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _displacement, bool _standingOnPlatform,
            bool _moveBeforePlatform)
        {
            transform = _transform;
            displacement = _displacement;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }
}