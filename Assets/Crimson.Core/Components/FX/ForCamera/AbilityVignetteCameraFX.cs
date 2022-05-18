using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Crimson.Core.Components.FX.ForCamera
{
	public class AbilityVignetteCameraFX : AbilityCameraFX
	{
		public Camera Target;
		public float Duration = .33f;
		public Color DamageColor = Color.black;

		[Button]
		public override void Execute()
		{
			var sequence = DOTween.Sequence();
			var lastColor = Target.backgroundColor;
			sequence.Append(Target.DOColor(DamageColor, Duration / 2));
			sequence.Append(Target.DOColor(lastColor, Duration / 2));
			sequence.Play();
		}
	}
}