using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crimson.Core.Components
{
    public interface ILevelable
    {
        int Level { get; set; }
        void SetLevel(int level);
        void SetLevelableProperty();
        List<LevelableProperties> LevelablePropertiesList { get; set; }
    }
    
    [Serializable]
    public struct LevelableProperties
    {
        [ValueDropdown("levelablePropertiesInfo")]
        public string propertyName;

        public ModifiablePropertiesActions levelablePropertyAction;

        public float modifier;
        
        [HideInInspector] public List<string> levelablePropertiesInfo;
    }

    public enum ModifiablePropertiesActions
    {
        Multiply = 0,
        Add = 1
    }
}