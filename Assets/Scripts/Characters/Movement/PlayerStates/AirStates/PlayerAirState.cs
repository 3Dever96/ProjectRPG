using ProjectRPG.Managers;
using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    [System.Serializable]
    public class PlayerAirState : PlayerAirSuperstate
    {
        [SerializeField] private float gravity;
        [SerializeField] private float fallSpeed;

        public override void StartState(PlayerController player)
        {
            
        }

        public override void UpdateState(PlayerController player)
        {
            if (!InputHub.Instance.Jump || Physics.CheckSphere(player.transform.position + Vector3.up * player.Controller.height, player.Controller.radius - 0.01f, LayerMask.GetMask("Solid")))
            {
                player.VerticalSpeed = Mathf.Min(0f, player.VerticalSpeed);
            }

            if (player.VerticalSpeed > fallSpeed)
            {
                player.VerticalSpeed += gravity * Time.deltaTime;
            }

            player.FaceDirection(player.LookDirection);

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
