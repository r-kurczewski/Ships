using UnityEngine;

namespace Ships.Extensions
{
	public static class VectorExtensions
	{
		public static float AbsSum(this Vector3 v)
		{
			return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);
		}
	}
}

