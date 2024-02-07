//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Scripts/PlayerControls.inputactions
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

namespace Samurai
{
    public partial class @PlayerControls: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerMap"",
            ""id"": ""745f47f1-fc2b-4685-921f-6e4c4ae1ec8b"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""2b66b1e9-2da0-43e9-81e8-1fb3303c7c6a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""f86f8919-b842-4adc-8181-03871c773961"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""BlueColor"",
                    ""type"": ""Button"",
                    ""id"": ""cff0ed7f-2416-4019-9add-c737a978e00d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RedColor"",
                    ""type"": ""Button"",
                    ""id"": ""9e806afb-cb39-432d-89d1-a00d20cd9a50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Move"",
                    ""id"": ""fa8861c9-4f0f-48b4-a24a-38cd2de1b939"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a236350b-1581-4bc7-bd4e-75f3c8cc908f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e464fd6f-5060-408e-805c-e9d413cf776a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fc5c5040-6597-43ad-ba02-371542523c20"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0851a1d5-2dc6-4a93-9c95-94535e7e877c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""af2d5de7-414c-468c-b01e-08ce323695a7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a2c94d6-f969-4c50-b573-b31a81abcbfb"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RedColor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b3888261-6124-454c-80e4-92117521c811"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BlueColor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // PlayerMap
            m_PlayerMap = asset.FindActionMap("PlayerMap", throwIfNotFound: true);
            m_PlayerMap_Movement = m_PlayerMap.FindAction("Movement", throwIfNotFound: true);
            m_PlayerMap_Shoot = m_PlayerMap.FindAction("Shoot", throwIfNotFound: true);
            m_PlayerMap_BlueColor = m_PlayerMap.FindAction("BlueColor", throwIfNotFound: true);
            m_PlayerMap_RedColor = m_PlayerMap.FindAction("RedColor", throwIfNotFound: true);
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

        // PlayerMap
        private readonly InputActionMap m_PlayerMap;
        private List<IPlayerMapActions> m_PlayerMapActionsCallbackInterfaces = new List<IPlayerMapActions>();
        private readonly InputAction m_PlayerMap_Movement;
        private readonly InputAction m_PlayerMap_Shoot;
        private readonly InputAction m_PlayerMap_BlueColor;
        private readonly InputAction m_PlayerMap_RedColor;
        public struct PlayerMapActions
        {
            private @PlayerControls m_Wrapper;
            public PlayerMapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_PlayerMap_Movement;
            public InputAction @Shoot => m_Wrapper.m_PlayerMap_Shoot;
            public InputAction @BlueColor => m_Wrapper.m_PlayerMap_BlueColor;
            public InputAction @RedColor => m_Wrapper.m_PlayerMap_RedColor;
            public InputActionMap Get() { return m_Wrapper.m_PlayerMap; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerMapActions set) { return set.Get(); }
            public void AddCallbacks(IPlayerMapActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerMapActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerMapActionsCallbackInterfaces.Add(instance);
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @BlueColor.started += instance.OnBlueColor;
                @BlueColor.performed += instance.OnBlueColor;
                @BlueColor.canceled += instance.OnBlueColor;
                @RedColor.started += instance.OnRedColor;
                @RedColor.performed += instance.OnRedColor;
                @RedColor.canceled += instance.OnRedColor;
            }

            private void UnregisterCallbacks(IPlayerMapActions instance)
            {
                @Movement.started -= instance.OnMovement;
                @Movement.performed -= instance.OnMovement;
                @Movement.canceled -= instance.OnMovement;
                @Shoot.started -= instance.OnShoot;
                @Shoot.performed -= instance.OnShoot;
                @Shoot.canceled -= instance.OnShoot;
                @BlueColor.started -= instance.OnBlueColor;
                @BlueColor.performed -= instance.OnBlueColor;
                @BlueColor.canceled -= instance.OnBlueColor;
                @RedColor.started -= instance.OnRedColor;
                @RedColor.performed -= instance.OnRedColor;
                @RedColor.canceled -= instance.OnRedColor;
            }

            public void RemoveCallbacks(IPlayerMapActions instance)
            {
                if (m_Wrapper.m_PlayerMapActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPlayerMapActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerMapActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerMapActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PlayerMapActions @PlayerMap => new PlayerMapActions(this);
        public interface IPlayerMapActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnShoot(InputAction.CallbackContext context);
            void OnBlueColor(InputAction.CallbackContext context);
            void OnRedColor(InputAction.CallbackContext context);
        }
    }
}
