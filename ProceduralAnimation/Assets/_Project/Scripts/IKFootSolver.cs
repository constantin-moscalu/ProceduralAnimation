using Unity.Mathematics;
using UnityEngine;

namespace _Project.Scripts
{
	public class IKFootSolver : MonoBehaviour
	{
		[SerializeField] private LayerMask layerMask;
		[SerializeField] private Transform body;
		[SerializeField] private IKFootSolver otherFoot;
		[SerializeField] private float speed = 1;
		[SerializeField] private float stepDistance = 4;
		[SerializeField] private float stepLength = 4;
		[SerializeField] private float stepHeight = 1;
		[SerializeField] private Vector3 footOffset;

		public bool IsMoving => lerp < 1;

		private float footSpacing;
		private Vector3 oldPosition, currentPosition, newPosition;
		private Vector3 oldNormal, currentNormal, newNormal;
		private float lerp;

		private RaycastHit lastHit;

		private void Start()
		{
			footSpacing = transform.localPosition.x;
			currentPosition = newPosition = oldPosition = transform.position;
			currentNormal = newNormal = oldNormal = transform.up;
			lerp = 1;
		}

		private void Update()
		{
			transform.position = currentPosition;
			transform.up = currentNormal;

			Ray ray = new Ray(body.position + body.right * footSpacing + body.forward * stepLength, Vector3.down);

			if (Physics.Raycast(ray, out RaycastHit hit, 10, layerMask))
			{
				lastHit = hit;
				
				if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.IsMoving && lerp >= 1)
				{
					lerp = 0;
					int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
					newPosition = hit.point + body.forward * (stepLength * direction) + footOffset;
					newNormal = hit.normal;
				}
			}

			if (lerp < 1)
			{
				Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
				tempPosition.y += Mathf.Sin(lerp * math.PI) * stepHeight;

				currentPosition = tempPosition;
				currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
				lerp += Time.deltaTime * speed;
			}
			else
			{
				oldPosition = newPosition;
				oldNormal = newNormal;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(newPosition, .1f);

			Gizmos.color = Color.green;
			Gizmos.DrawSphere(lastHit.point, .1f);
		}
	}
}