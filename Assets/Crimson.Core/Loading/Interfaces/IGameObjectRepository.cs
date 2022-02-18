namespace Crimson.Core.Loading
{
	public interface IGameObjectRepository
	{
		T Get<T>(string name) where T : UnityEngine.Object;
	}
}