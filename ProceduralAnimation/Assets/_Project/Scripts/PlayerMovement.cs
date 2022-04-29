using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float speed;

		[HorizontalLine]
		[SerializeField] private CharacterController characterController;

		private IInputService input;

		private void Awake() =>
			input = new StandaloneInputService();

		private void Update()
		{
			characterController.Move(Physics.gravity);

			if (input.Axis.sqrMagnitude <= .1f)
				return;

			var direction = new Vector3(input.Axis.x, 0, input.Axis.y);
			characterController.Move(direction.normalized * (speed * Time.deltaTime));
		}
	}
}