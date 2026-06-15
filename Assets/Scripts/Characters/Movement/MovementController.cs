using UnityEngine;

namespace ProjectRPG.Characters.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        public CharacterController Controller { get; private set; }
        public float CurrentSpeed {  get; set; }
        public float VerticalSpeed {  get; set; }
        public Vector3 LookDirection {  get; set; }

        protected virtual void Start()
        {
            Controller = GetComponent<CharacterController>();
        }

        public void FaceDirection(Vector3 direction, float turnSpeed = 500f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
        }

        public void ApplyMovement(Vector3 velocity)
        {
            Controller.Move(velocity * Time.deltaTime);
        }
    }
}
