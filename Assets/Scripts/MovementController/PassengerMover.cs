using System;
using System.Collections.Generic;
using UnityEngine;

namespace MovementController
{
    public class PassengerMover : BoxColliderCasts
    {
        [SerializeField] private LayerMask _passengerMask;
        private readonly Dictionary<Transform, Passenger> _passengerDictionary = new();
        private List<PassengerInfo> _passengerInfo;

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
            _passengerInfo = new List<PassengerInfo>();

            int directionX = Math.Sign(offset.x);
            int directionY = Math.Sign(offset.y);

            // Vertically moving platform
            if (offset.y != 0)
            {
                float rayLength = Math.Abs(offset.y);

                Vector2 boxRayOrigin = directionY == -1 ? BoxCastOrigins.bottomCenter : BoxCastOrigins.topCenter;
                Vector2 boxCastSize = new Vector2(BoundsWidth, SkinWidth);
                ContactFilter2D contactFilter2D = new ContactFilter2D();
                contactFilter2D.SetLayerMask(_passengerMask);
                var results = new List<RaycastHit2D>();

                Physics2D.BoxCast(boxRayOrigin, boxCastSize, 0, Vector2.up * directionY, contactFilter2D, results,
                    rayLength);

                results.ForEach(delegate(RaycastHit2D hit)
                {
                    if (!hit)
                        return;

                    if (movedPassengers.Contains(hit.transform))
                        return;
                    
                    movedPassengers.Add(hit.transform);

                    float pushX = directionY == 1 ? offset.x : 0;
                    float pushY = offset.y - hit.distance * directionY;

                    _passengerInfo.Add(new PassengerInfo(hit.transform, new Vector3(pushX, pushY),
                        directionY == 1, true));
                });
            }

            // Horizontally moving platform
            if (offset.x != 0)
            {
                float rayLength = Math.Abs(offset.x);

                Vector2 boxRayOrigin = directionX == -1 ? BoxCastOrigins.leftCenter : BoxCastOrigins.rightCenter;
                Vector2 boxCastSize = new Vector2(SkinWidth, BoundsHeight);
                ContactFilter2D contactFilter2D = new ContactFilter2D();
                contactFilter2D.SetLayerMask(_passengerMask);
                var results = new List<RaycastHit2D>();

                Physics2D.BoxCast(boxRayOrigin, boxCastSize, 0, Vector2.right * directionX, contactFilter2D, results,
                    rayLength);

                results.ForEach(delegate(RaycastHit2D hit)
                {
                    if (!hit)
                        return;

                    if (movedPassengers.Contains(hit.transform))
                        return;
                    
                    movedPassengers.Add(hit.transform);

                    float pushX = offset.x - (hit.distance - SkinWidth) * directionX;
                    float pushY = -SkinWidth;

                    _passengerInfo.Add(new PassengerInfo(hit.transform, new Vector3(pushX, pushY),
                        false,
                        true));
                });
            }

            // Passenger on top of a horizontally or downward moving platform
            if (directionY == -1 || (offset.y == 0 && offset.x != 0))
            {
                Vector2 boxCastSize = new Vector2(BoundsWidth, SkinWidth);
                ContactFilter2D contactFilter2D = new ContactFilter2D();
                contactFilter2D.SetLayerMask(_passengerMask);
                var results = new List<RaycastHit2D>();

                Physics2D.BoxCast(BoxCastOrigins.topCenter, boxCastSize, 0, Vector2.up, contactFilter2D, results,
                    SkinWidth);

                results.ForEach(delegate(RaycastHit2D hit)
                {
                    if (!hit)
                        return;

                    if (movedPassengers.Contains(hit.transform))
                        return;
                    
                    movedPassengers.Add(hit.transform);
                    
                    float pushX = offset.x;
                    float pushY = offset.y;

                    _passengerInfo.Add(new PassengerInfo(hit.transform, new Vector3(pushX, pushY), true,
                        false));
                });
            }
        }

        public void MovePassengers(bool beforeMovePlatform)
        {
            foreach (var passenger in _passengerInfo)
            {
                if (!_passengerDictionary.ContainsKey(passenger.transform))
                    _passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Passenger>());

                if (passenger.moveBeforePlatform == beforeMovePlatform)
                    _passengerDictionary[passenger.transform]
                        .Move(passenger.offset, passenger.standingOnPlatform);
            }
        }

        private struct PassengerInfo
        {
            public readonly Transform transform;
            public readonly Vector3 offset;
            public readonly bool standingOnPlatform;
            public readonly bool moveBeforePlatform;

            public PassengerInfo(Transform transform, Vector3 offset, bool standingOnPlatform,
                bool moveBeforePlatform)
            {
                this.transform = transform;
                this.offset = offset;
                this.standingOnPlatform = standingOnPlatform;
                this.moveBeforePlatform = moveBeforePlatform;
            }
        }
    }
}