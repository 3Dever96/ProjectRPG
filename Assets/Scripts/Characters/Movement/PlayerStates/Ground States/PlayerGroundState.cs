using ProjectRPG.Managers;
using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    public class PlayerGroundState : PlayerGroundSuperstate
    {
        private float moveSpeed;

        // True speed variables
        private float maxSpeed;
        private float accel;
        private float decel;
        private float fric;

        private float jumpSpeed;

        private bool canJump;

        Vector3 direction;

        public override void StartState(PlayerController player)
        {
            player.VerticalSpeed = player.StickForce;

            canJump = false;

            player.LookDirection = player.transform.forward;
        }

        public override void UpdateState(PlayerController player)
        {
            // Set true speed variables
            if (player.Stats.stats.ContainsKey("AGI"))
            {
                maxSpeed = player.BaseRunSpeed + 3f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
                accel = player.BaseAccel + 12f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
                decel = player.BaseDecel + 64f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
                fric = player.BaseFric + 6f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));

                jumpSpeed = player.BaseJumpSpeed + 5f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
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
                if (Vector3.Angle(direction, player.LookDirection) > player.TurnAngle)
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

            if (player.Stats.skills.Contains("Sprint"))
            {
                if (InputHub.Instance.Sprint && InputHub.Instance.Move != Vector2.zero && player.Stats.currentSP > 0f && !player.Stats.lockRegenSp && Vector3.Angle(direction,player.LookDirection) < player.TurnAngle)
                {
                    player.SetState(player.SprintState);
                }
            }

            if (InputHub.Instance.Crouch)
            {
                player.SetState(player.CrouchState);
            }
        }

        public override void ExitState(PlayerController player)
        {
            
        }
    }
}
