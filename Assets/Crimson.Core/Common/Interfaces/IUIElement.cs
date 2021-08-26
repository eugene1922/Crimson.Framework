namespace Crimson.Core.Common
{
    public interface IUIElement
    {
        string AssociatedID { get; }
        void SetData(object input);
    }
}