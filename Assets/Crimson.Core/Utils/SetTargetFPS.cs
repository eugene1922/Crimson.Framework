using UnityEngine;

namespace Crimson.Core.Utils
{
	public class SetTargetFPS : MonoBehaviour
	{
		public TargetFPS limit = TargetFPS.LimitTo60;

		// Start is called before the first frame update
		private void Start()
		{
			var fps = -1;

			switch (limit)
			{
				case TargetFPS.Unlimited:
					fps = int.MaxValue;
					break;

				case TargetFPS.LimitTo30:
					fps = 30;
					break;

				case TargetFPS.LimitTo60:
					fps = 60;
					break;

				case TargetFPS.LimitTo120:
					fps = 120;
					break;

				case TargetFPS.PlatformDefault:
					fps = -1;
					break;

				default:
					break;
			}

			Application.targetFrameRate = fps;
		}
	}

	public enum TargetFPS
	{
		PlatformDefault = 0,
		LimitTo30 = 1,
		LimitTo60 = 2,
		LimitTo120 = 3,
		Unlimited = 4
	}
}