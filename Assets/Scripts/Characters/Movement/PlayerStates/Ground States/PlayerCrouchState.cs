using ProjectRPG.Managers;
using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    [System.Serializable]
    public class PlayerCrouchState : PlayerGroundSuperstate
    {
        [SerializeField] private GameObject avatar;

        private float moveSpeed;

        // True speed variables
        private float maxSpeed;

        Vector3 direction;

        public override void StartState(PlayerController player)
        {
            player.VerticalSpeed = player.StickForce;

            player.LookDirection = player.transform.forward;

            avatar.transform.localScale = new Vector3(1f, 0.5f, 1f);
        }

        public override void UpdateState(PlayerController player)
        {
            // Set true speed variables
            if (player.Stats.stats.ContainsKey("AGI"))
            {
                maxSpeed = (player.BaseRunSpeed * 0.5f) + 1.5f * (player.Stats.stats["AGI"] / (player.Stats.stats["AGI"] + 150f));
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
                player.CurrentSpeed = moveSpeed;
                player.LookDirection = direction;
            }
            else
            {
                player.CurrentSpeed = 0f;
            }

            player.FaceDirection(player.LookDirection);

            player.ApplyMovement(player.LookDirection);
        }

        public override void ChangeState(PlayerController player)
        {
            base.ChangeState(player);

            if (!InputHub.Instance.Crouch)
            {
                player.SetState(player.GroundState);
            }
        }

        public override void ExitState(PlayerController player)
        {
            avatar.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
