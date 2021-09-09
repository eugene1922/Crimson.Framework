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
        private Dictionary<string, object> metricaEventDict;

        protected override void OnCreate()
        {
            _queryGameState = GetEntityQuery(
                ComponentType.ReadOnly<GameStateData>());
            _queryUser = GetEntityQuery(
                ComponentType.ReadOnly<UserInputData>());
            _queryPlayers = GetEntityQuery(
                ComponentType.ReadOnly<AbilityActorPlayer>());
            metricaEventDict = new Dictionary<string, object>();
        }

        protected override void OnUpdate()
        {
            var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var metrica = AppMetrica.Instance;

            Entities.With(_queryGameState).ForEach(
                (Entity entity, GameState state) =>
                {
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
                            state.metrica.ReportEvent("level_start", metricaEventDict);
                            state.metrica.SendEventsBuffer();
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
                                state.metrica.ReportEvent("level_finish", metricaEventDict);
                                state.metrica.SendEventsBuffer();
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
                        && state.players.First() == state.userPlayer)
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
            state.metrica.ReportEvent("level_finish", metricaEventDict);
            state.metrica.SendEventsBuffer();
            Debug.LogFormat(resultLogMessage, endResult.ToString().ToLower());
            targetPanel.SetActive(true);
        }
    }
}