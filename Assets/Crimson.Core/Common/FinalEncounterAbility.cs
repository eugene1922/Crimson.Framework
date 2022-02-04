using Crimson.Core.Components;
using Crimson.Core.Utils;
using Unity.Entities;

namespace Crimson.Core.Common
{
    public class FinalEncounterAbility : TimerBaseBehaviour, IActorAbility
    {
        public GameState gamestate;

        public float delay = 4f;
        public IActor Actor { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            if (gamestate == null) gamestate = FindObjectOfType<GameState>();
        }

        public void Execute()
        {
            if (gamestate == null) return;
            Timer.TimedActions.AddAction(TurnItOn, delay);
        }

        private void TurnItOn()
        {
            gamestate.NeedCheckEndGame = true;
        }
    }
}