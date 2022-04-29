using System;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.ProceduralWalking
{
	public class FeetSynchronizer : MonoBehaviour
	{
		[SerializeField] private float footOffset;
		[SerializeField] private float stepLength;
		[SerializeField] private float stepProgressSpeed;

		[HorizontalLine]
		[SerializeField] private ProceduralFoot leftFoot;
		[SerializeField] private ProceduralFoot rightFoot;
		[SerializeField] private CharacterController characterController;

		public event Action<float> onStepProgress;

		private float stepProgress;

		private void Start()
		{
			leftFoot.Initialize(this, false);
			rightFoot.Initialize(this, true);
		}

		private void Update()
		{
			if (characterController.velocity.sqrMagnitude < .1f)
				return;

			if (stepProgress < 1f)
			{
				stepProgress += stepProgressSpeed * Time.deltaTime;
				onStepProgress?.Invoke(Mathf.Clamp01(stepProgress));
				return;
			}

			stepProgress = 0;
			SwitchFeet();
		}

		private void SwitchFeet()
		{
			Vector3 forwardWithOffset = transform.position + transform.forward * stepLength;

			leftFoot.Switch(forwardWithOffset + transform.right * -footOffset);
			rightFoot.Switch(forwardWithOffset + transform.right * footOffset);
		}
	}
}