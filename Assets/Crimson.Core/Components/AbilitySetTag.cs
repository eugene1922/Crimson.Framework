using System;
using System.Collections;
using System.Collections.Generic;
using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
    [HideMonoScript]
    public class AbilitySetTag : MonoBehaviour, IActorAbility
    {
        public IActor Actor { get; set; }

        [ValueDropdown("Tags")] public string newTag;

        public bool alsoSetLayer;
        
        [ShowIf("alsoSetLayer")][ValueDropdown("Layers")] public string newLayer;

        public bool applyOnStart = true;
        
        private static IEnumerable Tags()
        {
            return EditorUtils.GetEditorTags();
        }
        
        private static IEnumerable Layers()
        {
            var layerNames = new List<string>();
            for(var i = 0; i <= 31; i++) 
            {
                var layerN=LayerMask.LayerToName(i);
                if (layerN.Length > 0)
                    layerNames.Add(layerN);
            }

            return layerNames;
        }
        private void Start()
        {
            if (applyOnStart) Execute();
        }
        
        public void AddComponentData(ref Entity entity, IActor actor)
        {
        }
        
        public void Execute()
        {
            if (newTag != string.Empty)
            {
                gameObject.tag = newTag;
            }

            if (newLayer != string.Empty)
            {
                gameObject.layer = LayerMask.NameToLayer(newLayer);
            }
        }
    }
}