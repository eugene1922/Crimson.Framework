using Crimson.Core.Components;
using Crimson.Core.Enums;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crimson.Core.Common
{
	[Serializable]
	public struct CollisionAction
	{
		[ValidateInput(nameof(MustBeAbility), "Ability MonoBehaviours must derive from IActorAbility!")]
		public List<MonoBehaviour> actions;

		[InfoBox("Objects without layer set will always collide")]
		public LayerMask collisionLayerMask;

		public bool destroyAfterAction;

		public bool executeOnCollisionWithSpawner;

		[ShowIf(nameof(useTagFilter))]
		[EnumToggleButtons]
		public TagFilterMode filterMode;

		[ShowIf(nameof(useTagFilter))]
		[ValueDropdown(nameof(Tags))]
		public List<string> filterTags;

		[InfoBox("Filter by tags works in addition to Collision Layer Mask")]
		public bool useTagFilter;

		private static IEnumerable Tags()
		{
			return EditorUtils.GetEditorTags();
		}

		private bool MustBeAbility(List<MonoBehaviour> a)
		{
			return !a.Exists(t => !(t is IActorAbility)) || a.Count == 0;
		}
	}

	[Serializable]
	public struct CollisionSettings
	{
		[InfoBox("Objects without layer set will always collide")]
		public LayerMask collisionLayerMask;

		public bool destroyAfterAction;

		public bool executeOnCollisionWithSpawner;

		[ShowIf(nameof(useTagFilter))]
		[EnumToggleButtons]
		public TagFilterMode filterMode;

		[ShowIf(nameof(useTagFilter))]
		[ValueDropdown(nameof(Tags))]
		public List<string> filterTags;

		[InfoBox("Filter by tags works in addition to Collision Layer Mask")]
		public bool useTagFilter;

		private static IEnumerable Tags()
		{
			return EditorUtils.GetEditorTags();
		}
	}
}