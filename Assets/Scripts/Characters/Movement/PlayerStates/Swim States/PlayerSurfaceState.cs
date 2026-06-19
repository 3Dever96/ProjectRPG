using ProjectRPG.Managers;
using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    [System.Serializable]
    public class PlayerSurfaceState : PlayerState
    {
        [SerializeField] private float maxSwimSpeed;
        [SerializeField] private float swimAccel;
        [SerializeField] private float swimDecel;
        [SerializeField] private float swimFric;

        [SerializeField] private float turnSpeed;

        private float maxSpeed;
        private float accel;
        private float decel;
        private float fric;

        private float moveSpeed;

        private float jumpSpeed;

        private Vector3 direction;

        private bool canJump;

        public override void StartState(PlayerController player)
        {
            player.Controller.enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.WaterLevel - (player.Controller.height / 2f), player.transform.position.z);
            player.Controller.enabled = true;

            player.VerticalSpeed = 0f;

            canJump = false;
        }

        public override void UpdateState(PlayerController player)
        {
            if (player.Stats.stats.ContainsKey("AGI"))
            {
                float agi = player.Stats.stats["AGI"];

                maxSpeed = maxSwimSpeed + 3f * (agi / (agi + 150f));
                accel = swimAccel + 6f * (agi / (agi + 150f));
                decel = swimDecel + 32f * (agi / (agi * 150f));
                fric = swimFric + 3f * (agi / (agi + 150f));

                jumpSpeed = (player.BaseJumpSpeed * 1.2f) + 5f * (agi / (agi + 150f));
            }

            // Get input direction
            Transform mainCamera = Camera.main.transform;
            direction = mainCamera.right * InputHub.Instance.Move.x + mainCamera.forward * InputHub.Instance.Move.y;
            direction.y = 0f;
            direction = direction.normalized;

            // Momentum Based Movement
            moveSpeed = maxSpeed * InputHub.Instance.Move.magnitude;

            if (InputHub.Instance.Move != Vector2.zero)
            {
                if (player.CurrentSpeed < moveSpeed)
                {
                    player.CurrentSpeed += accel * Time.deltaTime;
                }
                else if (player.CurrentSpeed > moveSpeed + 0.1f)
                {
                    player.CurrentSpeed -= decel * Time.deltaTime;
                }
                else
                {
                    player.CurrentSpeed = moveSpeed;
                }

                player.LookDirection = direction;
            }
            else
            {
                player.CurrentSpeed -= Mathf.Min(fric * Time.deltaTime, player.CurrentSpeed);
            }

            player.FaceDirection(player.LookDirection, turnSpeed);

            if (player.Stats.skills.Contains("Jump"))
            {
                if (InputHub.Instance.Jump && canJump)
                {
                    player.VerticalSpeed = jumpSpeed;
                }

                if (!InputHub.Instance.Jump && !canJump)
                {
                    canJump = true;
                }
            }

            player.ApplyMovement(player.transform.forward);
        }

        public override void ChangeState(PlayerController player)
        {
            if (player.TouchingWater && player.transform.position.y + (player.Controller.height / 2f) > player.WaterLevel)
            {
                if (player.VerticalSpeed == 0f)
                {
                    player.SetState(player.GroundState);
                }
                else if (player.VerticalSpeed > 0f)
                {
                    player.SetState(player.AirState);
                }
            }

            if (InputHub.Instance.Crouch)
            {
                player.VerticalSpeed = -maxSpeed;
                player.SetState(player.SubmergedState);
            }
        }

        public override void ExitState(PlayerController player)
        {
            
        }
    }
}
