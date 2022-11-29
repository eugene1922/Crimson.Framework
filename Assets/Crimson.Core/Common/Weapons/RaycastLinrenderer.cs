using UnityEngine;

namespace Assets.Crimson.Core.Common.Weapons
{
	public class RaycastLinrenderer : MonoBehaviour
	{
		[SerializeField] private Vector3 _direction;
		[SerializeField] private LineRenderer _renderer;

		private void Update()
		{
			var target = _direction;
			var direction = transform.TransformDirection(_direction);
			if (Physics.Raycast(transform.position, direction, out var hit))
			{
				target = transform.InverseTransformPoint(hit.point);
			}
			_renderer.SetPosition(_renderer.positionCount - 1, target);
		}
	}
}