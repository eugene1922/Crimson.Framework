using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Crimson.Core.Common
{
    [Serializable]
    public struct AbilityChainSettings
    {
        [SerializeField]
        public List<MonoBehaviour> Actions;
        public float Delay;
    }
}