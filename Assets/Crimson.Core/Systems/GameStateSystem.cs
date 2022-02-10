using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils.LowLevel;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crimson.Core.Systems
{
    [UpdateInGroup(typeof(FixedUpdateGroup))]
    public class GameStateSystem : ComponentSystem
    {
        private const string resultLogMessage = "[GAMESTATE] Appmetrica Finish event {0}";
        private EntityQuery _queryGameState;
        private EntityQuery _queryPlayers;
        private EntityQuery _queryUser;
        private BufferFromEntity<SpawnPrefabData> _spawnBuffers;
        private EntityQuery _spawners;
        private EntityQuery _useItemQuery;
        private Dictionary<string, object> metricaEventDict;

        protected override void OnCreate()
        {
            _queryGameState = GetEntityQuery(
                ComponentType.ReadOnly<GameStateData>());
            _queryUser = GetEntityQuery(
                ComponentType.ReadOnly<UserInputData>());
            _queryPlayers = GetEntityQuery(
                ComponentType.ReadOnly<AbilityActorPlayer>());
            _spawners = GetEntityQuery(
                ComponentType.ReadOnly<SpawnBuffer>());
            _useItemQuery = GetEntityQuery(
                ComponentType.ReadOnly<UseItemData>());
            metricaEventDict = new Dictionary<string, object>();
        }

        protected override void OnUpdate()
        {
            var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _spawnBuffers = GetBufferFromEntity<SpawnPrefabData>();

            Entities.With(_queryGameState).ForEach(
                (Entity entity, GameState state) =>
                {
                    Entities.With(_spawners).ForEach(
                        (Entity actorEntity, ref SpawnBuffer tag) =>
                        {
                            if (_spawnBuffers.HasComponent(actorEntity))
                            {
                                var buffer = _spawnBuffers[actorEntity];

                                for (var i = 0; i < buffer.Length; i++)
                                {
                                    var item = buffer[i];
                                    var prefab = state.PrefabRepository.Get(item.ID);
                                    ActorSpawn.SimpleSpawn(prefab, item.Position, item.Rotation);
                                }

                                buffer.Clear();
                                EntityManager.RemoveComponent<SpawnBuffer>(actorEntity);
                            }
                        });
                    if (state.userPlayer == null)
                    {
                        Entities.With(_queryUser).ForEach((AbilityActorPlayer player) =>
                        {
                            state.userPlayer = player;
                        });
                        if (state.userPlayer != null)
                        {
                            metricaEventDict.Clear();
                            metricaEventDict.Add("level", 1);
                            Debug.Log("[GAMESTATE] Appmetrica level start");
                            var panels = ActorSpawn.Spawn(state.sampleSpawner, state.userPlayer.Actor);

                            state.startTime = Time.ElapsedTime;
                            if (panels == null) return;

                            foreach (var panel in panels)
                            {
                                panel.transform.SetParent(state.rootCanvas.transform);
                                var r = panel.GetComponent<RectTransform>();
                                r.localScale = Vector3.one;
                                r.anchoredPosition = Vector2.zero;
                                r.anchorMax = Vector2.one;
                                r.anchorMin = Vector2.zero;
                                r.offsetMax = Vector2.one;
                                r.offsetMin = Vector2.zero;
                                r.sizeDelta = Vector2.one;
                                panel.SetActive(false);
                            }

                            state.respawnPanel = panels[0];
                            state.winPanel = panels[1];
                            state.losePanel = panels[2];

                            var resp = state.respawnPanel.GetComponent<UIGameStatePanel>();
                            resp.actionButton.onClick.AddListener(() =>
                            {
                                state.respawnPanel.SetActive(false);
                                dstManager.RemoveComponent<DeadActorData>(state.userPlayer.Actor.ActorEntity);
                                state.userPlayer.UpdateHealthData(state.userPlayer.MaxHealth);
                                var a = state.userPlayer.Actor.GameObject.GetComponent<Animator>();
                                a.SetBool("Dead", false);
                            });
                            resp.secondActionButton.onClick.AddListener(() =>
                            {
                                metricaEventDict.Clear();
                                metricaEventDict.Add("level", 1);
                                metricaEventDict.Add("result", "lose");
                                metricaEventDict.Add("time", (int)(Time.ElapsedTime - state.startTime));
                                metricaEventDict.Add("progress", 100);
                                Debug.Log("[GAMESTATE] Appmetrica Finish event lose");
                                dstManager.DestroyEntity(dstManager.UniversalQuery);
                                SceneManager.LoadScene(0);
                            });

                            var lose = state.losePanel.GetComponent<UIGameStatePanel>();
                            lose.actionButton.onClick.AddListener(() =>
                            {
                                dstManager.DestroyEntity(dstManager.UniversalQuery);
                                SceneManager.LoadScene(0);
                            });

                            var win = state.winPanel.GetComponent<UIGameStatePanel>();
                            win.actionButton.onClick.AddListener(() =>
                            {
                                dstManager.DestroyEntity(dstManager.UniversalQuery);
                                SceneManager.LoadScene(0);
                            });
                        }
                    }

                    if (state.userPlayer == null) return;

                    Entities.With(_useItemQuery).ForEach(
                        (Entity actorEntity, ref UseItemData data) =>
                        {
                            var prefab = state.PrefabRepository.Get(data.Item.ID);
                            var perks = prefab.GetComponentsInChildren<IPerkAbility>();
                            for (var i = 0; i < perks.Length; i++)
                            {
                                perks[i].Apply(state.userPlayer.Actor);
                            }
                            EntityManager.RemoveComponent<UseItemData>(actorEntity);
                        }
                    );

                    state.players.Clear();

                    Entities.With(_queryPlayers).ForEach((AbilityActorPlayer player) =>
                    {
                        state.players.Add(player);
                    });

                    if (!state.userPlayer.IsAlive)
                    {
                        if (state.userPlayer.deathCount <= state.maxDeathCount)
                        {
                            if (state.respawnPanel.activeSelf == false)
                            {
                                state.respawnPanel.SetActive(true);
                            }
                        }
                        else
                        {
                            ShowEndPanel(state, EndResultType.Lose);
                        }
                    }

                    if (state.NeedCheckEndGame
                        && state.players.Count(s => s.CompareTag(state.enemyTag)) == 0
                        && state.players.Contains(state.userPlayer))
                    {
                        ShowEndPanel(state, EndResultType.Win);
                    }
                });
        }

        private void ShowEndPanel(GameState state, EndResultType endResult)
        {
            var targetPanel = endResult == EndResultType.Lose ? state.losePanel : state.winPanel;
            if (targetPanel.activeSelf)
            {
                return;
            }
            metricaEventDict.Clear();
            metricaEventDict.Add("level", 1);
            metricaEventDict.Add("result", "win");
            metricaEventDict.Add("time", (int)(Time.ElapsedTime - state.startTime));
            metricaEventDict.Add("progress", 100);
            Debug.LogFormat(resultLogMessage, endResult.ToString().ToLower());
            targetPanel.SetActive(true);
        }
    }
}