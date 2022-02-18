using UnityEngine;

namespace Crimson.Core.Utils.LowLevel
{
	public static class FixReleaseBuild
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
#if UNITY_DISABLE_AUTOMATIC_SYSTEM_BOOTSTRAP
            DefaultWorldInitialization.Initialize("Default World", false);
#endif
		}
	}
}