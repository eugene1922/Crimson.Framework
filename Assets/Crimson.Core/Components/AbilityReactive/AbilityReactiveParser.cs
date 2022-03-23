using Crimson.Core.Common;
using Crimson.Core.Components.Interfaces;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Components.AbilityReactive
{
    [HideMonoScript]
    public class AbilityReactiveParser : MonoBehaviour,
        IActorAbility
    {
        [HideInInspector] public Dictionary<InputTypes, List<IActorAbility>> bindingsDict;
        public List<ReactiveInputBehaviour> InputBehaviours = new List<ReactiveInputBehaviour>();

        private EntityManager _dstManager;
        private float _lastInput;
        private float2 _lastStickInput;
        public IActor Actor { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            bindingsDict = new Dictionary<InputTypes, List<IActorAbility>>();

            Actor = actor;

            _dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _dstManager.AddComponentData(entity, new UserReactiveInput());
            for (var i = 0; i < InputBehaviours.Count; i++)
            {
                AddCustomBinding(InputBehaviours[i]);
            }
        }

        public void AddCustomBinding(ReactiveInputBehaviour behaviour)
        {
            if (!InputBehaviours.Contains(behaviour))
            {
                InputBehaviours.Add(behaviour);
            }

            var actions = behaviour.Actions.ConvertAll(a => a as IActorAbility);
            if (!bindingsDict.Keys.Contains(behaviour.InputType))
            {
                bindingsDict.Add(behaviour.InputType, new List<IActorAbility>(actions.Count));
            }

            bindingsDict[behaviour.InputType].AddRange(actions);
        }

        public void Execute()
        {
        }

        public void Parse(int index, PlayerInputData inputData)
        {
            var input = inputData.CustomInput[index];
            var stickInput = inputData.CustomSticksInput[index];
            foreach (var key in bindingsDict.Keys)
            {
                switch (key)
                {
                    case InputTypes.OnDrag:
                        ParseDrag(stickInput);
                        break;

                    case InputTypes.OnHold:
                        ParseHold(input);
                        break;

                    case InputTypes.OnClickDown:
                        ParseClickDown(input);
                        break;

                    case InputTypes.OnClickUp:
                        ParseClickUp(input);
                        break;

                    default:
                        break;
                }
            }
            _lastInput = input;
            _lastStickInput = stickInput;
        }

        private void InvokeActionsByType(InputTypes inputType)
        {
            var actions = bindingsDict[inputType];
            for (var i = 0; i < actions.Count; i++)
            {
                actions[i].Execute();
            }
        }

        private void InvokeBeginDrag(float2 input, List<IActorAbility> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                actions[i].Execute();
                if (actions[i] is IDragable dragable)
                {
                    dragable.BeginDrag(input);
                }
            }
        }

        private void InvokeDrag(float2 input, List<IActorAbility> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                actions[i].Execute();
                if (actions[i] is IDragable dragable)
                {
                    dragable.Drag(input);
                }
            }
        }

        private void InvokeEndDrag(float2 input, List<IActorAbility> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                actions[i].Execute();
                if (actions[i] is IDragable dragable)
                {
                    dragable.EndDrag(input);
                }
            }
        }

        private void ParseClickDown(float input)
        {
            if (_lastInput == 0 && input == 1)
            {
                InvokeActionsByType(InputTypes.OnClickDown);
            }
        }

        private void ParseClickUp(float input)
        {
            if (_lastInput == 1 && input == 0)
            {
                InvokeActionsByType(InputTypes.OnClickUp);
            }
        }

        private void ParseDrag(float2 stickInput)
        {
            var actions = bindingsDict[InputTypes.OnDrag];

            if (math.length(_lastStickInput) == 0 && math.length(stickInput) > 0)
            {
                InvokeBeginDrag(stickInput, actions);
            }

            if (math.length(stickInput) > 0)
            {
                InvokeDrag(stickInput, actions);
            }

            if (math.length(stickInput) == 0 && math.length(_lastStickInput) > 0)
            {
                InvokeEndDrag(stickInput, actions);
            }
        }

        private void ParseHold(float input)
        {
            if (_lastInput == 1 && input == 1)
            {
                InvokeActionsByType(InputTypes.OnHold);
            }
        }
    }
}