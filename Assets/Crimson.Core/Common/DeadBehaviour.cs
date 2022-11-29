using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Crimson.Core.Common
{
	[Serializable]
	public class DeadBehaviour
	{
		public bool RemoveTag = false;

		[HideIf(nameof(ReceiveOverDamage))]
		public bool RemoveRigidbody = true;

		[HideIf(nameof(ReceiveOverDamage))]
		public bool RemoveColliders = true;

		public bool RemoveInput = true;
		public bool ReceiveOverDamage = false;

		public List<string> OnDeathActionsComponentNames = new List<string>();

		public List<string> OnOverdamageActionsComponentNames = new List<string>();
	}
}