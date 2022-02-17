namespace Crimson.Core.Utils
{
	[System.AttributeUsage(System.AttributeTargets.Field |
						   System.AttributeTargets.Property)]
	public class CastToUI : System.Attribute
	{
		private string _fieldId;

		public CastToUI(string fieldId)
		{
			_fieldId = fieldId;
		}

		public string FieldId => _fieldId;
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