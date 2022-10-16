using UnityEngine;

namespace MovementController
{
    public struct DpadDirection
    {
        public DpadDirection(int horizontal = 0, int vertical = 0)
        {
            X = horizontal;
            Y = vertical;
        }
        
        public static readonly DpadDirection None = new DpadDirection();
        public static readonly DpadDirection Left = new DpadDirection(-1, 0);
        public static readonly DpadDirection Right = new DpadDirection(1, 0);
        public static readonly DpadDirection Up = new DpadDirection(0, 1);
        public static readonly DpadDirection Down = new DpadDirection(0, -1);

        public int X { get; private set; }
        public int Y { get; private set; }
        
        public void SetDirection(Vector2 direction)
        {
            X = Mathf.RoundToInt(direction.x);
            Y = Mathf.RoundToInt(direction.y);
        }
    }
}