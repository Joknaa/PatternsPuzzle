using GameControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PuzzleSystem {
    public class TileMovement : MonoBehaviour {
        [SerializeField] private EventTrigger eventTrigger;

        private Vector3 _originalPosition;
        private Transform _originalParent;
        private int _startIndex;
        private Tile _thisTile;
        private Camera _mainCamera;
        private bool _isDragged;
        private bool _isSnapped;

        
        
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

            HandleTileMovement();
        }

        private void HandleTileMovement() {
            var hits = CanvasController.Instance.CheckRaycast(Input.GetTouch(0).position);
            foreach (var hit in hits) {
                var hitGameObject = hit.gameObject;
                if (hitGameObject.CompareTag("TileShadow")) {
                    var shadow = hitGameObject.GetComponent<TileShadow>();
                    OnHoverOverEmptySlot(shadow);
                    break;
                }

                _isSnapped = false;
            }
        }

        private void OnHoverOverEmptySlot(TileShadow shadow) {
            _isSnapped = true;
            // shadow.ActivateHighlight(true);
            transform.position = shadow.transform.position;
        }

        private void OnPointerDown(BaseEventData data) {
            Transform tileTransform = transform;
            _originalPosition = tileTransform.position;
            _originalParent = tileTransform.parent;
            _startIndex = tileTransform.GetSiblingIndex();
            tileTransform.SetParent(PuzzleController.Instance.CurrentPuzzle.tileShadowsContainer);
            // tileTransform.SetParent(CanvasController.Instance.Canvas.transform);
        }

        private void OnDrag(BaseEventData data) {
            _isDragged = true;
            transform.position = ((PointerEventData)data).position;
        }
        
        private void OnPointerUp(BaseEventData data) {
            _isDragged = false;
            if (_isSnapped) return;
            
            transform.SetParent(_originalParent);
            transform.SetPositionAndRotation(_originalPosition, Quaternion.identity);
            transform.SetSiblingIndex(_startIndex);
            RefreshScrollListUI();
        }

        private void SetEventTrigger(EventTriggerType type, UnityAction<BaseEventData> action) {
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            eventTrigger.triggers.Add(entry);
        }
        
        private static void RefreshScrollListUI() {
            var tileContainer = PuzzleController.Instance.CurrentPuzzle.tilesContainer;
            tileContainer.GetChild(1).transform.SetAsFirstSibling();
        }
    }
}