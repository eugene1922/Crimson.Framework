namespace Crimson.Core.Common
{
	public interface IUIProgressBar : IUIElement
	{
		string MaxValueAssociatedID { get; }

		void SetMaxValue(object maxValue);
	}
}