//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.3
//     from Assets/Input/InputControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControl"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""18c12bce-aebf-4196-ac93-da5c070bb135"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""8a4c4498-60cf-40ae-a337-13ad8dc70b29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""422c02f9-25f5-44fb-8146-6373e10bc01f"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3d78539e-7ab2-4073-a166-613f2b7ff518"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4576bd87-30f7-4e0e-b41b-5ccbf1c15886"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ffc6d363-2f5f-44ba-8246-953765635a81"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4cc2457a-17d1-43c8-b0be-8c07d1f7213a"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""db93c4ff-a7af-447e-8790-5347502060f6"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6d5a08a2-7f6c-4046-afd9-844870660407"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7e720edf-5967-407c-96b1-a1f071681687"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b4bd277d-4df5-4154-8df3-d30d14f789f3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9c5f6691-9a11-4fd6-93bd-9c5c443d6109"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""7029d25a-1f1d-417d-9278-a47039aeeda2"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d2cb77b0-4896-4eb0-bdf2-9897d383d96c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7c73e1fd-417d-4c12-ac22-f1120de0007f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e127fed2-94d3-4594-bf2c-aca2c360696f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2291eae9-fc80-43c9-be56-39626d4dd416"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Game"",
            ""id"": ""9ef1461b-2d0f-4a2e-8119-844ff5296a77"",
            ""actions"": [
                {
                    ""name"": ""ShowInGameMenu"",
                    ""type"": ""Button"",
                    ""id"": ""c628b68b-4879-4cf2-bd3e-aaa9555a2bb3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0b6dffde-d8a4-4057-ba0a-779fd6d1959f"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ShowInGameMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0b9c1b7-bd72-4704-a267-83cba33f518b"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ShowInGameMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9b1def49-32fa-4585-bee8-286a4f947088"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ShowInGameMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""aae422d6-5583-495b-8254-b29aee6cb831"",
            ""actions"": [
                {
                    ""name"": ""SpeedUpGame"",
                    ""type"": ""Button"",
                    ""id"": ""9328c44f-56fb-4445-9d7f-d55bf5f0c5f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpeedDownGame"",
                    ""type"": ""Button"",
                    ""id"": ""7c7a1dc0-249d-427b-9fa0-ce8490a11ce1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SetNormalGameSpeed"",
                    ""type"": ""Button"",
                    ""id"": ""e8aa7123-e3ee-4cfa-a637-e7c6d9d9f862"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ReloadScene"",
                    ""type"": ""Button"",
                    ""id"": ""02a3dd08-f0af-49fa-888b-d1756979cb6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9b2e1c46-ae74-46a9-9065-d1130019e6e2"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SpeedDownGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b479c0b8-ae91-4712-b4ec-e1b6a0c873cf"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SpeedUpGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""761fb8cf-ad65-42c5-b3d2-76310edd3f66"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SetNormalGameSpeed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f59e4ef-5387-49f1-8fc2-f8ffaca08629"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ReloadScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": []
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_ShowInGameMenu = m_Game.FindAction("ShowInGameMenu", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_SpeedUpGame = m_Debug.FindAction("SpeedUpGame", throwIfNotFound: true);
        m_Debug_SpeedDownGame = m_Debug.FindAction("SpeedDownGame", throwIfNotFound: true);
        m_Debug_SetNormalGameSpeed = m_Debug.FindAction("SetNormalGameSpeed", throwIfNotFound: true);
        m_Debug_ReloadScene = m_Debug.FindAction("ReloadScene", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Move;
    public struct PlayerActions
    {
        private @InputControl m_Wrapper;
        public PlayerActions(@InputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Game
    private readonly InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private readonly InputAction m_Game_ShowInGameMenu;
    public struct GameActions
    {
        private @InputControl m_Wrapper;
        public GameActions(@InputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @ShowInGameMenu => m_Wrapper.m_Game_ShowInGameMenu;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @ShowInGameMenu.started -= m_Wrapper.m_GameActionsCallbackInterface.OnShowInGameMenu;
                @ShowInGameMenu.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnShowInGameMenu;
                @ShowInGameMenu.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnShowInGameMenu;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ShowInGameMenu.started += instance.OnShowInGameMenu;
                @ShowInGameMenu.performed += instance.OnShowInGameMenu;
                @ShowInGameMenu.canceled += instance.OnShowInGameMenu;
            }
        }
    }
    public GameActions @Game => new GameActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_SpeedUpGame;
    private readonly InputAction m_Debug_SpeedDownGame;
    private readonly InputAction m_Debug_SetNormalGameSpeed;
    private readonly InputAction m_Debug_ReloadScene;
    public struct DebugActions
    {
        private @InputControl m_Wrapper;
        public DebugActions(@InputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @SpeedUpGame => m_Wrapper.m_Debug_SpeedUpGame;
        public InputAction @SpeedDownGame => m_Wrapper.m_Debug_SpeedDownGame;
        public InputAction @SetNormalGameSpeed => m_Wrapper.m_Debug_SetNormalGameSpeed;
        public InputAction @ReloadScene => m_Wrapper.m_Debug_ReloadScene;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @SpeedUpGame.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnSpeedUpGame;
                @SpeedUpGame.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnSpeedUpGame;
                @SpeedUpGame.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnSpeedUpGame;
                @SpeedDownGame.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnSpeedDownGame;
                @SpeedDownGame.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnSpeedDownGame;
                @SpeedDownGame.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnSpeedDownGame;
                @SetNormalGameSpeed.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnSetNormalGameSpeed;
                @SetNormalGameSpeed.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnSetNormalGameSpeed;
                @SetNormalGameSpeed.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnSetNormalGameSpeed;
                @ReloadScene.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnReloadScene;
                @ReloadScene.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnReloadScene;
                @ReloadScene.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnReloadScene;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SpeedUpGame.started += instance.OnSpeedUpGame;
                @SpeedUpGame.performed += instance.OnSpeedUpGame;
                @SpeedUpGame.canceled += instance.OnSpeedUpGame;
                @SpeedDownGame.started += instance.OnSpeedDownGame;
                @SpeedDownGame.performed += instance.OnSpeedDownGame;
                @SpeedDownGame.canceled += instance.OnSpeedDownGame;
                @SetNormalGameSpeed.started += instance.OnSetNormalGameSpeed;
                @SetNormalGameSpeed.performed += instance.OnSetNormalGameSpeed;
                @SetNormalGameSpeed.canceled += instance.OnSetNormalGameSpeed;
                @ReloadScene.started += instance.OnReloadScene;
                @ReloadScene.performed += instance.OnReloadScene;
                @ReloadScene.canceled += instance.OnReloadScene;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
    }
    public interface IGameActions
    {
        void OnShowInGameMenu(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnSpeedUpGame(InputAction.CallbackContext context);
        void OnSpeedDownGame(InputAction.CallbackContext context);
        void OnSetNormalGameSpeed(InputAction.CallbackContext context);
        void OnReloadScene(InputAction.CallbackContext context);
    }
}
