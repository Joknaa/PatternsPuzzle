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
        private Transform _originalParent;
        private Vector3 _originalPosition;
        private Vector3 _originalScale;
        private int _startIndex;
        private TileGroupMovement _groupMovementScript;
        private GameObject _tileGroupInstance;
        private bool _isInInventory = true;
        private TileGroupNeighbors _tileGroupNeighbors;

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
            _originalParent = tileTransform.parent;
            _originalPosition = tileTransform.position;
            _originalScale = tileTransform.localScale;
            _startIndex = transform.GetSiblingIndex();

            transform.SetParent(CanvasController.Instance.Canvas.transform);
            transform.localScale = Vector3.one;
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
            if (IsMatched) return;
            
            SetIsDragged(false);
            if (IsInCorrectPlace) {
                _groupMovementScript.SnapTileToCorrectPosition();
                return;
            }

            transform.SetParent(_originalParent);
            transform.SetPositionAndRotation(_originalPosition, Quaternion.identity);
            transform.localScale = _originalScale;
            transform.SetSiblingIndex(_startIndex);

            RefreshScrollListUI();
        }

        private void RefreshScrollListUI() {
            var tileContainer = _puzzle.tilesContainer;
            tileContainer.GetChild(1).transform.SetAsFirstSibling();
            tileContainer.GetChild(1).transform.SetAsFirstSibling();
        }

        public void Init(List<Tile> groupedTiles, Puzzle puzzle, Tile originTile, TileGroupNeighbors tileGroupNeighbors) {
            if (_groupMovementScript == null) _groupMovementScript = GetComponent<TileGroupMovement>();
            _groupMovementScript.OriginTile = originTile;
            tiles = groupedTiles;
            TilesValue = tiles.Count;
            _puzzle = puzzle;
            _tileGroupNeighbors = tileGroupNeighbors;
            name = "TileGroup " + (tiles.Count);


            ResizeTileGroup();
            // ResizeTiles();
        }

        private void ResizeTileGroup() {
            var tileDimensions = _puzzle.TileDimensions;
            var fullScale = new Vector2(400 / tileDimensions.x, 400 / tileDimensions.y);
            var unifiedScale = new Vector2(fullScale.x * 0.3f, fullScale.y * 0.3f);
            transform.localScale = unifiedScale;
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

    public class TileGroupNeighbors {
        public Tile UP = null;
        public Tile DOWN = null;
        public Tile LEFT = null;
        public Tile RIGHT = null;
        
        public TileGroupNeighbors(Tile up = null, Tile down = null, Tile left = null, Tile right = null) {
            if (up != null) UP = up;
            if (down != null) DOWN = down;
            if (left != null) LEFT = left;
            if (right != null) RIGHT = right;
        }
        
        public void AddNeighbor(Tile originalTile, Tile neighborTile) {
            var originalCoords = originalTile.Coordinates;
            var neighborCoords = neighborTile.Coordinates;
            
            
            if (originalCoords.x - neighborCoords.x == 1) {
                LEFT = neighborTile;
            }
            else if (originalCoords.x - neighborCoords.x == -1) {
                RIGHT = neighborTile;
            }
            else if (originalCoords.y - neighborCoords.y == 1) {
                UP = neighborTile;
            }
            else if (originalCoords.y - neighborCoords.y == -1) {
                DOWN = neighborTile;
            }
            
        }

        public Vector2 GetTileGroupSize() {
            var size = new Vector2(0, 0);
            if (UP != null) size.y += 1;
            if (DOWN != null) size.y += 1;
            if (LEFT != null) size.x += 1;
            if (RIGHT != null) size.x += 1;
            return size;
        }
    }
}