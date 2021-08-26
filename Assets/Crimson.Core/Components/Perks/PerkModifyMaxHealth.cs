using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.Perks
{
    [HideMonoScript]
    public class PerkModifyMaxHealth : MonoBehaviour, IActorAbility, IPerkAbility, ILevelable
    {
        [ReadOnly] public int perkLevel = 1;
        [LevelableValue] public float healthModifier = 15;
        public IActor Actor { get; set; }
        public int Level
        {
            get => perkLevel;
            set => perkLevel = value;
        }
        
        [Space] [TitleGroup("Levelable properties")] [OnValueChanged("SetLevelableProperty")]
        public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();
        
        public List<FieldInfo> LevelablePropertiesInfoCached
        {
            get
            {
                if (_levelablePropertiesInfoCached.Any()) return _levelablePropertiesInfoCached;
                return _levelablePropertiesInfoCached = this.GetFieldsWithAttributeInfo<LevelableValue>();
            }
        }
        
        public List<LevelableProperties> LevelablePropertiesList
        {
            get => levelablePropertiesList;
            set => levelablePropertiesList = value;
        }
        
        private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
        }

        public void Execute()
        {
            var abilityActorPlayer = Actor.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;
            
            if (abilityActorPlayer == null) return;
            abilityActorPlayer.UpdateMaxHealthData((int) healthModifier);
        }

        public void Apply(IActor target)
        {
            var copy = target.GameObject.CopyComponent(this) as PerkModifyMaxHealth;
            
            if (copy == null)
            {
                Debug.LogError("[PERK MODIFY MAX HEALTH] Error copying perk to Actor!");
                return;
            }
            
            if (Actor.Spawner.AppliedPerks.Contains(copy)) Actor.Spawner.AppliedPerks.Add(copy);
            
            copy.Execute();
        }

        public void Remove()
        {
            var player = GetComponent<AbilityActorPlayer>();
            player.UpdateMaxHealthData((int) -healthModifier);
            
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
    }
}