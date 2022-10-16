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
                UpdatePassengerInfo(offset.y, offset.x, directionY,  BoxCastOrigins.bottomCenter, BoxCastOrigins.topCenter,
                    new Vector2(BoundsWidth, SkinWidth), Vector2.up, Math.Abs(offset.y), movedPassengers);

            // Horizontally moving platform
            if (offset.x != 0)
                UpdatePassengerInfo(offset.x, offset.y, directionX,  BoxCastOrigins.leftCenter, BoxCastOrigins.rightCenter,
                    new Vector2(SkinWidth, BoundsHeight), Vector2.right, Math.Abs(offset.x), movedPassengers);

            // Passenger on top of a horizontally or downward moving platform
            if (directionY == -1 || (offset.y == 0 && offset.x != 0))
                UpdatePassengerInfo(offset.y, offset.x, directionY,  BoxCastOrigins.topCenter, BoxCastOrigins.topCenter,
                new Vector2(BoundsWidth, SkinWidth), Vector2.down, SkinWidth, movedPassengers);

        }
        
        private void UpdatePassengerInfo(float offset1, float offset2, int direction, Vector2 negativeOrigin, Vector2 positiveOrigin, 
            Vector2 boxCastSize, Vector2 castDirection, float rayLength, HashSet<Transform> movedPassengers)
        {
            Vector2 boxRayOrigin = direction == -1 ? negativeOrigin : positiveOrigin;
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(_passengerMask);
            var results = new List<RaycastHit2D>();

            Physics2D.BoxCast(boxRayOrigin, boxCastSize, 0, castDirection * direction, contactFilter2D, results,
                rayLength);

            results.ForEach(delegate(RaycastHit2D hit)
            {
                if (!hit)
                    return;

                if (movedPassengers.Contains(hit.transform))
                    return;
                    
                movedPassengers.Add(hit.transform);

                float pushX = 0;
                float pushY = 0;

                if (castDirection == Vector2.up)
                {
                    pushX = direction == 1 ? offset2 : 0;
                    pushY = offset1 - hit.distance * direction;
                }
                else if(castDirection == Vector2.right)
                {
                    pushX = offset1 - (hit.distance - SkinWidth) * direction;
                    pushY = -SkinWidth;                    
                }
                else if (castDirection == Vector2.down)
                {
                    pushX = offset2;
                    pushY = offset1;
                }
                
                _passengerInfo.Add(new PassengerInfo(hit.transform, new Vector3(pushX, pushY),
                    true,
                    false));
            });
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