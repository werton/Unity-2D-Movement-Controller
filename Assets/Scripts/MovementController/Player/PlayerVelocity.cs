/*
 * Script gets player's intended velocity & displacement (caused by environment variables + user input which is taken from PlayerInput)
 * See for equations/physics: https://en.wikipedia.org/wiki/Equations_of_motion
 * See: http://lolengine.net/blog/2011/12/14/understanding-motion-in-games for Verlet integration vs. Euler
 */

using System;
using UnityEngine;

namespace MovementController
{
    [RequireComponent(typeof(Movement))]
    public class PlayerVelocity : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] private float _moveSpeed = 10;

        [Header("Jumping settings")]
        [SerializeField] private float _maxJumpHeight = 3;
        [SerializeField] private float _minJumpHeight = 2;
        [SerializeField] private float _timeToJumpApex = .4f;

        [Header("Falling settings")]
        [SerializeField] private float _accelerationTimeAirborne = .2f;
        [SerializeField] private float _accelerationTimeGrounded = .1f;
        [SerializeField] private float _forceFallSpeed = 20;

        [Header("Wall stick settings")]
        [SerializeField] private bool _stickToWallEdges = false;
        [SerializeField] private float _wallEdgeDetectRayWidth = .1f;
        
        [Header("Wall jump settings")]
        [SerializeField] private Vector2 _wallJump = new Vector2(15, 15);
        [SerializeField] private Vector2 _wallJumpClimb = new Vector2(5, 15);
        [SerializeField] private Vector2 _wallLeapOff = new Vector2(15, 15);

        [Header("Wall slide settings")]
        [SerializeField] private float _wallSlideMaxSpeed = 3;
        [SerializeField] private float _wallStickBeforeSlideDelay = .15f;
        [SerializeField] private float _wallDropOnBackInputDelay = .25f;

        private Movement _movement;
        private Vector3 _velocity;
        private Vector3 _oldVelocity;
        // private Vector2 _directionalInput;
        private DpadDirection _directionalInput;
        private float _gravity;
        private float _maxJumpVelocity;
        private float _minJumpVelocity;
        private float _wallStickBeforeSlideTime;        
        private float _wallDropOnBackInputTime;
        private float _velocityXSmoothing;
        private int _wallDirX;
        private bool _wallContact;

        private void Start()
        {
            _movement = GetComponent<Movement>();

            // see suvat calculations; s = ut + 1/2at^2, v^2 = u^2 + 2at, where u=0, scalar looking at only y dir
            _gravity = -(2 * _maxJumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
            _maxJumpVelocity = Math.Abs(_gravity) * _timeToJumpApex;
            _minJumpVelocity = Mathf.Sqrt(2 * Math.Abs(_gravity) * _minJumpHeight);
            _wallStickBeforeSlideTime = _wallStickBeforeSlideDelay;            
            _wallDropOnBackInputTime = _wallDropOnBackInputDelay;
        }

        private void Update()
        {
            CalculateVelocity();
            HandleWallSliding();

            _movement.ResetCollisions();

            // r = r0 + 1/2(v+v0)t, note Vector version used here
            // displacement = 1/2(v+v0)t since the playerMovementController uses Translate which moves from r0
            var offset = (_velocity + _oldVelocity) * (0.5f * Time.deltaTime);
            // Move player using movement controller which checks for collisions then applies correct transform (displacement) translation
            _movement.Move(offset, _directionalInput);

            var verticalCollision =
                _movement.CollisionDirection.above || _movement.CollisionDirection.below;

            if (!verticalCollision)
                return;

            if (_movement.SlidingDownMaxSlope)
                _velocity.y += _movement.CollisionInfo.slopeNormal.y * -_gravity * Time.deltaTime;
            else
                _velocity.y = 0;
        }

        private void CalculateVelocity()
        {
            // suvat; s = ut, note a=0
            var targetVelocityX = _directionalInput.X * _moveSpeed;
            _oldVelocity = _velocity;
            // ms when player is on the ground faster vs. in air
            var smoothTime = _movement.CollisionDirection.below
                ? _accelerationTimeGrounded
                : _accelerationTimeAirborne;
            _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, smoothTime);
            _velocity.y += _gravity * Time.deltaTime;
        }

        private void HandleWallSliding()
        {
            _wallDirX = _movement.CollisionDirection.left ? -1 : 1;
            
            var horizontalCollision =
                _movement.CollisionDirection.left || _movement.CollisionDirection.right;

            float rayWidth = _movement.BoundsWidth / 2 + _wallEdgeDetectRayWidth;
            
            Debug.DrawRay(_movement.BoxCastOrigins.bottomCenter,
                rayWidth * _wallDirX * Vector2.right, Color.red);
            
            if (horizontalCollision && !_movement.CollisionDirection.below && !_movement.ForceFall &&
                _movement.CollisionInfo.onWall)
            {
                if (_stickToWallEdges == false)
                {
                    var hit = Physics2D.Raycast(_movement.BoxCastOrigins.bottomCenter,
                        _wallDirX * Vector2.right, rayWidth,
                        _movement.VerticalCollisionMask);

                    if (!hit)
                        return;
                }

                if (_wallContact == false)
                {
                    _wallContact = true;
                    _wallStickBeforeSlideTime = _wallStickBeforeSlideDelay;
                }

                // Check if falling down - only wall slide then
                if (_velocity.y >= 0)
                    return;

                // Grab wall if input facing wall
                if (_directionalInput.X == _wallDirX || _wallStickBeforeSlideTime > 0)
                {
                    _velocity.y = 0;
                    _wallStickBeforeSlideTime -= Time.deltaTime;
                }
                else
                {
                    // Only slow down if falling faster than slide speed
                    if (_velocity.y < -_wallSlideMaxSpeed)
                        _velocity.y = -_wallSlideMaxSpeed;

                        // Stick to wall until timeToWallUnstick has counted down to 0 from wallStickTime
                    if (_wallDropOnBackInputTime > 0)
                    {
                        _velocityXSmoothing = 0;
                        _velocity.x = 0;

                        if (_directionalInput.X == -_wallDirX)
                            _wallDropOnBackInputTime -= Time.deltaTime;
                        else
                            _wallDropOnBackInputTime = _wallDropOnBackInputDelay;
                    }
                }
            }
            else
            {
                _wallContact = false;
                _wallDropOnBackInputTime = _wallDropOnBackInputDelay;
            }
        }

        /* Public Functions used by PlayerInput script */
        
        /// <summary>
        ///     Handle horizontal movement input
        /// </summary>
        public void SetDirectionalInput(Vector2 direction)
        {
            // _directionalInput = input;
            _directionalInput.SetDirection(direction);
        }
        
        /// <summary>
        ///     Handle jumps
        /// </summary>
        public void OnJumpInputDown()
        {
            if (_wallContact)
            {
                // Standard wall jump
                if (_directionalInput.X == 0)
                {
                    _velocity.x = -_wallDirX * _wallJump.x;
                    _velocity.y = _wallJump.y;
                }
                // Climb up if input is facing wall
                else if (_wallDirX == _directionalInput.X)
                {
                    _velocity.x = -_wallDirX * _wallJumpClimb.x;
                    _velocity.y = _wallJumpClimb.y;
                }
                // Leap wall if input facing away from wall
                else
                {
                    _velocity.x = -_wallDirX * _wallLeapOff.x;
                    _velocity.y = _wallLeapOff.y;
                }
            }

            if (_movement.CollisionDirection.below)
            {
                if (_movement.SlidingDownMaxSlope)
                {
                    // Jumping away from max slope dir
                    if (_directionalInput.X != -Math.Sign(_movement.CollisionInfo.slopeNormal.x))
                    {
                        _velocity.y = _maxJumpVelocity * _movement.CollisionInfo.slopeNormal.y;
                        _velocity.x = _maxJumpVelocity * _movement.CollisionInfo.slopeNormal.x;
                    }
                }
                else
                {
                    _velocity.y = _maxJumpVelocity;
                }
            }
        }

        /// <summary>
        ///     Handle not fully commited jumps - allow for mini jumps
        /// </summary>
        public void OnJumpInputUp()
        {
            if (_velocity.y > _minJumpVelocity)
                _velocity.y = _minJumpVelocity;
        }

        /// <summary>
        ///     Handle down direction - force fall
        /// </summary>
        public void OnFallInputDown()
        {
            if (!_movement.CollisionDirection.below)
            {
                _velocity.y = -_forceFallSpeed;
                _movement.SetForceFall();
            }
        }
    }
}