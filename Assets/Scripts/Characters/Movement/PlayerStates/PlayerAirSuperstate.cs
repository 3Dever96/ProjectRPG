using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    public class PlayerAirSuperstate : PlayerState
    {
        public override void StartState(PlayerController player)
        {
            
        }

        public override void UpdateState(PlayerController player)
        {
            
        }

        public override void ChangeState(PlayerController player)
        {
            if (player.VerticalSpeed < 0f && Physics.CheckSphere(player.transform.position, player.Controller.radius - 0.01f, LayerMask.GetMask("Solid")))
            {
                player.SetState(player.GroundState);
            }

            if (player.Stats.skills.Contains("Swim"))
            {
                if (player.TouchingWater && player.transform.position.y + (player.Controller.height / 2f) < player.WaterLevel)
                {
                    player.SetState(player.SubmergedState);
                }
            }
        }

        public override void ExitState(PlayerController player)
        {
            
        }
    }
}
