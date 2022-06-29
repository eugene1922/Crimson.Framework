using UnityEngine;

namespace Assets.Crimson.Core.Utils.ForGizmos
{
	public class SphereGizmosDrawer : GizmosDrawer
	{
		[SerializeField] private float radius = 1;

		private void OnDrawGizmos()
		{
			Gizmos.color = color;
			Gizmos.DrawSphere(transform.position, radius);
		}
	}
}