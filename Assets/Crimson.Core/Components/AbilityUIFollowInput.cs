using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	[HideMonoScript]
	public class AbilityUIFollowInput : MonoBehaviour, IActorAbility
	{
		public RectTransform Mark;

		[ValueDropdown(nameof(_modes))]
		public string Mode = _modes[1];

		[ShowIf(nameof(_isLookMode))]
		public float Radius = 5;

		private static readonly string[] _modes = new string[2]
		{
			"Mouse",
			"Look"
		};

		public IActor Actor { get; set; }
		private bool _isLookMode => Mode.Equals(_modes[1]);

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Execute()
		{
		}

		public void Setup(PlayerInputData data)
		{
			if (_modes[0].Equals(Mode))
			{
				var position = data.Mouse;
				position.x -= Screen.width * Mark.pivot.x;
				position.y -= Screen.height * Mark.pivot.y;
				Mark.anchoredPosition = position;
			}

			if (_modes[1].Equals(Mode))
			{
				var position = data.Look;
				position *= Radius;
				Mark.anchoredPosition = position;
			}
		}
	}
}