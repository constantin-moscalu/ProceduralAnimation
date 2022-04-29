using UnityEngine;

namespace _Project.Scripts.ProceduralWalking
{
	public class ProceduralFoot : MonoBehaviour
	{
		[SerializeField] private LayerMask layerMask;
		[SerializeField] private Vector3 positionOffset;

		private bool isActive;
		private Vector3 nextPosition;
		private Vector3 lastPosition;
		private FeetSynchronizer feetSynchronizer;

		public bool IsActive
		{
			get => isActive;
			set => isActive = value;
		}

		private void Awake()
		{
			lastPosition = nextPosition = transform.position - positionOffset;
			
			Ray ray = new Ray(transform.position, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit, 10, layerMask)) 
				lastPosition = nextPosition = hit.point;
		}

		private void OnDestroy() => 
			feetSynchronizer.onStepProgress -= UpdateStepPosition;

		public void Initialize(FeetSynchronizer feetSynchronizer, bool newState)
		{
			this.feetSynchronizer = feetSynchronizer;
			isActive = newState;
			feetSynchronizer.onStepProgress += UpdateStepPosition;
		}

		private void UpdateStepPosition(float stepProgress)
		{
			if(!isActive)
				return;

			Vector3 lerpPosition = Vector3.Lerp(lastPosition, nextPosition, stepProgress);
			transform.position = lerpPosition + positionOffset;
		}

		public void Switch(Vector3 nextStep)
		{
			isActive = !isActive;

			if (!isActive)
				return;

			Ray ray = new Ray(nextStep, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit, 10, layerMask))
			{
				lastPosition = nextPosition;
				nextPosition = hit.point;
			}
		}
	}
}