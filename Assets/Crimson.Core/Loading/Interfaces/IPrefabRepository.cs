using UnityEngine;

namespace Crimson.Core.Loading
{
	public interface IPrefabRepository
	{
		GameObject Get(int key);
	}
}