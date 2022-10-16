/*
 * Class to setup needed raycasts around a BoxCollider2D player for proper collision detection
 */
using UnityEngine;

namespace MovementController
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxColliderCasts : MonoBehaviour
    {
        [field: SerializeField] public LayerMask HorizontalCollisionMask { get; private set; }
        [field: SerializeField] public LayerMask VerticalCollisionMask { get; private set; }
        [field: SerializeField] public float SkinWidth { get; private set; } = .02f;
        [field: SerializeField] public float DistanceBetweenRays { get; private set; } = .2f;  
        
        private BoxCollider2D _boxCollider;
        private BoxCastOrigins _boxCastOrigins;
        private RaycastOrigins _raycastOrigins;
        private int _horizontalRayCount;
        
        protected int VerticalRayCount { get; private set; }
        protected float HorizontalRaySpacing { get; private set; }
        protected float VerticalRaySpacing { get; private set; }
        public float BoundsWidth { get; private set; }
        public float BoundsHeight { get; private set; }

        public BoxCastOrigins BoxCastOrigins => _boxCastOrigins;
        public RaycastOrigins RaycastOrigins => _raycastOrigins; 
        
        public virtual void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        public virtual void Start()
        {
            CalculateRaySpacing();
        }

        protected void UpdateRaycastOrigins()
        {
            var bounds = _boxCollider.bounds;
            // Skin width for ray detection even when boxCollider is flush against surfaces
            bounds.Expand(SkinWidth * -2);

            // Match corners of box boxCollider
            _raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            _raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            _raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            _raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        protected void UpdateBoxCastOrigins()
        {
            var bounds = _boxCollider.bounds;

            _boxCastOrigins.bottomCenter = new Vector2(bounds.center.x, bounds.min.y);
            _boxCastOrigins.topCenter = new Vector2(bounds.center.x, bounds.max.y);
            _boxCastOrigins.leftCenter = new Vector2(bounds.min.x, bounds.center.y);
            _boxCastOrigins.rightCenter = new Vector2(bounds.max.x, bounds.center.y);
        }

        private void CalculateRaySpacing()
        {
            var bounds = _boxCollider.bounds;
            // Skin width for ray detection even when boxCollider is flush against surfaces
            bounds.Expand(SkinWidth * -2);

            BoundsWidth = bounds.size.x;
            BoundsHeight = bounds.size.y;

            _horizontalRayCount = Mathf.RoundToInt(BoundsHeight / DistanceBetweenRays);
            VerticalRayCount = Mathf.RoundToInt(BoundsWidth / DistanceBetweenRays);

            HorizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
            VerticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
        }
    }
}

