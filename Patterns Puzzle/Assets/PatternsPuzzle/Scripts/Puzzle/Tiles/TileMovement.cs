using System;
using System.Collections.Generic;
using GameControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;

namespace PuzzleSystem {
    public class TileMovement : MonoBehaviour {
        [SerializeField] private EventTrigger eventTrigger;

        private Vector3 _originalPosition;
        private Transform _originalParent;
        private int _startIndex;
        private Tile _thisTile;
        private Camera _mainCamera;
        private bool _isDragged;

        
        
        private void Awake() {
            _thisTile = GetComponent<Tile>();
        }

        private void Start() {
            _mainCamera = CameraController.Instance.Camera;
            
            
            SetEventTrigger(EventTriggerType.PointerDown, OnPointerDown);
            SetEventTrigger(EventTriggerType.Drag, OnDrag);
            SetEventTrigger(EventTriggerType.PointerUp, OnPointerUp);
        }

        void Update() {
            if (!_isDragged) return;
            
            var hits = CanvasController.Instance.CheckRaycast(Input.GetTouch(0).position);
            foreach (var hit in hits) {
                var hitGameObject = hit.gameObject;
                if (!hitGameObject.CompareTag("TileShadow")) continue;
                
                var shadow = hitGameObject.GetComponent<TileShadow>();
                if (shadow == null) continue;

                OnHoverOverEmptySlot(shadow);
            }
        }

        private void OnHoverOverEmptySlot(TileShadow shadow) {
            shadow.ActivateHighlight(true);
            transform.position = shadow.transform.position;
        }

        private void SetEventTrigger(EventTriggerType type, UnityAction<BaseEventData> action) {
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            eventTrigger.triggers.Add(entry);
        }

        private void OnPointerDown(BaseEventData data) {
            Transform tileTransform = transform;

            _originalPosition = tileTransform.position;
            _originalParent = tileTransform.parent;
            _startIndex = tileTransform.GetSiblingIndex();
            tileTransform.SetParent(CanvasController.Instance.Canvas.transform);
        }

        private void OnDrag(BaseEventData data) {
            _isDragged = true;
            transform.position = ((PointerEventData)data).position;
            
        }

        private bool TileIsOverEmptySlot(BaseEventData data, out TileShadow emptySlot) {
            Ray ray = _mainCamera.ScreenPointToRay(((PointerEventData)data).position);


            emptySlot = null;
            return true;
        }

        private void OnPointerUp(BaseEventData data) {
            _isDragged = false;
            if (TileIsOverItsRightPlace()) {
                PlaceTileInThePuzzle();
                return;
            }

            transform.SetParent(_originalParent);
            transform.SetPositionAndRotation(_originalPosition, Quaternion.identity);
            transform.SetSiblingIndex(_startIndex);
            RefreshScrollListUI();
        }

        private void PlaceTileInThePuzzle() { }

        private bool TileIsOverItsRightPlace() {
            return false;
        }

        private static void RefreshScrollListUI() {
            var tileContainer = PuzzleController.Instance._currentPuzzle.tilesContainer;
            tileContainer.GetChild(1).transform.SetAsFirstSibling();
        }
    }
}