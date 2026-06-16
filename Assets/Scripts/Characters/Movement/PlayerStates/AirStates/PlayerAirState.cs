using ProjectRPG.Managers;
using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    public class PlayerAirState : PlayerAirSuperstate
    {
        public override void StartState(PlayerController player)
        {
            
        }

        public override void UpdateState(PlayerController player)
        {
            if (!InputHub.Instance.Jump || Physics.CheckSphere(player.transform.position + Vector3.up * player.Controller.height, player.Controller.radius - 0.01f, LayerMask.GetMask("Solid")))
            {
                player.VerticalSpeed = Mathf.Min(0f, player.VerticalSpeed);
            }

            if (player.VerticalSpeed > player.FallSpeed)
            {
                player.VerticalSpeed += player.Gravity * Time.deltaTime;
            }

            player.FaceDirection(player.LookDirection);

            player.ApplyMovement(player.LookDirection);
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
