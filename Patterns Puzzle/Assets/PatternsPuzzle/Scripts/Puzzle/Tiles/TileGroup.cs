using System;
using System.Collections.Generic;
using GameControllers;
using OknaaEXTENSIONS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PuzzleSystem {
    public class TileGroup : MonoBehaviour {
        public List<Tile> tiles = new List<Tile>();
        [SerializeField] private EventTrigger eventTrigger;

        private Puzzle _puzzle;
        private Vector3 _originalPosition;
        private Transform _originalParent;
        private int _startIndex;
        private TileGroupMovement _groupMovementScript;
        private GameObject _tileGroupInstance;
        private bool _isInInventory = true;

        public int TilesValue { get; private set; }

        private bool IsSnapped => _groupMovementScript.IsSnapped;
        private bool IsInCorrectPlace => _groupMovementScript.IsInCorrectPlace;
        private bool IsMatched => _groupMovementScript.OriginTile.IsMatched;

        private bool IsDragged {
            set => _groupMovementScript.IsDragged = value;
        }


        private void Awake() {
            _groupMovementScript = GetComponent<TileGroupMovement>();
        }

        private void Start() {
            SetEventTrigger(EventTriggerType.PointerDown, OnPointerDown);
            SetEventTrigger(EventTriggerType.Drag, OnDrag);
            SetEventTrigger(EventTriggerType.PointerUp, OnPointerUp);
        }

        private void OnPointerDown(BaseEventData data) {
            if (IsMatched) return;
            // if (_isInInventory) {
            var tileTransform = transform;
            _originalPosition = tileTransform.position;
            _originalParent = tileTransform.parent;
            _startIndex = transform.GetSiblingIndex();

            // SetTilePlaceHolder();
            // }

            transform.SetParent(CanvasController.Instance.Canvas.transform);
        }

        private void SetTilePlaceHolder() {
            _tileGroupInstance = Instantiate(gameObject, _originalPosition, Quaternion.identity, _originalParent);
            _tileGroupInstance.transform.SetSiblingIndex(_startIndex);
            Destroy(_tileGroupInstance.GetComponent<TileGroup>());
            var tiles = _tileGroupInstance.GetComponentsInChildren<Tile>();
            foreach (var tile in tiles) {
                Destroy(tile.GetComponent<TileGroupMovement>());
            }
        }

        private void OnDrag(BaseEventData data) {
            if (IsMatched) return;
            transform.position = ((PointerEventData)data).position;
            SetIsDragged(true);
        }

        private void OnPointerUp(BaseEventData data) {
            SetIsDragged(false);
            if (IsInCorrectPlace) {
                _groupMovementScript.SnapTileToCorrectPosition();
                return;
            }

            transform.SetParent(_originalParent);
            transform.SetPositionAndRotation(_originalPosition, Quaternion.identity);
            transform.SetSiblingIndex(_startIndex);

            RefreshScrollListUI();
        }

        private void RefreshScrollListUI() {
            var tileContainer = _puzzle.tilesContainer;
            tileContainer.GetChild(1).transform.SetAsFirstSibling();
            tileContainer.GetChild(1).transform.SetAsFirstSibling();
        }

        public void Init(List<Tile> groupedTiles, Puzzle puzzle, Tile originTile) {
            if (_groupMovementScript == null) _groupMovementScript = GetComponent<TileGroupMovement>();
            _groupMovementScript.OriginTile = originTile;
            tiles = groupedTiles;
            TilesValue = tiles.Count;
            _puzzle = puzzle;
            name = "TileGroup " + (tiles.Count);


            ResizeTiles();
        }

        private void ResizeTiles() {
            foreach (var tile in tiles) {
                tile.ResizeToOriginalSize();
            }
        }

        private void SetEventTrigger(EventTriggerType type, UnityAction<BaseEventData> action) {
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            eventTrigger.triggers.Add(entry);
        }
        
        private void SetIsDragged(bool value) {
            IsDragged = value;
            foreach (var tile in tiles) {
                //tile.TileGroupMovementScript.IsDragged = value;
                tile.TileShadowDetectorScript.IsDragged = value;
            }
        }
    }
}