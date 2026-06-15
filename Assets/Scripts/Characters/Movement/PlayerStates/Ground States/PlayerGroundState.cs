using ProjectRPG.Managers;
using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    [System.Serializable]
    public class PlayerGroundState : PlayerGroundSuperstate
    {
        [Header("Momentum System")]
        [SerializeField] private float baseSpeed;
        [SerializeField] private float baseAccel;
        [SerializeField] private float baseDecel;
        [SerializeField] private float baseFric;
        [SerializeField] private float turnAngle;

        // True speed variables
        private float maxSpeed;
        private float accel;
        private float decel;
        private float fric;

        private float moveSpeed;

        [Header("Vertical Movement")]
        [SerializeField] private float baseJumpSpeed;
        [SerializeField] private float stickForce;

        private float jumpSpeed;
        private bool canJump;

        public override void StartState(PlayerController player)
        {
            player.VerticalSpeed = stickForce;

            canJump = false;
        }

        public override void UpdateState(PlayerController player)
        {
            // Set true speed variables
            if (player.Stats.stats.ContainsKey("AGI"))
            {
                maxSpeed = baseSpeed + 3f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
                accel = baseAccel + 12f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
                decel = baseDecel + 64f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
                fric = baseFric + 6f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));

                jumpSpeed = baseJumpSpeed + 5f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
            }

            // Get input direction
            Transform mainCamera = Camera.main.transform;
            Vector3 direction = mainCamera.right * InputHub.Instance.Move.x + mainCamera.forward * InputHub.Instance.Move.y;
            direction.y = 0f;
            direction = direction.normalized;

            // Momentum Based Movement
            moveSpeed = maxSpeed * InputHub.Instance.Move.magnitude;

            if (InputHub.Instance.Move != Vector2.zero)
            {
                if (Vector3.Angle(direction, player.LookDirection) > turnAngle)
                {
                    if (player.CurrentSpeed > 0f)
                    {
                        player.CurrentSpeed -= decel * Time.deltaTime;
                    }
                    else
                    {
                        player.CurrentSpeed = 0f;
                        player.LookDirection = direction;
                    }
                }
                else
                {
                    if (player.CurrentSpeed < moveSpeed)
                    {
                        player.CurrentSpeed += accel * Time.deltaTime;
                    }
                    else if (player.CurrentSpeed > moveSpeed + 0.1f)
                    {
                        player.CurrentSpeed -= fric * Time.deltaTime;
                    }
                    else
                    {
                        player.CurrentSpeed = moveSpeed;
                    }

                    player.LookDirection = direction;
                }
            }
            else
            {
                player.CurrentSpeed -= Mathf.Min(player.CurrentSpeed, fric * Time.deltaTime);
            }

            player.FaceDirection(player.LookDirection);

            // Jumping
            if (InputHub.Instance.Jump && canJump)
            {
                player.VerticalSpeed = jumpSpeed;
            }

            if (!InputHub.Instance.Jump && !canJump)
            {
                canJump = true;
            }

            // Set Velocity
            Vector3 velocity = player.CurrentSpeed * player.LookDirection;
            velocity.y = player.VerticalSpeed;

            player.ApplyMovement(velocity);
        }

        public override void ChangeState(PlayerController player)
        {
            base.ChangeState(player);
        }

        public override void ExitState(PlayerController player)
        {
            
        }
    }
}
