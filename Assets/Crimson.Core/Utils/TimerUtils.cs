using Crimson.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crimson.Core.Utils
{
    public static class TimerUtils
    {
        public static void AddAction(this List<TimerAction> timedActions, Action action, float delay)
        {
            timedActions.Add(CreateAction(action, delay));
        }

        public static void AddAction(this TimerBaseBehaviour obj, Action action, float delay)
        {
            var t = obj.Timer;
            if (t == null) return;
            obj.Timer.TimedActions.Add(CreateAction(action, delay));
        }

        public static void AddAction(this TimerBaseBehaviour obj, Action action, Func<float> delay)
        {
            obj.Timer.TimedActions.Add(CreateAction(action, delay()));
        }

        public static void AddRepeatedAction(this TimerBaseBehaviour obj, Action action, Func<float> delay)
        {
            var d = delay();
            obj.Timer.TimedActions.Add(CreateAction(action, d));
            obj.Timer.TimedActions.Add(new TimerAction
            {
                Act = () =>
                {
                    if (obj.TimerActive) obj.AddRepeatedAction(action, delay);
                },
                Delay = d
            });
        }

        public static bool ContainsAction(this TimerBaseBehaviour obj, Action action)
        {
            return obj.Timer.TimedActions.Any(act => act.Act.Equals(action));
        }

        public static TimerAction CreateAction(Action action, float delay)
        {
            return new TimerAction { Act = action, Delay = delay };
        }

        public static TimerAction? GetCurrentTimer(this TimerBaseBehaviour obj)
        {
            if (obj.Timer == null) return null;
            if (!obj.Timer.TimedActions.Any()) return null;

            return obj.Timer.TimedActions.First();
        }

        public static TimerComponent GetOrCreateTimer(this GameObject obj, TimerComponent timer)
        {
            if (timer != null) return timer;
            return (timer = obj.GetComponent<TimerComponent>()) != null ? timer : obj.AddComponent<TimerComponent>();
        }

        public static void RemoveAction(this TimerBaseBehaviour obj, Action action)
        {
            var indexes = FindActionIndexes(obj, action);
            for (var i = 0; i < indexes.Count; i++)
            {
                obj.Timer.TimedActions.RemoveAt(indexes[i]);
            }
        }

        public static List<int> FindActionIndexes(this TimerBaseBehaviour obj, Action action)
        {
            var result = new List<int>();
            var t = obj.Timer;
            if (t == null) return result;
            var actions = t.TimedActions;
            for (var i = 0; i < actions.Count; i++)
            {
                if (!actions[i].Act.Equals(action))
                {
                    continue;
                }

                result.Add(i);
            }

            return result;
        }

        public static void RestartAction(this TimerBaseBehaviour obj, Action action, float delay)
        {
            RemoveAction(obj, action);
            obj.Timer.TimedActions.Add(CreateAction(action, delay));
        }
    }
}