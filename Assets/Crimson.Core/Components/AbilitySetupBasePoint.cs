using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilitySetupBasePoint : MonoBehaviour, IActorAbility
	{
		public bool _executeOnAwake;

		private Entity _entity;
		private EntityManager _entityManager;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entity = entity;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			if (_executeOnAwake)
			{
				Execute();
			}
		}

		public void Execute()
		{
			var hasData = _entityManager.HasComponent<BasePointData>(_entity);
			if (hasData)
			{
				UpdateData();
			}
			else
			{
				AddData();
			}
		}

		private void AddData()
		{
			var data = new BasePointData()
			{
				Position = transform.position
			};
			_entityManager.AddComponentData(_entity, data);
		}

		private void UpdateData()
		{
			var data = _entityManager.GetComponentData<BasePointData>(_entity);
			data.Position = transform.position;
			_entityManager.SetComponentData(_entity, data);
		}
	}
}