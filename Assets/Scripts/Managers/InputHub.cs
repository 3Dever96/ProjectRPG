using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectRPG.Managers
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputHub : MonoBehaviour
    {
        // Singleton instance
        public static InputHub Instance { get; private set; }

        // Input properties to be read by player
        public Vector2 Move { get { return move; } }
        public Vector2 Look { get { return look; } }
        public bool CycleRight { get { return cycleRight; } }
        public bool CycleLeft { get { return cycleLeft; } }
        public bool Jump { get { return jump; } }
        public bool Attack { get { return attack; } }
        public bool Defend {  get { return defend; } }
        public bool Command {  get { return command; } }
        public bool Sprint {  get { return sprint; } }
        public bool Crouch {  get { return crouch; } }
        public bool Swamp { get { return swap; } }
        public bool Map { get { return map; } }
        public bool Pause {  get { return pause; } }

        // Reference to the player input
        private PlayerInput input;

        // Input values
        private Vector2 move;
        private Vector2 look;
        private bool cycleRight;
        private bool cycleLeft;
        private bool jump;
        private bool attack;
        private bool defend;
        private bool command;
        private bool sprint;
        private bool crouch;
        private bool swap;
        private bool map;
        private bool pause;

        // This function assigns the values to the inputs
        public void OnAction(InputAction.CallbackContext context)
        {
            switch (context.action.name)
            {
                case "Move":
                    move = context.ReadValue<Vector2>();
                    break;
                case "Look":
                    look = context.ReadValue<Vector2>();
                    break;
                case "CycleRight":
                    SetReference(ref cycleRight, context);
                    break;
                case "CycleLeft":
                    SetReference(ref cycleLeft, context);
                    break;
                case "Jump":
                    SetReference(ref jump, context);
                    break;
                case "Attack":
                    SetReference(ref attack, context);
                    break;
                case "Defend":
                    SetReference(ref defend, context);
                    break;
                case "Command":
                    SetReference(ref command, context);
                    break;
                case "Sprint":
                    SetReference(ref sprint, context);
                    break;
                case "Crouch":
                    SetReference(ref crouch, context);
                    break;
                case "Swap":
                    SetReference(ref swap, context);
                    break;
                case "Map":
                    SetReference(ref map, context);
                    break;
                case "Pause":
                    SetReference(ref pause, context);
                    break;
            }
        }

        // This function helps assign values to bool based inputs
        private void SetReference(ref bool value, InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                value = true;
            }

            if (context.canceled)
            {
                value = false;
            }
        }

        // Set singleton
        private void Awake()
        {
            Instance = this;

            input = GetComponent<PlayerInput>();
        }

        // Subscribe to onActionTriggered delegation
        private void OnEnable()
        {
            input.onActionTriggered += OnAction;
        }

        // Unsubscribe from onActionTriggered delegation
        private void OnDisable()
        {
            input.onActionTriggered -= OnAction;
        }
    }
}
