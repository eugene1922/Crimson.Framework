using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace Crimson.Core.Common
{
	public struct CustomDeviceState : IInputStateTypeInfo
	{
		[InputControl(name = "customStick_1", format = "VEC2", layout = "Stick", displayName = "First Stick")]
		public Vector2 customStick_1;

		[InputControl(name = "customStick_2", format = "VEC2", layout = "Stick", displayName = "Second Stick")]
		public Vector2 customStick_2;

		[InputControl(name = "customStick_3", format = "VEC2", layout = "Stick", displayName = "Third Stick")]
		public Vector2 customStick_3;

		[InputControl(name = "customStick_4", format = "VEC2", layout = "Stick", displayName = "Third Stick")]
		public Vector2 customStick_4;

		public FourCC format => new FourCC('C', 'U', 'S', 'T');
	}

#if UNITY_EDITOR

	[InitializeOnLoad]
#endif
	[InputControlLayout(stateType = typeof(CustomDeviceState))]
	public class CustomDevice : InputDevice, IInputUpdateCallbackReceiver
	{
#if UNITY_EDITOR

		static CustomDevice()
		{
			Initialize();
		}

#endif

		public static CustomDevice Current { get; private set; }

		public StickControl CustomStick_1 { get; private set; }

		public StickControl CustomStick_2 { get; private set; }

		public StickControl CustomStick_3 { get; private set; }

		public StickControl CustomStick_4 { get; private set; }

		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Current = this;
		}

		public void OnUpdate()
		{
		}

		protected override void FinishSetup()
		{
			base.FinishSetup();

			CustomStick_1 = GetChildControl<StickControl>("customStick_1");
			CustomStick_2 = GetChildControl<StickControl>("customStick_2");
			CustomStick_3 = GetChildControl<StickControl>("customStick_3");
			CustomStick_3 = GetChildControl<StickControl>("customStick_4");
		}

		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (Current == this)
			{
				Current = null;
			}
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			InputSystem.RegisterLayout<CustomDevice>(
				matches: new InputDeviceMatcher()
					.WithInterface("CustomDevice"));
		}
	}
}