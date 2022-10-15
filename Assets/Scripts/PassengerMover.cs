using System;
using System.Collections.Generic;
using UnityEngine;

public class PassengerMover : BoxColliderCasts
{
    [SerializeField] private LayerMask _passengerMask;
    private readonly Dictionary<Transform, Passenger> _passengerDictionary = new();
    private List<PassengerMovement> _passengerMovement;

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        UpdateBoxCastOrigins();
    }

    public void CalculatePassengerMovement(Vector3 offset)
    {
        var movedPassengers = new HashSet<Transform>();
        _passengerMovement = new List<PassengerMovement>();

        var directionX = Math.Sign(offset.x);
        var directionY = Math.Sign(offset.y);

        // Vertically moving platform
        if (offset.y != 0)
        {
            var rayLength = Mathf.Abs(offset.y);

            var boxRayOrigin = directionY == -1 ? BoxCastOrigins.bottomCenter : BoxCastOrigins.topCenter;
            var boxCastSize = new Vector2(BoundsWidth, SkinWidth);
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(_passengerMask);
            var results = new List<RaycastHit2D>();

            Physics2D.BoxCast(boxRayOrigin, boxCastSize, 0, Vector2.up * directionY, contactFilter2D, results,
                rayLength);

            results.ForEach(delegate(RaycastHit2D hit)
            {
                if (hit)
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        var pushX = directionY == 1 ? offset.x : 0;
                        var pushY = offset.y - hit.distance * directionY;

                        _passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY),
                            directionY == 1, true));
                    }
            });
        }

        // Horizontally moving platform
        if (offset.x != 0)
        {
            var rayLength = Mathf.Abs(offset.x);

            var boxRayOrigin = directionX == -1 ? BoxCastOrigins.leftCenter : BoxCastOrigins.rightCenter;
            var boxCastSize = new Vector2(SkinWidth, BoundsHeight);
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(_passengerMask);
            var results = new List<RaycastHit2D>();

            Physics2D.BoxCast(boxRayOrigin, boxCastSize, 0, Vector2.right * directionX, contactFilter2D, results,
                rayLength);

            results.ForEach(delegate(RaycastHit2D hit)
            {
                if (hit)
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        var pushX = offset.x - (hit.distance - SkinWidth) * directionX;
                        var pushY = -SkinWidth;

                        _passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false,
                            true));
                    }
            });
        }

        // Passenger on top of a horizontally or downward moving platform
        if (directionY == -1 || (offset.y == 0 && offset.x != 0))
        {
            var boxCastSize = new Vector2(BoundsWidth, SkinWidth);
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(_passengerMask);
            var results = new List<RaycastHit2D>();

            Physics2D.BoxCast(BoxCastOrigins.topCenter, boxCastSize, 0, Vector2.up, contactFilter2D, results,
                SkinWidth);

            results.ForEach(delegate(RaycastHit2D hit)
            {
                if (hit)
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        var pushX = offset.x;
                        var pushY = offset.y;

                        _passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true,
                            false));
                    }
            });
        }
    }

    public void MovePassengers(bool beforeMovePlatform)
    {
        foreach (var passenger in _passengerMovement)
        {
            if (!_passengerDictionary.ContainsKey(passenger.transform))
                _passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Passenger>());

            if (passenger.moveBeforePlatform == beforeMovePlatform)
                _passengerDictionary[passenger.transform].Move(passenger.displacement, passenger.standingOnPlatform);
        }
    }

    private struct PassengerMovement
    {
        public readonly Transform transform;
        public readonly Vector3 displacement;
        public readonly bool standingOnPlatform;
        public readonly bool moveBeforePlatform;

        public PassengerMovement(Transform transform, Vector3 displacement, bool standingOnPlatform,
            bool moveBeforePlatform)
        {
            this.transform = transform;
            this.displacement = displacement;
            this.standingOnPlatform = standingOnPlatform;
            this.moveBeforePlatform = moveBeforePlatform;
        }
    }
}