using System;
using GameControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace PuzzleSystem {
    public class TileMovement : MonoBehaviour {
        [SerializeField] private EventTrigger eventTrigger;
        
        private Vector3 _startPosition;
        private int _startIndex;
        private Tile _thisTile;

        private void Awake() {
            _thisTile = GetComponent<Tile>();
        }

        private void Start() {
            SetEventTrigger(EventTriggerType.PointerDown, OnPointerDown);
            SetEventTrigger(EventTriggerType.Drag, OnDrag);
            SetEventTrigger(EventTriggerType.PointerUp, OnPointerUp);
        }

        private void SetEventTrigger(EventTriggerType type, UnityAction<BaseEventData> action) {
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            eventTrigger.triggers.Add(entry);
        }

        private void OnPointerDown(BaseEventData data) {
            _startPosition = transform.position;
            _startIndex = transform.GetSiblingIndex();
        }

        private void OnDrag(BaseEventData data) => transform.position = ((PointerEventData) data).position;
        private void OnPointerUp(BaseEventData data) {
            if (TileIsOverItsRightPlace()) {
                PlaceTileInThePuzzle();
                return;
            }
            
            transform.position = _startPosition;
            transform.SetSiblingIndex(_startIndex);
            var tileContainer = PuzzleController.Instance._currentPuzzle.tilesContainer;
            var child0 = tileContainer.GetChild(0);
            var child1 = tileContainer.GetChild(1);
            child1.transform.SetAsFirstSibling();
        }

        private void PlaceTileInThePuzzle() {
        }

        private bool TileIsOverItsRightPlace() {
            return false;
        }
    }
}