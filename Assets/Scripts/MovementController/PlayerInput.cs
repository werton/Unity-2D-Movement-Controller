using System.IO.MemoryMappedFiles;
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
                    _playerVelocity.OnJumpInputDown();

            if (context.canceled)
                _playerVelocity.OnJumpInputUp();

            if (horizontalDirection.y < 0)
                _playerVelocity.OnFallInputDown();
        }

        private void OnKeyJump(InputAction.CallbackContext context)
        {
            if (context.started)
                _playerVelocity.OnJumpInputDown();

            if (context.canceled)
                _playerVelocity.OnJumpInputUp();
        }

        private void ReloadScene(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
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