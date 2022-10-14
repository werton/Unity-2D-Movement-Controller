using System;
using System.Collections.Generic;
using UnityEngine;

public class Movement : BoxColliderCasts
{
    private const float WallAngle = 90;
    private const float WallTolerance = 1;
    
    [SerializeField] [Range(0f, WallAngle - WallTolerance)] private float _maxSlopeAngle = 60;
    
    private CollisionInfo _collisionInfo;
    private CollisionDirection _collisionDirection; 
    private int _faceDirection;    
    private bool _descendSlope;
    private bool _passThroughPlatform;
    private bool _ascendSlope;
    private Vector2 _objectInput;  
    
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
    public void Move(Vector2 displacement, Vector2 input)
    {
        ResetDetection();

        _objectInput = input;

        if (displacement.y < 0)
            CheckSlopeDescent(ref displacement);

        // Check face direction - done after slope descent in case of sliding down max slope
        if (displacement.x != 0)
            _faceDirection = Math.Sign(displacement.x);

        CheckHorizontalCollisions(ref displacement, _faceDirection);

        if (displacement.y != 0)
            CheckVerticalCollisions(ref displacement);

        transform.Translate(displacement);

        // Reset grounded variables
        if (_collisionDirection.below)
            ForceFall = false;
    }

    private void ResetDetection()
    {
        UpdateRaycastOrigins();
        UpdateBoxCastOrigins();
        _collisionDirection.Reset();
        _collisionInfo.Reset();
        _ascendSlope = false;
        _descendSlope = false;
        SlidingDownMaxSlope = false;
    }

    /// <summary>
    ///     Check horizontal collisions using box cast (more smooth than ray cast), if angle hit found check for ascent
    /// </summary>
    private void CheckHorizontalCollisions(ref Vector2 displacement, int directionX)
    {
        // Use 2x skin due box cast origin being brought in 
        var rayLength = Math.Abs(displacement.x) + SkinWidth * 2;

        var boxRayOrigin = directionX == -1 ? BoxCastOrigins.leftCenter : BoxCastOrigins.rightCenter;
        boxRayOrigin -= Vector2.right * directionX * SkinWidth;

        var boxCastSize = new Vector2(SkinWidth, BoundsHeight - SkinWidth);

        var contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(collisionMask);

        var results = new List<RaycastHit2D>();

        Physics2D.BoxCast(boxRayOrigin, boxCastSize, 0, Vector2.right * directionX, contactFilter2D, results,
            rayLength);

        for (var i = 0; i < results.Count; i++)
        {
            var hit = results[i];
            
            if (hit == false)
                return;
            
            if (hit.collider.tag == "Through")
                continue;

            var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            _collisionInfo.SetSlopeAngle(slopeAngle, hit.normal, WallAngle, WallTolerance);

            // Calc slope movement logic when first ray hit is an allowed angled
            if (slopeAngle <= _maxSlopeAngle)
            {
                if (_descendSlope)
                    _descendSlope = false;
                CheckSlopeAscent(ref displacement, slopeAngle);
            }

            if (!_ascendSlope || slopeAngle > _maxSlopeAngle)
            {
                // Set displacement be at hit
                displacement.x = hit.distance * directionX;

                // Adjust y accordingly using tan(angle) = O/A, to prevent further ascend when wall hit
                if (_ascendSlope)
                    displacement.y = Mathf.Tan(_collisionInfo.slopeAngle * Mathf.Deg2Rad) *
                                     Mathf.Abs(displacement.x);

                _collisionDirection.left = directionX == -1;
                _collisionDirection.right = directionX == 1;
            }
            
        }
    }

