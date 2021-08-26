using System;
using Crimson.Core.Loading.ActorSpawners;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Crimson.Core.Common
{
    [HideMonoScript]
    public class UIRespawnScreenView : MonoBehaviour
    {
        [SerializeField] private Button respawnButton;
        [SerializeField] private Actor actor;
        
        private LevelActorSpawnBehaviour _levelActorSpawnBehaviour;

        public void Start()
        {
            gameObject.SetActive(false);

            _levelActorSpawnBehaviour = FindObjectOfType<LevelActorSpawnBehaviour>();
        }
        
        public void ShowRespawnWindow(Action onRespawnAction)
        {
            gameObject.SetActive(true);
            
            respawnButton.onClick.AddListener(() =>
            {
                PlayerRespawn();
                onRespawnAction.Invoke();
                
                gameObject.DestroyWithEntity(actor.ActorEntity);
            });
        }
        
        private void PlayerRespawn()
        {
            if (_levelActorSpawnBehaviour == null) return;
            
            _levelActorSpawnBehaviour.Spawn();
            _levelActorSpawnBehaviour.RunSpawnActions();
        }
    }
}