using UnityEngine;

namespace MovementController
{
    /// <summary>
    ///     Object can be moved by passenger movers
    /// </summary>
    [RequireComponent(typeof(Movement))]
    public class Passenger : MonoBehaviour
    {
        private Movement _movement;
        private PassengerMover _passengerMover;

        private void Awake()
        {
            _movement = GetComponent<Movement>();
            TryGetComponent(out _passengerMover);
        }

        /// <summary>
        ///     Move passenger by given displacement
        /// </summary>
        public void Move(Vector2 offset, bool standingOnPlatform)
        {
            if (_passengerMover)
            {
                _passengerMover.CalculatePassengerMovement(offset);
                _passengerMover.MovePassengers(true);
                MoveTarget(offset);
                _passengerMover.MovePassengers(false);
            }
            else
            {
                MoveTarget(offset);
            }
        }

        private void MoveTarget(Vector2 offset)
        {
            _movement.Move(offset, DpadDirection.None);
        }
    }
}