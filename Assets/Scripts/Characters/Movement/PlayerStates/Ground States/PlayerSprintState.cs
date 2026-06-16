using log4net.Core;
using ProjectRPG.Managers;
using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    [System.Serializable]
    public class PlayerSprintState : PlayerGroundSuperstate
    {
        [SerializeField] private float staminaRate;

        private float sprintSpeed;
        private float jumpSpeed;

        private bool canJump;

        Vector3 direction;

        public override void StartState(PlayerController player)
        {
            player.Stats.canRegenSp = false;
            player.VerticalSpeed = player.StickForce;

            canJump = false;
        }

        public override void UpdateState(PlayerController player)
        {
            player.Stats.currentSP -= staminaRate * Time.deltaTime;

            if (player.Stats.stats.ContainsKey("AGI"))
            {
                sprintSpeed = player.BaseSprintSpeed + 2f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
                jumpSpeed = player.BaseJumpSpeed + 5f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
            }

            // Get input direction
            Transform mainCamera = Camera.main.transform;
            direction = mainCamera.right * InputHub.Instance.Move.x + mainCamera.forward * InputHub.Instance.Move.y;
            direction.y = 0f;
            direction = direction.normalized;

            if (Vector3.Angle(direction, player.LookDirection) < player.TurnAngle)
            {
                player.LookDirection = direction;
            }

            player.FaceDirection(player.LookDirection);

            player.CurrentSpeed = sprintSpeed;

            // Jumping
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

            player.ApplyMovement(player.LookDirection);
        }

        public override void ChangeState(PlayerController player)
        {
            base.ChangeState(player);

            if (!InputHub.Instance.Sprint || InputHub.Instance.Move == Vector2.zero || player.Stats.currentSP <= 0f || Vector3.Angle(direction, player.LookDirection) > player.TurnAngle)
            {
                player.SetState(player.GroundState);
            }
        }

        public override void ExitState(PlayerController player)
        {
            player.Stats.canRegenSp = true;
            if (player.Stats.currentSP <= 0f)
            {
                player.Stats.lockRegenSp = true;
            }
        }
    }
}
