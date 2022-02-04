using Crimson.Core.Components;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Crimson.Core.Common
{
    public class UIGameStatePanel : TimerBaseBehaviour, IActorAbility
    {
        public Button actionButton;
        public Button secondActionButton;
        public CanvasGroup canvasGroup;

        public float fadeInTime = 2f;
        public IActor Actor { get; set; }

        private Entity _entity;

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
            canvasGroup.alpha = 0f;
            _entity = entity;
        }

        private void OnEnable()
        {
            if (Actor == null) return;

            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, fadeInTime);
        }

        private void OnDisable()
        {
            canvasGroup.alpha = 0f;
        }

        public void Execute()
        {
        }
    }
}