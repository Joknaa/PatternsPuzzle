using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PuzzleSystem {
    public class TileMovement : MonoBehaviour {
        [SerializeField] private Canvas canvas;
        
        private EventTrigger eventTrigger;    
        
        private void Awake() {
            canvas = FindObjectOfType<Canvas>();
            eventTrigger = GetComponent<EventTrigger>();
            if (eventTrigger == null) eventTrigger = gameObject.AddComponent<EventTrigger>();
            
            eventTrigger.triggers.Add(new EventTrigger.Entry {
                eventID = EventTriggerType.Drag,
                callback = new EventTrigger.TriggerEvent()
            });
            eventTrigger.triggers[0].callback.AddListener(OnDrag);
        }


        private void OnDrag(BaseEventData data) {
            PointerEventData pointerEventData = (PointerEventData) data;
            transform.position = pointerEventData.position;
        }
    }
}