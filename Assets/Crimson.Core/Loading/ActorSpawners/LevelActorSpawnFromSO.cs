using Crimson.Core.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Crimson.Core.Loading.ActorSpawners
{
	[HideMonoScript]
	public class LevelActorSpawnFromSO : MonoBehaviour, IActorSpawner, IComponentName
	{
		[Space] [SerializeField] public string componentName = "";

		[Space]
		public LevelActorSpawnerDataSO spawnDataFile;

		public string ComponentName
		{
			get => componentName;
			set => componentName = value;
		}

		public List<GameObject> SpawnedObjects { get; private set; }

		public void InitPool()
		{
			spawnDataFile.SpawnData.InitPool();
		}

		public void Spawn()
		{
			Assert.IsNotNull(spawnDataFile);

			var spawnItems = ActorSpawn.GenerateData(spawnDataFile.SpawnData);
			SpawnedObjects.Clear();
			for (var i = 0; i < spawnItems.Count; i++)
			{
				SpawnedObjects.Add(ActorSpawn.Spawn(spawnItems[i]));
			}
		}

		private void Awake()
		{
			InitPool();
		}
	}
}