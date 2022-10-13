using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerVelocity))]
public class PlayerInput : MonoBehaviour
{
    private PlayerVelocity _playerVelocity;
    public InputControl Input { get; private set; }

    private void Awake()
    {
        Input = new InputControl();
        Input.Player.Jump.performed += OnKeyJump;
        Input.Player.Move.performed += OnKeyMove;
        Input.Player.Move.canceled += OnKeyMove;
    }

    private void Start()
    {
        _playerVelocity = GetComponent<PlayerVelocity>();
    }

    private void OnEnable()
    {
        Input.Enable();
    }

    private void OnDisable()
    {
        Input.Disable();
        Input.Player.Jump.performed -= OnKeyJump;
        Input.Player.Move.performed -= OnKeyMove;
        Input.Player.Move.canceled -= OnKeyMove;
    }

    public void OnKeyMove(InputAction.CallbackContext context)
    {
        var horizontalDirection = context.ReadValue<Vector2>();

        _playerVelocity.SetDirectionalInput(horizontalDirection);

        if (horizontalDirection.y > 0)
            _playerVelocity.OnJumpInputDown();

        if (horizontalDirection.y < 0)
            _playerVelocity.OnFallInputDown();
    }

    public void OnKeyJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            _playerVelocity.OnJumpInputDown();
    }
}