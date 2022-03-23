using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Crimson.Core.Common
{
    [HideMonoScript]
    public class AvailablePerks : MonoBehaviour
    {
        public List<GameObject> AvailablePerksList = new List<GameObject>();
        public List<GameObject> PresetPerksList = new List<GameObject>();

        public List<GameObject> CheatPerksList = new List<GameObject>();
    }
}