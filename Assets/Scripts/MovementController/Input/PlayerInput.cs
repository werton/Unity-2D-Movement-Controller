using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MovementController
{
    [RequireComponent(typeof(PlayerVelocity))]

    public class PlayerInput : MonoBehaviour
    {
        private PlayerVelocity _playerVelocity;
        private InputControl _input;
        public DpadDirection DpadDirection { get; private set; }

        private void Awake()
        {
            _input = new InputControl();
            _input.Player.Jump.started += OnKeyJump;
            _input.Player.Jump.canceled += OnKeyJump;
            _input.Player.Move.started += OnKeyMove;
            _input.Player.Move.canceled += OnKeyMove;
            _input.Debug.ReloadScene.started += ReloadScene;
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
            _input.Player.Jump.started -= OnKeyJump;
            _input.Player.Jump.canceled -= OnKeyJump;
            _input.Player.Move.started -= OnKeyMove;
            _input.Player.Move.canceled -= OnKeyMove;
            _input.Debug.ReloadScene.started -= ReloadScene;
        }

        private void OnKeyMove(InputAction.CallbackContext context)
        {
            var horizontalDirection = context.ReadValue<Vector2>();
            _playerVelocity.SetDirectionalInput(horizontalDirection);
            

            if (horizontalDirection.y > 0)
                if (context.started)
                    _playerVelocity.JumpStart();

            if (context.canceled)
                _playerVelocity.JumpStop();

            if (horizontalDirection.y < 0)
                _playerVelocity.FallDown();
        }

        private void OnKeyJump(InputAction.CallbackContext context)
        {
            if (context.started)
                _playerVelocity.JumpStart();

            if (context.canceled)
                _playerVelocity.JumpStop();
        }

        private void ReloadScene(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}