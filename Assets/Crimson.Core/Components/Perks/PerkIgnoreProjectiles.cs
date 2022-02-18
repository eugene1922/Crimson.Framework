using Crimson.Core.Common;
using Crimson.Core.Enums;
using Crimson.Core.Loading;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.Perks
{
	[HideMonoScript]
	public class PerkIgnoreProjectiles : CooldownBehaviour, IActorAbility, IPerkAbility, IPerkAbilityBindable,
		ILevelable, ICooldownable
	{
		public float cooldownTime;
		public List<GameObject> FxPrefabs = new List<GameObject>();
		[ValueDropdown("Tags")] public string ignoredTag;

		[InfoBox("Select materials that will not be replaced")]
		public List<Material> immutableMaterials = new List<Material>();

		[Space]
		[TitleGroup("Levelable properties")]
		[OnValueChanged(nameof(SetLevelableProperty))]
		public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

		public Material materialToApply;
		public bool notRenderImmutableMaterials;
		[ReadOnly] public int perkLevel = 1;
		[LevelableValue] public float perkLifetime;
		public List<MonoBehaviour> perkRelatedComponents = new List<MonoBehaviour>();
		private readonly List<Renderer> _immutableMaterialsRenderers = new List<Renderer>();
		private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();
		private IActor _target;
		public IActor Actor { get; set; }

		public int BindingIndex { get; set; } = -1;

		public float CooldownTime
		{
			get => cooldownTime;
			set => cooldownTime = value;
		}

		public int Level
		{
			get => perkLevel;
			set => perkLevel = value;
		}

		public List<FieldInfo> LevelablePropertiesInfoCached
		{
			get
			{
				if (_levelablePropertiesInfoCached.Count == 0)
				{
					_levelablePropertiesInfoCached = this.GetFieldsWithAttributeInfo<LevelableValue>();
				}

				return _levelablePropertiesInfoCached;
			}
		}

		public List<LevelableProperties> LevelablePropertiesList
		{
			get => levelablePropertiesList;
			set => levelablePropertiesList = value;
		}

		public List<MonoBehaviour> PerkRelatedComponents
		{
			get
			{
				perkRelatedComponents.RemoveAll(c => c is null);
				return perkRelatedComponents;
			}
			set => perkRelatedComponents = value;
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;

			if (!Actor.Abilities.Contains(this))
			{
				Actor.Abilities.Add(this);
			}
		}

		public void Apply(IActor target)
		{
			_target = target;

			ApplyActionWithCooldown(cooldownTime, ApplyIgnoreProjectilesPerk);
		}

		public void ApplyIgnoreProjectilesPerk()
		{
			if (_target != Actor.Owner)
			{
				var ownerActorPlayer =
					Actor.Owner.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

				if (ownerActorPlayer == null)
				{
					return;
				}

				this.SetAbilityLevel(ownerActorPlayer.Level, LevelablePropertiesInfoCached, Actor, _target);
			}

			if (!_target.AppliedPerks.Contains(this))
			{
				_target.AppliedPerks.Add(this);
			}

			var prevTag = _target.GameObject.tag;
			_target.GameObject.tag = ignoredTag;

			var abilityChangeMaterial = Actor.GameObject.SafeAddComponent<AbilityActorChangeMaterial>();
			abilityChangeMaterial.Actor = _target;

			abilityChangeMaterial.materialToApply = materialToApply;
			abilityChangeMaterial.immutableMaterials = immutableMaterials;

			abilityChangeMaterial.Execute();

			if (notRenderImmutableMaterials)
			{
				SetupImmutableMaterialsRenders(false);
			}

			if (FxPrefabs != null && FxPrefabs.Count > 0)
			{
				var spawnData = new ActorSpawnerSettings
				{
					objectsToSpawn = FxPrefabs,
					SpawnPosition = SpawnPosition.UseSpawnerPosition,
					parentOfSpawns = TargetType.None,
					runSpawnActionsOnObjects = true,
					destroyAbilityAfterSpawn = true
				};

				var fx = ActorSpawn.Spawn(spawnData, Actor, null)?.First();
			}

			Timer.TimedActions.AddAction(() =>
			{
				_target.GameObject.tag = prevTag;
				abilityChangeMaterial.TrySetOriginalMaterials();

				if (notRenderImmutableMaterials)
				{
					SetupImmutableMaterialsRenders(true);
				}
			}, perkLifetime);
		}

		public void Execute()
		{
			Apply(Actor);
		}

		public override void FinishTimer()
		{
			base.FinishTimer();
			this.FinishAbilityCooldownTimer(Actor);
		}

		public void Remove()
		{
			if (_target != null && _target.AppliedPerks.Contains(this))
			{
				_target.AppliedPerks.Remove(this);
			}

			foreach (var component in perkRelatedComponents)
			{
				Destroy(component);
			}

			Destroy(this);
		}

		public void SetLevel(int level)
		{
			this.SetAbilityLevel(level, LevelablePropertiesInfoCached, Actor);
		}

		public void SetLevelableProperty()
		{
			this.SetLevelableProperty(LevelablePropertiesInfoCached);
		}

		public override void StartTimer()
		{
			base.StartTimer();
			this.StartAbilityCooldownTimer(Actor);
		}

		private static IEnumerable Tags()
		{
			return EditorUtils.GetEditorTags();
		}

		private void SetupImmutableMaterialsRenders(bool areEnabled)
		{
			if (_target == null)
			{
				return;
			}

			if (!_immutableMaterialsRenderers.Any())
			{
				var renderers = _target.GameObject.GetComponentsInChildren<Renderer>()
					.Where(r => immutableMaterials.Contains(r.sharedMaterial))
					.ToList();

				if (!renderers.Any())
				{
					return;
				}

				_immutableMaterialsRenderers.AddRange(renderers);
			}

			_immutableMaterialsRenderers.ForEach(r => r.enabled = areEnabled);
		}
	}
}