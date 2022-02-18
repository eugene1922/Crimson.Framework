using Crimson.Core.Components;
using Crimson.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Crimson.Core.Common
{
	public class UIPerkListAbility : MonoBehaviour, IActorAbility
	{
		public int NumberOfPerksToShow = 3;

		public GameObject PerkButtonPrefab;

		public Transform RootUIObject;
		public IActor Actor { get; set; }

		private List<GameObject> AvailablePerks => GameMeta.AvailablePerksList;

		[HideInInspector]
		public List<GameObject> spawnedButtons = new List<GameObject>();

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<UIAvailablePerksPanelData>(Actor.ActorEntity);
		}

		public void Execute()
		{
			AvailablePerks.Shuffle();

			for (var i = 0; i < NumberOfPerksToShow && i < AvailablePerks.Count; i++)
			{
				var b = GameObject.Instantiate(PerkButtonPrefab, RootUIObject ?? transform);
				var p = AvailablePerks[i].GetComponent<IPerkUpgrade>();
				if (p == null)
				{
					Debug.LogError("[PERK UPGRADE SETUP] Perk Upgrade Prefab must have Perk Upgrade Component derived from IPerkUpgrade!");
					continue;
				}

				var perkUpgradeButton = b.GetComponent<PerkUpgradeButton>();
				if (perkUpgradeButton == null)
				{
					Debug.LogError("[PERK UPGRADE SETUP] Perk Button Prefab must have Perk Upgrade Button Component!");
					continue;
				}

				if (p.PerkImage != null)
				{
					perkUpgradeButton.SetImage(p.PerkImage);
				}

				perkUpgradeButton.SetText(p.PerkName);

				var button = b.GetComponent<Button>();
				if (button == null)
				{
					Debug.LogError("[PERK UPGRADE SETUP] Perk Button Prefab must have Button Component!");
					continue;
				}

				button.onClick.AddListener(() =>
				{
					p.SpawnPerk(Actor.Spawner);
					CleanUp(NumberOfPerksToShow);
				});

				spawnedButtons.Add(b);
			}
		}

		public void CleanUp(int amount)
		{
			var amountToDestroy = amount <= spawnedButtons.Count ? amount : spawnedButtons.Count;

			var buttonsToDestroy = spawnedButtons.Take(amountToDestroy);

			foreach (var button in buttonsToDestroy)
			{
				Destroy(button);
			}

			spawnedButtons.RemoveRange(0, amountToDestroy);
		}
	}

	public struct UIAvailablePerksPanelData : IComponentData
	{
	}
}