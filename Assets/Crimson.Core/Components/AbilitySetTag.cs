using System.Collections;
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

        public bool applyOnStart = true;
        
        private static IEnumerable Tags()
        {
            return EditorUtils.GetEditorTags();
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
        }
    }
}