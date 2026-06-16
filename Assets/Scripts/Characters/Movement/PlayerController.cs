using ProjectRPG.Characters.Combat;
using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    public class PlayerController : MovementController
    {
        public PlayerStats Stats { get; private set; }

        public PlayerState CurrentState { get; private set; }
        public PlayerGroundState GroundState { get { return groundState; } }
        public PlayerAirState AirState { get { return airState; } }
        public PlayerSprintState SprintState { get { return sprintState; } }

        public float BaseRunSpeed {  get { return baseRunSpeed; } }
        public float BaseAccel {  get { return baseAccel; } }
        public float BaseDecel {  get { return baseDecel; } }
        public float BaseFric { get { return baseFric; } }
        public float TurnAngle { get { return turnAngle; } }

        public float BaseJumpSpeed {  get { return baseJumpSpeed; } }
        public float StickForce {  get { return stickForce; } }
        public float Gravity { get { return gravity; } }
        public float FallSpeed { get { return fallSpeed; } }

        public float BaseSprintSpeed {  get { return baseSprintSpeed; } }

        [SerializeField] private PlayerGroundState groundState = new PlayerGroundState();
        [SerializeField] private PlayerAirState airState = new PlayerAirState();
        [SerializeField] private PlayerSprintState sprintState = new PlayerSprintState();

        [Header("Momentum System")]
        [SerializeField] private float baseRunSpeed;
        [SerializeField] private float baseAccel;
        [SerializeField] private float baseDecel;
        [SerializeField] private float baseFric;
        [SerializeField] private float turnAngle;

        [Header("Vertical Movement")]
        [SerializeField] private float baseJumpSpeed;
        [SerializeField] private float stickForce;
        [SerializeField] private float gravity;
        [SerializeField] private float fallSpeed;

        [Header("Sprint Movement")]
        [SerializeField] private float baseSprintSpeed;

        protected override void Start()
        {
            base.Start();

            Stats = GetComponent<PlayerStats>();

            SetState(GroundState);
        }

        private void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.UpdateState(this);
                CurrentState.ChangeState(this);
            }
        }

        public void SetState(PlayerState newState)
        {
            if (CurrentState != null)
            {
                CurrentState.ExitState(this);
            }

            CurrentState = newState;

            if (CurrentState != null)
            {
                CurrentState.StartState(this);
            }
        }
    }
}
