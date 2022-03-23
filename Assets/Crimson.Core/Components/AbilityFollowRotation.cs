using System.Collections;
using Crimson.Core.Common;
using Crimson.Core.Enums;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Components
{
    [HideMonoScript]
    public class AbilityFollowRotation : MonoBehaviour, IActorAbility
    {
        public IActor Actor { get; set; }
        
        [EnumToggleButtons] public TargetType followTarget;

        [ShowIf("followTarget", TargetType.ComponentName)]
        public string actorWithComponentName;

        [ShowIf("followTarget", TargetType.ChooseByTag)] [ValueDropdown("Tags")]
        public string targetTag;

        [HideIf("followTarget", TargetType.Spawner)] [EnumToggleButtons]
        public ChooseTargetStrategy strategy;

        public bool followX = false;
        public bool followY = true;
        public bool followZ = false;

        public bool retainRotationOffset = true;
        
        public bool hideIfNoTarget = false;
        
        [HideInInspector] public Transform target;
        
        private static IEnumerable Tags()
        {
            return EditorUtils.GetEditorTags();
        }
        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
            
            var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            dstManager.AddComponentData(entity, new ActorFollowRotationData());
            dstManager.AddComponentData(entity, new ActorNoFollowTargetRotationData());

            dstManager.AddComponentData(entity, new RotateDirectlyData
            {
                Constraints = new bool3(followX,followY,followZ)
            });
            
            if (hideIfNoTarget) gameObject.SetActive(false);
        }
        
        public void Execute()
        {
        }
    }

    public struct ActorFollowRotationData : IComponentData
    {
        public float3 Origin;
    }

    public struct ActorNoFollowTargetRotationData : IComponentData
    {
    }
}