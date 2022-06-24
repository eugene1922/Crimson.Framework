using UnityEngine;
using UnityEngine.AI;

namespace Assets.Crimson.Core.Components
{
	public class PlayerInput : MonoBehaviour
	{
		[SerializeField]
		private LayerMask FloorLayers;

		[SerializeField]
		private NavMeshAgent Agent;

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Mouse1))
			{
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit Hit, FloorLayers))
				{
					Agent.SetDestination(Hit.point);
				}
			}
		}
	}
}