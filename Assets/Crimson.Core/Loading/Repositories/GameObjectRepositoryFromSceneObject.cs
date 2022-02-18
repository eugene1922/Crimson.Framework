using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crimson.Core.Loading.Repositories
{
	public sealed class GameObjectRepositoryFromSceneObject : MonoBehaviour, IGameObjectRepository
	{
		public GameObject[] gameObjects;

		T IGameObjectRepository.Get<T>(string name)
		{
			var foundGameObject = gameObjects.FirstOrDefault(prefab => string.Equals(prefab.name, name, StringComparison.Ordinal)) as T;

			return foundGameObject ?? throw new KeyNotFoundException($"GameObject with name '{name}' not found at GameObject repository!");
		}
	}
}