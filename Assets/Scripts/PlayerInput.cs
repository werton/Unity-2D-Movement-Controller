using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerVelocity))]
public class PlayerInput : MonoBehaviour
{
    private PlayerVelocity _playerVelocity;
    private InputControl _input;

    private void Awake()
    {
        _input = new InputControl();
        _input.Player.Jump.performed += OnKeyJump;
        _input.Player.Move.performed += OnKeyMove;
        _input.Player.Move.canceled += OnKeyMove;
        _input.Debug.ReloadScene.performed += context => ReloadScene();
    }

    private void Start()
    {
        _playerVelocity = GetComponent<PlayerVelocity>();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Jump.performed -= OnKeyJump;
        _input.Player.Move.performed -= OnKeyMove;
        _input.Player.Move.canceled -= OnKeyMove;
    }

    private void OnKeyMove(InputAction.CallbackContext context)
    {
        var horizontalDirection = context.ReadValue<Vector2>();

        _playerVelocity.SetDirectionalInput(horizontalDirection);

        if (horizontalDirection.y > 0)
            _playerVelocity.OnJumpInputDown();

        if (horizontalDirection.y < 0)
            _playerVelocity.OnFallInputDown();
    }

    private void OnKeyJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            _playerVelocity.OnJumpInputDown();
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}