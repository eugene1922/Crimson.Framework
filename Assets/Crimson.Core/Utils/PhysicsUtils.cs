using Crimson.Core.Common;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Utils
{
	public static class PhysicsUtils
	{
		public static bool Contains(this LayerMask mask, int layer)
		{
			return LayerMask.LayerToName(layer).Equals(string.Empty, StringComparison.Ordinal) || mask == (mask | (1 << layer));
		}

		public static void DeathPhysics(this GameObject go, Entity entity, DeadBehaviour deadBehaviour)
		{
			if (deadBehaviour.RemoveTag)
			{
				go.tag = string.Empty;
			}

			if (deadBehaviour.RemoveRigidbody)
			{
				go.GetComponents<Rigidbody>().ForEach(a => UnityEngine.Object.Destroy(a));
			}

			if (deadBehaviour.RemoveColliders)
			{
				go.GetComponentsInChildren<Collider>().ForEach(a => UnityEngine.Object.Destroy(a));
			}
		}

		public static List<Collider> GetAllColliders(this GameObject go)
		{
			List<Collider> result = null;
			if (go != null)
			{
				result = go.GetComponents<Collider>().ToList();
			}
			return result;
		}

		public static void ToWorldSpaceBox(this BoxCollider box, out float3 center, out float3 halfExtents,
			out quaternion orientation)
		{
			var transform = box.transform;
			orientation = transform.rotation;
			center = transform.TransformPoint(box.center);
			var lossyScale = transform.lossyScale;
			var scale = Abs(lossyScale);
			halfExtents = Vector3.Scale(scale, box.size) * 0.5f;
		}

		public static void ToWorldSpaceCapsule(this CapsuleCollider capsule, out float3 point0, out float3 point1,
			out float radius)
		{
			var transform = capsule.transform;
			var center = (float3)transform.TransformPoint(capsule.center);
			radius = 0f;
			var height = 0f;
			var lossyScale = Abs(transform.lossyScale);
			var dir = float3.zero;

			switch (capsule.direction)
			{
				case 0: // x
					radius = Mathf.Max(lossyScale.y, lossyScale.z) * capsule.radius;
					height = lossyScale.x * capsule.height;
					dir = capsule.transform.TransformDirection(Vector3.right);
					break;

				case 1: // y
					radius = Mathf.Max(lossyScale.x, lossyScale.z) * capsule.radius;
					height = lossyScale.y * capsule.height;
					dir = capsule.transform.TransformDirection(Vector3.up);
					break;

				case 2: // z
					radius = Mathf.Max(lossyScale.x, lossyScale.y) * capsule.radius;
					height = lossyScale.z * capsule.height;
					dir = capsule.transform.TransformDirection(Vector3.forward);
					break;
			}

			if (height < radius * 2f)
			{
				dir = Vector3.zero;
			}

			point0 = center + (dir * ((height * 0.5f) - radius));
			point1 = center - (dir * ((height * 0.5f) - radius));
		}

		public static void ToWorldSpaceSphere(this SphereCollider sphere, out float3 center, out float radius)
		{
			var transform = sphere.transform;
			center = transform.TransformPoint(sphere.center);
			radius = sphere.radius * Max(Abs(transform.lossyScale));
		}

		private static float3 Abs(float3 v)
		{
			return new float3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		private static float Max(float3 v)
		{
			return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
		}
	}
}