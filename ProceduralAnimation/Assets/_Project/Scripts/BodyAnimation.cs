using UnityEngine;

namespace _Project.Scripts
{
	public class BodyAnimation : MonoBehaviour
	{
		[SerializeField] private CharacterController characterController;

		private void Update()
		{
			Vector3 velocity = characterController.velocity;

			if (velocity.sqrMagnitude < .1f)
				return;

			float yTargetAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
			print(velocity + " => " + yTargetAngle);

			transform.rotation = Quaternion.Euler(0, yTargetAngle, 0);
		}
	}
}