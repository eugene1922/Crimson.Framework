using Crimson.Core.Common;
using Crimson.Core.Components;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.FX.ForCamera
{
	public class AbilityCameraFadeFX : MonoBehaviour, IActorAbility, IHasComponentName
	{
		public string _componentName;

		public AnimationCurve Curve;
		public CanvasGroup Target;
		public float Duration { get; set; }
		public IActor Actor { get; set; }
		public string ComponentName { get => _componentName; set => _componentName = value; }

		public virtual void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		[Button]
		public void Execute()
		{
			var sequence = DOTween.Sequence();
			sequence.Append(DOVirtual.Float(0, 1, Duration, ChangeAlpha));
			sequence.Play();
		}

		private void ChangeAlpha(float value)
		{
			Target.alpha = Curve.Evaluate(value);
		}
	}
}