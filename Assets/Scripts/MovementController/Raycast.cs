using UnityEngine;

namespace MovementController
{
    public struct RaycastOrigins
    {
        public Vector2 bottomLeft, bottomRight;
        public Vector2 topLeft, topRight;
    }

    public struct BoxCastOrigins
    {
        public Vector2 bottomCenter, topCenter, leftCenter, rightCenter;
    }
}