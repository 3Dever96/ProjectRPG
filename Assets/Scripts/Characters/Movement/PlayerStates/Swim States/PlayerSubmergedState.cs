using ProjectRPG.Managers;
using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    [System.Serializable]
    public class PlayerSubmergedState : PlayerState
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

        Vector3 direction;

        public override void StartState(PlayerController player)
        {
            player.VerticalSpeed *= 0.5f;
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

            if (InputHub.Instance.Jump)
            {
                if (player.VerticalSpeed < 0f)
                {
                    player.VerticalSpeed += decel * Time.deltaTime;
                }
                else if (player.VerticalSpeed < maxSpeed)
                {
                    player.VerticalSpeed += accel * Time.deltaTime;
                }
                else
                {
                    player.VerticalSpeed = maxSpeed;
                }
            }
            else if (InputHub.Instance.Crouch)
            {
                if (player.VerticalSpeed > 0f)
                {
                    player.VerticalSpeed -= decel * Time.deltaTime;
                }
                else if (player.VerticalSpeed > -maxSpeed)
                {
                    player.VerticalSpeed -= accel * Time.deltaTime;
                }
                else
                {
                    player.VerticalSpeed = -maxSpeed;
                }
            }
            else
            {
                player.VerticalSpeed -= Mathf.Min(fric * Time.deltaTime, Mathf.Abs(player.VerticalSpeed)) * Mathf.Sign(player.VerticalSpeed);
            }

            player.ApplyMovement(player.transform.forward);
        }

        public override void ChangeState(PlayerController player)
        {
            if (player.TouchingWater && player.transform.position.y + (player.Controller.height / 2f) > player.WaterLevel)
            {
                player.SetState(player.SurfaceState);
            }
        }

        public override void ExitState(PlayerController player)
        {

        }
    }
}
