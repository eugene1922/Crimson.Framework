namespace Crimson.Core.Loading
{
    public interface IPrefabRepository
    {
        T Get<T>(string name) where T : UnityEngine.Object;
        T Get<T>(ushort key) where T : UnityEngine.Object;
    }
}