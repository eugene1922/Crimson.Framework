using Assets.Crimson.Core.Components.Interfaces;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityBlocker : MonoBehaviour, IActorAbility, IEnableable
	{
		public List<Type> BlockedTypes;

		public bool EnableOnAwake;

		[ValueDropdown(nameof(GetAbilityList), IsUniqueList = true)]
		public List<string> References;

		private bool _isEnable;

		public IActor Actor { get; set; }

		public bool IsEnable
		{
			get => _isEnable;
			set
			{
				var isNew = _isEnable != value;
				_isEnable = value;
				if (isNew)
				{
					RefreshRefs();
				}
			}
		}

		private Type[] Types => Assembly.GetExecutingAssembly().GetTypes();

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			BlockedTypes = new List<Type>();
			for (var i = 0; i < References.Count; i++)
			{
				var refTypeName = References[i];
				if (!string.IsNullOrEmpty(refTypeName))
				{
					BlockedTypes.Add(Types.First(s => s.Name.Equals(refTypeName)));
				}
			}
			IsEnable = EnableOnAwake;
		}

		public void Execute()
		{
		}

		private List<string> GetAbilityList()
		{
			return Assembly.GetExecutingAssembly().GetTypes()
				.Where(s => s.GetInterfaces().Contains(typeof(IBlockable)))
				.Select(s => s.Name)
				.ToList();
		}

		private void RefreshRefs()
		{
			var abilities = Actor.Abilities
				.Where(s => BlockedTypes.Contains(s.GetType()))
				.Where(s => s.GetType().GetInterfaces().Contains(typeof(IBlockable)));
			foreach (IBlockable ability in abilities)
			{
				ability.IsBlocked = IsEnable;
			}
		}
	}
}