    /// <summary>
    ///     Check vertical collisions using ray cast - not using box cast here as it starts to interfere with horizontal box
    ///     cast / slopes
    /// </summary>
    private void CheckVerticalCollisions(ref Vector2 displacement)
    {
        var directionY = Mathf.Sign(displacement.y);
        var rayLength = Mathf.Abs(displacement.y) + SkinWidth;

        for (var i = 0; i < VerticalRayCount; i++)
        {
            // Send out rays to check for collisions for given layer in y dir, starting based on whether travelling up/down
            var rayOrigin = directionY == -1 ? RaycastOrigins.bottomLeft : RaycastOrigins.topLeft;
            // Note additional distance from movement in x dir needed to adjust rayOrigin correctly
            rayOrigin += Vector2.right * (VerticalRaySpacing * i + displacement.x);
            var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            if (hit)
            {
                // Shows green ray if hit detected
                Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.green);

                // Allow drop/jump through "Through" platforms
                if (hit.collider.tag == "Through")
                {
                    if (directionY == 1 || hit.distance == 0)
                        continue;
                    if (_passThroughPlatform)
                        continue;
                    if (_objectInput.y == -1)
                    {
                        _passThroughPlatform = true;
                        Invoke("ResetPassThroughPlatform", .5f);
                        continue;
                    }
                }

                // Move object to just before the hit ray
                displacement.y = (hit.distance - SkinWidth) * directionY;
                // Adjust ray length to make sure future rays don't lead to further movement past current hit
                rayLength = hit.distance;

                // Adjust x accordingly using tan(angle) = O/A, to prevent further ascend when ceiling hit
                if (_ascendSlope)
                    displacement.x = displacement.y / Mathf.Tan(_collisionInfo.slopeAngle * Mathf.Deg2Rad) *
                                     Mathf.Sign(displacement.x);

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
    private void CheckSlopeAscent(ref Vector2 displacement, float slopeAngle)
    {
        /// Use intended x dir speed for moveDistance (H) up slope 
        var moveDistance = Mathf.Abs(displacement.x);
        /// Work out ascendDisplacementY (O) with Sin(angle)=O/H
        var ascendDisplacementY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        // Check if object is jumping already before ascend
        if (displacement.y <= ascendDisplacementY)
        {
            displacement.y = ascendDisplacementY;
            /// Work out ascendDisplacementX (A) with Cos(angle)=A/H
            displacement.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(displacement.x);
            _collisionDirection.below = true;
            _ascendSlope = true;
        }
    }

    /// <summary>
    ///     Use of trig to work out X/Y components of displacement down slope
    ///     Additional checks done for max slope descent
    /// </summary>
    private void CheckSlopeDescent(ref Vector2 displacement)
    {
        // Check for max slope angle hits, XOR ensures only on side checked at a time
        var maxSlopeHitLeft = Physics2D.Raycast(RaycastOrigins.bottomLeft, Vector2.down,
            Mathf.Abs(displacement.y) + SkinWidth, collisionMask);
        var maxSlopeHitRight = Physics2D.Raycast(RaycastOrigins.bottomRight, Vector2.down,
            Mathf.Abs(displacement.y) + SkinWidth, collisionMask);
        if (maxSlopeHitLeft ^ maxSlopeHitRight)
        {
            if (maxSlopeHitLeft)
                SlideDownMaxSlope(maxSlopeHitLeft, ref displacement);
            else
                SlideDownMaxSlope(maxSlopeHitRight, ref displacement);
        }
        // Fix jittering at the end of sliding from max slope
        else if (maxSlopeHitLeft && maxSlopeHitRight)
        {
            if (maxSlopeHitLeft.distance < maxSlopeHitRight.distance)
                SlideDownMaxSlope(maxSlopeHitLeft, ref displacement);
            else
                SlideDownMaxSlope(maxSlopeHitRight, ref displacement);
        }

        if (!SlidingDownMaxSlope)
        {
            var directionX = Mathf.Sign(displacement.x);
            var rayOrigin = directionX == -1 ? RaycastOrigins.bottomRight : RaycastOrigins.bottomLeft;
            // Cast ray downwards infinitly to check for slope
            var hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if (hit)
            {
                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                _collisionInfo.SetSlopeAngle(slopeAngle, hit.normal, WallAngle, WallTolerance);

                var descendableSlope = slopeAngle != 0 && slopeAngle <= _maxSlopeAngle;
                var moveInSlopeDirection = Mathf.Sign(hit.normal.x) == directionX;
                // Calculate accordingly using tan(angle) = O/A, to prevent further falling when slope hit
                var fallingToSlope = hit.distance - SkinWidth <=
                                     Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(displacement.x);

                if (descendableSlope && moveInSlopeDirection && fallingToSlope)
                {
                    /// Use intended x dir speed for moveDistance (H) down slope 
                    var moveDistance = Mathf.Abs(displacement.x);
                    /// Work out descendDisplacementY (O) with Sin(angle)=O/H
                    var descendDisplacementY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                    /// Work out descendDisplacementX (A) with Cos(angle)=A/H
                    displacement.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(displacement.x);
                    displacement.y -= descendDisplacementY;

                    _descendSlope = true;
                    _collisionDirection.below = true;
                }
            }
        }
    }

    /// <summary>
    ///     Slides down a non-climbable i.e. max slope based on gravity component affecting y
    /// </summary>
    private void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 displacement)
    {
        var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
        _collisionInfo.SetSlopeAngle(slopeAngle, hit.normal, WallAngle, WallTolerance);
        if (slopeAngle > _maxSlopeAngle && slopeAngle < WallAngle - WallTolerance)
        {
            // Calculate accordingly using tan(angle) = O / A, to slide on slope, where x (A), where y (O)
            displacement.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(displacement.y) - hit.distance) /
                             Mathf.Tan(slopeAngle * Mathf.Deg2Rad);
            SlidingDownMaxSlope = true;
        }
    }
    
}