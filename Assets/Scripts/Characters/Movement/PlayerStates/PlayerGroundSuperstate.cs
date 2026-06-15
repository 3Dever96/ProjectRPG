using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    public class PlayerGroundSuperstate : PlayerState
    {
        public override void StartState(PlayerController player)
        {

        }

        public override void UpdateState(PlayerController player)
        {
            
        }

        public override void ChangeState(PlayerController player)
        {
            if (player.VerticalSpeed > 0f || !Physics.CheckSphere(player.transform.position, player.Controller.radius - 0.01f, LayerMask.GetMask("Solid")))
            {
                player.SetState(player.AirState);
            }
        }

        public override void ExitState(PlayerController player)
        {
            
        }
    }
}
