using Assets.Crimson.Core.Components.Tags.Effects;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Unity.Entities;

namespace Assets.Crimson.Core.Components.FX.ForCamera
{
	public class ShowFadeInFx : TimerBaseBehaviour, IActorAbility
	{
		public string EffectName;
		public float StartupDelay;
		public float Duration = 2;

		private Entity _entity;
		private EntityManager _entityManager;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			Actor = actor;
			StartTimer();
		}

		public void Execute()
		{
			if (StartupDelay == 0)
			{
				SetFXTag();
			}
			else
			{
				Timer.TimedActions.AddAction(SetFXTag, StartupDelay);
			}
		}

		private void SetFXTag()
		{
			_entityManager.AddComponentData(_entity, new FadeInFXTag());
		}
	}
}