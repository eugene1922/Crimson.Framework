using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crimson.Core.Loading;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class BootstrapLoader : MonoBehaviour
{
    [ValueDropdown("GetBootstraps")]
    public GameObject bootstrapPrefab = null;
    
    void Start()
    {
        if (bootstrapPrefab == null)
        {
            Debug.LogException(new Exception("[LEVEL LOAD] No valid LevelBootstrap Prefab specified!"));
        }

        GameObject.Instantiate(bootstrapPrefab, this.gameObject.transform);
    }

    private static IEnumerable<GameObject> GetBootstraps()
    {
        GameObject[] results;
        var scripts = Resources.FindObjectsOfTypeAll<LevelBootstrap>().Where(b => (b.gameObject.scene.rootCount == 0) && (b.gameObject.tag.Equals("BootstrapPrefab")));
        results = scripts.Convert((b) => ((LevelBootstrap)b).gameObject).ToArray();
        return results;
    }
}
