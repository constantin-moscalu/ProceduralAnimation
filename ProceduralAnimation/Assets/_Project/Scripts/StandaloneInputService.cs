using UnityEngine;

namespace _Project.Scripts
{
	public class StandaloneInputService : IInputService
	{
		public Vector2 Axis => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}
}