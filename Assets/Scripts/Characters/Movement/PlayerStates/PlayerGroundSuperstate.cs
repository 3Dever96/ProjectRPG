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

            if (player.Stats.skills.Contains("Swim"))
            {
                if (player.transform.position.y + (player.Controller.height / 2f) < player.WaterLevel && player.TouchingWater)
                {
                    player.SetState(player.SurfaceState);
                }
            }
        }

        public override void ExitState(PlayerController player)
        {
            
        }
    }
}
