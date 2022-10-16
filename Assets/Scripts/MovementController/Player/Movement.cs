using System;
using UnityEngine;

namespace MovementController
{
    public class Movement : BoxColliderCasts
    {
        private const float WallAngle = 90;
        private const float WallTolerance = 1;
        private const float ShellRadius = .01f;

        [SerializeField] [Range(0f, WallAngle - WallTolerance)]
        private float _maxSlopeAngle = 60;

        private CollisionInfo _collisionInfo;
        private CollisionDirection _collisionDirection;
        private int _faceDirection;
        private bool _descendSlope;
        private bool _passThroughPlatform;
        private bool _ascendSlope;
        private DpadDirection _objectInput;
        private RaycastHit2D[] _raycastHits = new RaycastHit2D[1];


        public bool ForceFall { get; private set; }
        public bool SlidingDownMaxSlope { get; private set; }
        public CollisionInfo CollisionInfo => _collisionInfo;
        public CollisionDirection CollisionDirection => _collisionDirection;

        public override void Start()
        {
            base.Start();
        }

        public void SetForceFall()
        {
            ForceFall = true;
        }

        /// <summary>
        ///     Checks for collisions then applies correct transform translation to move object
        /// </summary>
        public void Move(Vector2 offset, DpadDirection input)
        {
            ResetDetection();

            _objectInput = input;

            if (offset.y < 0)
                CheckSlopeDescent(ref offset);

            // Check face direction - done after slope descent in case of sliding down max slope
            if (offset.x != 0)
            {
                _faceDirection = Math.Sign(offset.x);
            }

            TryMoveHorizontally(ref offset, _faceDirection);

            if (offset.y != 0)
                TryMoveVertically(ref offset);

            transform.Translate(offset);

            // Reset grounded variables
            if (_collisionDirection.below)
                ForceFall = false;
        }

        private void ResetDetection()
        {
            UpdateRaycastOrigins();
            UpdateBoxCastOrigins();
            // _collisionDirection.Reset();
            _collisionInfo.Reset();
            _ascendSlope = false;
            _descendSlope = false;
            SlidingDownMaxSlope = false;
        }

        public void ResetCollisions()
        {
            _collisionDirection.Reset();
        }

        /// <summary>
        ///     Check horizontal collisions using box cast (more smooth than ray cast), if angle hit found check for ascent
        /// </summary>
        private void TryMoveHorizontally(ref Vector2 offset, int directionX)
        {
            // Use 2x skin due box cast origin being brought in 
            var rayLength = Math.Abs(offset.x) + SkinWidth * 2;

            var boxRayOrigin = directionX == -1 ? BoxCastOrigins.leftCenter : BoxCastOrigins.rightCenter;
            boxRayOrigin -= Vector2.right * (directionX * SkinWidth);

            var boxCastSize = new Vector2(SkinWidth, BoundsHeight - SkinWidth);

            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(HorizontalCollisionMask);

            int raycastHit2D = Physics2D.BoxCast(boxRayOrigin, boxCastSize, 0,
                Vector2.right * directionX, contactFilter2D, _raycastHits, rayLength + ShellRadius);

            if (raycastHit2D == 0)
                return;

            foreach (var hit in _raycastHits)
            {
                if (hit == false)
                    return;
                
                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                _collisionInfo.SetSlopeAngle(slopeAngle, hit.normal, WallAngle, WallTolerance);

                // Calc slope movement logic when first ray hit is an allowed angled
                if (slopeAngle <= _maxSlopeAngle)
                {
                    if (_descendSlope)
                        _descendSlope = false;
                    CheckSlopeAscent(ref offset, slopeAngle);
                }

                if (!_ascendSlope || slopeAngle > _maxSlopeAngle)
                {
                    // Set displacement be at hit
                    if (slopeAngle < WallAngle)
                        offset.x = hit.distance * directionX;
                    else
                        offset.x = (hit.distance - ShellRadius) * directionX;
                    
                    // Adjust y accordingly using tan(angle) = O/A, to prevent further ascend when wall hit
                    if (_ascendSlope)
                        offset.y = Mathf.Tan(_collisionInfo.slopeAngle * Mathf.Deg2Rad) *
                                   Math.Abs(offset.x);

                    _collisionDirection.left = directionX == -1;
                    _collisionDirection.right = directionX == 1;
                }
            }
        }

