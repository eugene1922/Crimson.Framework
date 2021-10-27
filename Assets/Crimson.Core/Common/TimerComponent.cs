using System;
using System.Collections.Generic;
using System.Linq;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Common
{
    [NetworkSimObject]
    public class TimerComponent : MonoBehaviour
    {
        [NetworkSimData]
        public List<TimerAction> TimedActions = new List<TimerAction>();
        
        [NetworkSimData]
        public IActor actor;

        
        
        public void Start()
        {
            actor = this.gameObject.GetComponent<IActor>();
            if (actor == null)
            {
                Debug.LogError("[TIMER COMPONENT] No IActor component found, aborting!");
                return;
            }
            
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<TimerData>(actor.ActorEntity);
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentObject(actor.ActorEntity,this);
        }

#if UNITY_EDITOR

        [ShowInInspector]
        private List<TimerActionEditor> Actions
        {
            get
            {
                return TimedActions.Select(action => new TimerActionEditor
                    {Act = action.Act.Method.Name, Delay = action.Delay}).ToList();
            }
        }

        [Button(ButtonSizes.Medium)]
        private void StopTimers()
        {
            var t = this.gameObject.GetComponent<ITimer>();
            t?.FinishTimer();
        }

        [Button(ButtonSizes.Medium)]
        private void StartTimers()
        {
            var t = this.gameObject.GetComponent<ITimer>();
            t?.StartTimer();
        }

#endif
        
    }

    public struct TimerAction
    {
        public Action Act;
        public float Delay;
    }

    public struct TimerActionEditor
    {
        public string Act;
        public float Delay;
    }
}