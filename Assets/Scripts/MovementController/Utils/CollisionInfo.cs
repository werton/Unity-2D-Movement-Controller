using UnityEngine;

namespace MovementController
{
    /// <summary>
    ///     Contains information about the most recent collision's directions
    /// </summary>
    public struct CollisionDirection
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

    /// <summary>
    ///     Contains information about the most recent collision's slope
    /// </summary>
    public struct CollisionInfo
    {
        public float slopeAngle;
        public Vector2 slopeNormal;
        public bool onWall;
        
        public void Reset()
        {
            slopeAngle = 0;
            slopeNormal = Vector2.zero;
            onWall = false;
        }

        public void SetSlopeAngle(float angle, Vector2 normal, float wallAngle, float wallTolerance)
        {
            slopeAngle = angle;
            slopeNormal = normal;

            bool wallHit = wallAngle - wallTolerance < slopeAngle && slopeAngle < wallAngle + wallTolerance;
            
            if (wallHit)
                onWall = true;
        }
    }    
}
