using Crimson.Core.Loading;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BootstrapLoader : MonoBehaviour
{
	[ValueDropdown(nameof(GetBootstraps))]
	public GameObject bootstrapPrefab = null;

	private static IEnumerable<GameObject> GetBootstraps()
	{
		GameObject[] results;
		var scripts = Resources.FindObjectsOfTypeAll<LevelBootstrap>().Where(b => (b.gameObject.scene.rootCount == 0) && b.gameObject.CompareTag("BootstrapPrefab"));
		results = scripts.Convert((b) => ((LevelBootstrap)b).gameObject).ToArray();
		return results;
	}

	private void Start()
	{
		if (bootstrapPrefab == null)
		{
			Debug.LogException(new Exception("[LEVEL LOAD] No valid LevelBootstrap Prefab specified!"));
		}

		GameObject.Instantiate(bootstrapPrefab, this.gameObject.transform);
	}
}