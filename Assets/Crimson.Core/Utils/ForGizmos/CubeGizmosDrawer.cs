using UnityEngine;

namespace Assets.Crimson.Core.Utils.ForGizmos
{
	public class CubeGizmosDrawer : GizmosDrawer
	{
		private void OnDrawGizmos()
		{
			Gizmos.color = color;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
		}
	}
}