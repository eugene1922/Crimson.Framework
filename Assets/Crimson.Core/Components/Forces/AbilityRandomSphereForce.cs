using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Forces
{
	[HideMonoScript]
	public class AbilityRandomSphereForce : MonoBehaviour, IActorAbility
	{
		[SerializeField] private bool _executeOnStart;

		public float ForceValue = 1;
		public ForceMode Mode = ForceMode.Force;
		private EntityManager _dstManager;
		private Entity _entity;
		public IActor Actor { get; set; }
		public Transform Spawner => Actor.Spawner.GameObject.transform;

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entity = entity;
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			if (_executeOnStart)
			{
				Execute();
			}
		}

		public void Execute()
		{
			var direction = UnityEngine.Random.insideUnitSphere;
			var forceData = new ForceData()
			{
				Direction = direction,
				Force = ForceValue,
				ForceMode = (int)Mode
			};
			_dstManager.AddComponentData(_entity, forceData);
		}
	}
}