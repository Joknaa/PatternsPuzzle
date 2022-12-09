using System;
using GameControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PuzzleSystem {
    public class TileShadow : MonoBehaviour {
        [SerializeField] private EventTrigger eventTrigger;

        private Vector3 _startPosition;
        private int _startIndex;
        private Image _highlightImage;

        private void Awake() {
            _highlightImage = GetComponent<Image>();
            ActivateHighlight(false);
        }

        private void Start() {
            SetEventTrigger(EventTriggerType.PointerEnter, OnPointerEnter);
            SetEventTrigger(EventTriggerType.PointerExit, OnPointerExit);
        }

        private void SetEventTrigger(EventTriggerType type, UnityAction<BaseEventData> action) {
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            eventTrigger.triggers.Add(entry);
        }

        private void OnPointerEnter(BaseEventData data) => ActivateHighlight(true);
        private void OnPointerExit(BaseEventData data) => ActivateHighlight(false);
        

        
        private void OnDrag(BaseEventData data) => transform.position = ((PointerEventData)data).position;

        private void OnPointerUp(BaseEventData data) {
            transform.position = _startPosition;
            transform.SetSiblingIndex(_startIndex);
            var tileContainer = PuzzleController.Instance._currentPuzzle.tilesContainer;
            var child0 = tileContainer.GetChild(0);
            var child1 = tileContainer.GetChild(1);
            child1.transform.SetAsFirstSibling();
        }
        
        private void ActivateHighlight(bool active) => _highlightImage.enabled = active;

    }
}