        /// <summary>
        ///     Check vertical collisions using ray cast - not using box cast here as it starts to interfere with horizontal box
        ///     cast / slopes
        /// </summary>
        private void TryMoveVertically(ref Vector2 offset)
        {
            int directionY = Math.Sign(offset.y);
            float rayLength = Math.Abs(offset.y) + SkinWidth;

            for (var i = 0; i < VerticalRayCount; i++)
            {
                // Send out rays to check for collisions for given layer in y dir, starting based on whether travelling up/down
                Vector2 rayOrigin = directionY == -1 ? RaycastOrigins.bottomLeft : RaycastOrigins.topLeft;
                // Note additional distance from movement in x dir needed to adjust rayOrigin correctly
                rayOrigin += Vector2.right * (VerticalRaySpacing * i + offset.x);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, VerticalCollisionMask);

                if (hit)
                {
                    // Shows green ray if hit detected
                    Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.green);

                    // Allow drop/jump through "Through" platforms
                    if (hit.collider.TryGetComponent(out Platform platform))
                    {
                        if (directionY == 1 || hit.distance == 0)
                            continue;
                        
                        if (_passThroughPlatform)
                            continue;
                        
                        if (_objectInput.Y == -1)
                        {
                            _passThroughPlatform = true;
                            Invoke(nameof(ResetPassThroughPlatform), .5f);
                            continue;
                        }
                    }

                    // Move object to just before the hit ray
                    offset.y = (hit.distance - SkinWidth) * directionY;
                    // Adjust ray length to make sure future rays don't lead to further movement past current hit
                    rayLength = hit.distance;

                    // Adjust x accordingly using tan(angle) = O/A, to prevent further ascend when ceiling hit
                    if (_ascendSlope)
                        offset.x = offset.y / Mathf.Tan(_collisionInfo.slopeAngle * Mathf.Deg2Rad) *
                                         Math.Sign(offset.x);

                    _collisionDirection.below = directionY == -1;
                    _collisionDirection.above = directionY == 1;
                }
                else
                {
                    // Draw remaining rays being checked
                    Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);
                }
            }
        }

        private void ResetPassThroughPlatform()
        {
            _passThroughPlatform = false;
        }

        /// <summary>
        ///     Use of trig to work out X/Y components of displacement up slope
        /// </summary>
        private void CheckSlopeAscent(ref Vector2 offset, float slopeAngle)
        {
            // Use intended x dir speed for moveDistance (H) up slope 
            var moveDistance = Math.Abs(offset.x);
            // Work out ascendDisplacementY (O) with Sin(angle)=O/H
            var ascendOffsetY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

            // Check if object is jumping already before ascend
            if (offset.y <= ascendOffsetY)
            {
                offset.y = ascendOffsetY;
                // Work out ascendDisplacementX (A) with Cos(angle)=A/H
                offset.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(offset.x);
                _collisionDirection.below = true;
                _ascendSlope = true;
            }
        }

        /// <summary>
        ///     Use of trig to work out X/Y components of displacement down slope
        ///     Additional checks done for max slope descent
        /// </summary>
        private void CheckSlopeDescent(ref Vector2 offset)
        {
            // Check for max slope angle hits, XOR ensures only on side checked at a time
            var maxSlopeHitLeft = Physics2D.Raycast(RaycastOrigins.bottomLeft, Vector2.down,
                Math.Abs(offset.y) + SkinWidth, VerticalCollisionMask);
            var maxSlopeHitRight = Physics2D.Raycast(RaycastOrigins.bottomRight, Vector2.down,
                Math.Abs(offset.y) + SkinWidth, VerticalCollisionMask);

            if (maxSlopeHitLeft ^ maxSlopeHitRight)
            {
                if (maxSlopeHitLeft)
                    SlideDownMaxSlope(maxSlopeHitLeft, ref offset);
                else
                    SlideDownMaxSlope(maxSlopeHitRight, ref offset);
            }
            // Fix jittering at the end of sliding from max slope
            else if (maxSlopeHitLeft && maxSlopeHitRight)
            {
                if (maxSlopeHitLeft.distance < maxSlopeHitRight.distance)
                    SlideDownMaxSlope(maxSlopeHitLeft, ref offset);
                else
                    SlideDownMaxSlope(maxSlopeHitRight, ref offset);
            }

            if (!SlidingDownMaxSlope)
            {
                var directionX = Math.Sign(offset.x);
                var rayOrigin = directionX == -1 ? RaycastOrigins.bottomRight : RaycastOrigins.bottomLeft;
                // Cast ray downwards infinitely to check for slope
                var hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, VerticalCollisionMask);

                if (hit)
                {
                    var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    _collisionInfo.SetSlopeAngle(slopeAngle, hit.normal, WallAngle, WallTolerance);

                    var descendableSlope = slopeAngle != 0 && slopeAngle <= _maxSlopeAngle;
                    var moveInSlopeDirection = Math.Sign(hit.normal.x) == directionX;
                    // Calculate accordingly using tan(angle) = O/A, to prevent further falling when slope hit
                    var fallingToSlope = hit.distance - SkinWidth <=
                                         Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(offset.x);

                    if (descendableSlope && moveInSlopeDirection && fallingToSlope)
                    {
                        // Use intended x dir speed for moveDistance (H) down slope 
                        var moveDistance = Math.Abs(offset.x);
                        // Work out descendDisplacementY (O) with Sin(angle)=O/H
                        var descendOffsetY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        // Work out descendDisplacementX (A) with Cos(angle)=A/H
                        offset.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance *
                                         Math.Sign(offset.x);
                        offset.y -= descendOffsetY;

                        _descendSlope = true;
                        _collisionDirection.below = true;
                    }
                }
            }
        }

        /// <summary>
        ///     Slides down a non-climbable i.e. max slope based on gravity component affecting y
        /// </summary>
        private void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 offset)
        {
            var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            _collisionInfo.SetSlopeAngle(slopeAngle, hit.normal, WallAngle, WallTolerance);
            if (slopeAngle > _maxSlopeAngle && slopeAngle < WallAngle - WallTolerance)
            {
                // Calculate accordingly using tan(angle) = O / A, to slide on slope, where x (A), where y (O)
                offset.x = Math.Sign(hit.normal.x) * (Math.Abs(offset.y) - hit.distance) /
                                 Mathf.Tan(slopeAngle * Mathf.Deg2Rad);
                SlidingDownMaxSlope = true;
            }
        }
    }
}