using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float speed;

		[HorizontalLine]
		[SerializeField] private CharacterController characterController;

		private const float TurnSmoothDuration = .1f;
		
		private IInputService input;
		private float turnSmoothVelocity;

		private void Awake() =>
			input = new StandaloneInputService();

		private void Update()
		{
			characterController.Move(Physics.gravity);

			if (input.Axis.sqrMagnitude <= .1f)
				return;

			Vector3 direction = new Vector3(input.Axis.x, 0, input.Axis.y).normalized;

			transform.rotation = Quaternion.Euler(0f, GetRotation(direction), 0f);
			characterController.Move(direction * (speed * Time.deltaTime));
		}

		private float GetRotation(Vector3 direction)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothDuration);
			return angle;
		}
	}
}