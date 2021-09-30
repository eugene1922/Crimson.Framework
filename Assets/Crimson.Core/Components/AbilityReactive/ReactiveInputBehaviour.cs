using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Crimson.Core.Components.AbilityReactive
{
    [Serializable]
    public struct ReactiveInputBehaviour
    {
        public InputTypes InputType;

        [ValidateInput("MustBeAbility", "Ability MonoBehaviours must derive from IActorAbility!")]
        public List<MonoBehaviour> Actions;

        private bool MustBeAbility(List<MonoBehaviour> a)
        {
            return !a.Exists(t => !(t is IActorAbility)) || a.Count == 0;
        }
    }
}
