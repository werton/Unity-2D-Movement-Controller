using UnityEngine;

/// <summary>
///     Object can be moved by passenger movers
/// </summary>
public class Passenger : MonoBehaviour
{
    /// <summary>
    ///     Move passenger by given displacement
    /// </summary>
    public void Move(Vector2 offset, bool standingOnPlatform)
    {
        var passengerMover = gameObject.GetComponent<PassengerMover>();

        if (passengerMover)
        {
            passengerMover.CalculatePassengerMovement(offset);

            passengerMover.MovePassengers(true);
            MoveTarget(offset);
            passengerMover.MovePassengers(false);
        }
        else
        {
            MoveTarget(offset);
        }
    }

    private void MoveTarget(Vector2 displacement)
    {
        if (gameObject.tag == "Player" || gameObject.tag == "Enemy" || gameObject.tag == "Ally")
        {
            var movement = GetComponent<Movement>();
            if (movement)
                movement.Move(displacement, Vector2.zero);
            else
                Debug.LogError("gameObject requires movement script if passenger");
        }
        else
        {
            transform.Translate(displacement);
        }
    }
}