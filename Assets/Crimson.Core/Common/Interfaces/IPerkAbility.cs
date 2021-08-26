namespace Crimson.Core.Common
{
    public interface IPerkAbility
    {
        void Apply(IActor target);
        void Remove();
    }
}