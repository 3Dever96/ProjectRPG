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


        [SerializeField] private PlayerGroundState groundState = new PlayerGroundState();
        [SerializeField] private PlayerAirState airState = new PlayerAirState();

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
