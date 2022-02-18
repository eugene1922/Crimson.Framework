namespace Crimson.Core.Utils
{
	[System.AttributeUsage(System.AttributeTargets.Field |
						   System.AttributeTargets.Property)]
	public class CastToUI : System.Attribute
	{
		public CastToUI(string fieldId)
		{
			FieldId = fieldId;
		}

		public string FieldId { get; }
	}

	[System.AttributeUsage(System.AttributeTargets.Field |
						   System.AttributeTargets.Property)]
	public class LevelableValue : System.Attribute
	{
	}

	[System.AttributeUsage(System.AttributeTargets.Field |
						   System.AttributeTargets.Property)]
	public class NetworkSimData : System.Attribute
	{
	}

	[System.AttributeUsage(System.AttributeTargets.Class |
							   System.AttributeTargets.Struct)]
	public class NetworkSimObject : System.Attribute
	{
	}
}