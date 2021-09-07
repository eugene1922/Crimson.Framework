using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
    [HideMonoScript]
    public class AbilityChainActions : TimerBaseBehaviour, IActorAbility
    {
        public bool ExecuteOnAwake = false;
        public AbilityChainSettings Settings = new AbilityChainSettings();
        public IActor Actor { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
        }

        [ContextMenu("Test")]
        public void Execute()
        {
            Timer.TimedActions.AddAction(TestMethod, 2);
        }

        private void Start()
        {
            if (ExecuteOnAwake)
            {
                Execute();
            }
        }

        private void TestMethod()
        {
            Debug.Log("TestMetod");
        }
    }
}