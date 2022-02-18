using Crimson.Core.Common;

namespace Crimson.Core.Utils
{
	public static class CurveUtil
	{
		public static float Evaluate(this Curve curve, float t)
		{
			t = MathUtils.Clamp(t, 0f, 1f);

			var distanceTime = curve.keyframe1.time - curve.keyframe0.time;

			var m0 = curve.keyframe0.outTangent * distanceTime;
			var m1 = curve.keyframe1.inTangent * distanceTime;

			var t2 = t * t;
			var t3 = t2 * t;

			var a = (2 * t3) - (3 * t2) + 1;
			var b = t3 - (2 * t2) + t;
			var c = t3 - t2;
			var d = (-2 * t3) + (3 * t2);

			return (a * curve.keyframe0.value) + (b * m0) + (c * m1) + (d * curve.keyframe1.value);
		}
	}
}