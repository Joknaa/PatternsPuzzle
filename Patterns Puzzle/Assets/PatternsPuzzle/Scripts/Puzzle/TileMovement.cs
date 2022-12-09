using System;
using GameControllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace PuzzleSystem {
    public class TileMovement : MonoBehaviour {
        [SerializeField] private EventTrigger eventTrigger;
        
        private Vector3 _startPosition;
        private int _startIndex;
        
        
        private void Start() {
            var onPointerDownEvent = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            var onDragEvent = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
            var onPointerUpEvent = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            
            eventTrigger.triggers.Add(onPointerDownEvent);
            eventTrigger.triggers.Add(onDragEvent);
            eventTrigger.triggers.Add(onPointerUpEvent);

            onPointerDownEvent.callback.AddListener(OnPointerDown);
            onDragEvent.callback.AddListener(OnDrag);
            onPointerUpEvent.callback.AddListener(OnPointerUp);

        }

        private void OnPointerDown(BaseEventData data) {
            _startPosition = transform.position;
            _startIndex = transform.GetSiblingIndex();
        }

        private void OnDrag(BaseEventData data) => transform.position = ((PointerEventData) data).position;
        private void OnPointerUp(BaseEventData data) {
            transform.position = _startPosition;
            transform.SetSiblingIndex(_startIndex);
            var tileContainer = PuzzleController.Instance._currentPuzzle.tilesContainer;
            var child0 = tileContainer.GetChild(0);
            var child1 = tileContainer.GetChild(1);
            child1.transform.SetAsFirstSibling();
        }
    }
}