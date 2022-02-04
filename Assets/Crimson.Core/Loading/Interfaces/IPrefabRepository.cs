using UnityEngine;

namespace Crimson.Core.Loading
{
    public interface IPrefabRepository
    {
        GameObject Get(string name);

        GameObject Get(ushort key);
    }
}