using UnityEngine;

namespace _Project.Scripts.ProceduralWalking
{
	public class ProceduralFoot : MonoBehaviour
	{
		[SerializeField] private LayerMask layerMask;
		[SerializeField] private Vector3 positionOffset;

		private Vector3 nextPosition, lastPosition;
		private Vector3 nextNormal, lastNormal;
		private FeetSynchronizer feetSynchronizer;
		private AnimationCurve stepCurve;
		private float stepHeight;

		public bool IsActive { get; private set; }

		private void Awake()
		{
			lastPosition = nextPosition = transform.position - positionOffset;

			Ray ray = new Ray(transform.position, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit, 10, layerMask))
			{
				lastPosition = nextPosition = hit.point;
				lastNormal = nextNormal = hit.normal;
			}
		}

		private void OnDestroy() =>
			feetSynchronizer.onStepProgress -= UpdateStepPosition;

		private void Update()
		{
			if (IsActive)
				return;

			transform.position = nextPosition + positionOffset;
		}

		public void Initialize(FeetSynchronizer feetSynchronizer, AnimationCurve stepCurve, float stepHeight, bool newState)
		{
			this.feetSynchronizer = feetSynchronizer;
			this.stepCurve = stepCurve;
			this.stepHeight = stepHeight;
			IsActive = newState;
			
			feetSynchronizer.onStepProgress += UpdateStepPosition;
		}

		private void UpdateStepPosition(float stepProgress)
		{
			if (!IsActive)
				return;

			Vector3 lerpPosition = Vector3.Lerp(lastPosition, nextPosition, stepProgress);
			Vector3 verticalOffset = new Vector3(0, stepCurve.Evaluate(stepProgress) * stepHeight, 0);
			transform.position = lerpPosition + verticalOffset + positionOffset;

			Vector3 currentNormal = Vector3.Lerp(lastNormal, nextNormal, stepProgress);
			transform.forward -= Vector3.Dot(transform.forward, currentNormal) * currentNormal;
		}

		public void Switch(Vector3 nextStep)
		{
			IsActive = !IsActive;

			if (!IsActive)
				return;

			Ray ray = new Ray(nextStep, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit, 10, layerMask))
			{
				lastPosition = nextPosition;
				nextPosition = hit.point;

				lastNormal = nextNormal;
				nextNormal = hit.normal;
			}
		}

		private void OnDrawGizmos()
		{
			if (!IsActive)
				return;

			DrawGizmo(lastPosition, .1f, Color.yellow);
			DrawGizmo(nextPosition, .1f, Color.green);
		}

		private void DrawGizmo(Vector3 position, float radius, Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawSphere(position, radius);
		}
	}
}