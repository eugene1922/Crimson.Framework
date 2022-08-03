using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.FX.ForCamera
{
	public abstract class AbilityCameraFX : MonoBehaviour, IActorAbility, IHasComponentName
	{
		public string _componentName;

		public IActor Actor { get; set; }
		public string ComponentName { get => _componentName; set => _componentName = value; }

		public virtual void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public abstract void Execute();
	}
}