using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Crimson.Core.Common.UI
{
	public class LoadLevelAsync : MonoBehaviour
	{
		[SerializeField] private UnityEvent _onLoad;

		public void Load(int index)
		{
			SceneManager.LoadSceneAsync(index);
			_onLoad?.Invoke();
		}

		public void Load(string name)
		{
			SceneManager.LoadSceneAsync(name);
			_onLoad?.Invoke();
		}
	}
}