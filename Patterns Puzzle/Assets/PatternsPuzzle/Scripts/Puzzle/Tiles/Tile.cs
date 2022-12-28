using System;
using System.Collections.Generic;
using GameControllers;
using OknaaEXTENSIONS;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PuzzleSystem {
    public class Tile : MonoBehaviour {
        public static Action<int> OnTilePlacedInCorrectSlot;
        public TileGroupMovement TileGroupMovementScript => _tileGroupMovement;
        private TileGroupMovement _tileGroupMovement;
        
        public TileShadowDetector TileShadowDetectorScript => _tileShadowDetector;
        private TileShadowDetector _tileShadowDetector;

        public Puzzle Puzzle => _puzzle;
        private Puzzle _puzzle;
        
        public Vector2Int Coordinates => new Vector2Int(x, y);


        public bool IsMatched { get; private set; } = false;

        public RectTransform RectTransform => _rectTransform;
        private RectTransform _rectTransform;

        private readonly List<Tile> neighbouringTiles = new List<Tile>();
        public TileGroup tileGroupParent;
        private TileSlot slot;
        private Image spriteRenderer;
        private float _combiningChance;
        private int _numberOfTilesToCombineWith = 0;
        private int x;
        private int y;
        private bool isTaken;


        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _tileGroupMovement = GetComponent<TileGroupMovement>();
            _tileShadowDetector = GetComponent<TileShadowDetector>();
            slot = GetComponentInChildren<TileSlot>();
            spriteRenderer = GetComponent<Image>();
        }

        #region Tile Generation

        public void Init(Puzzle puzzle, Vector2Int coord, Sprite sprite) {
            _puzzle = puzzle;
            x = coord.x;
            y = coord.y;
            _combiningChance = _puzzle.combiningChance;
            _numberOfTilesToCombineWith = _puzzle.numberOfTilesToCombine;
            gameObject.name = $"Tile {x} {y}";
            isTaken = false;

            if (spriteRenderer == null) spriteRenderer = GetComponent<Image>();
            spriteRenderer.sprite = sprite;
            if (slot == null) slot = GetComponentInChildren<TileSlot>();
            slot.Init(_puzzle, coord, this);
        }


        public void SetUpNeighbours() {
            if (_puzzle.GetTileByIndex(x, y + 1, out var tileAbove)) neighbouringTiles.Add(tileAbove);
            if (_puzzle.GetTileByIndex(x, y - 1, out var tileBelow)) neighbouringTiles.Add(tileBelow);
            if (_puzzle.GetTileByIndex(x - 1, y, out var tileLeft)) neighbouringTiles.Add(tileLeft);
            if (_puzzle.GetTileByIndex(x + 1, y, out var tileRight)) neighbouringTiles.Add(tileRight);
        }

        public void CombineTileWithRandomNeighbors() {
            if (isTaken) return;

            List<Tile> tilesToCombineWith = new List<Tile>() { this };
            if (CannotCombine()) {
                CreateTileGroup(tilesToCombineWith);
                return;
            }

            Tile randomTile;
            while (_numberOfTilesToCombineWith > 0) {
                if (AllNeighbouringTilesAreTaken) break;

                randomTile = neighbouringTiles.Random();
                if (randomTile.isTaken) continue;

                randomTile.isTaken = true;
                tilesToCombineWith.Add(randomTile);
                _numberOfTilesToCombineWith--;
            }

            CreateTileGroup(tilesToCombineWith);
        }

        private void CreateTileGroup(List<Tile> tilesToCombine) {
            var tileGroup = Instantiate(_puzzle.tileGroupPrefab, _puzzle.transform);
            var tileGroupRectTransform = tileGroup.GetComponent<RectTransform>();

            tileGroupRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _puzzle.TileDimensions.x);
            tileGroupRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _puzzle.TileDimensions.y);
            tileGroupRectTransform.localPosition = transform.localPosition;

            TileGroupNeighbors tileGroupNeighbors = new TileGroupNeighbors();
            foreach (var tile in tilesToCombine) {
                var direction = tile.transform.localPosition - transform.localPosition;
                tileGroupNeighbors.AddNeighbor(this, tile);
                tile.HandleTileGrouping(tileGroup);
            }

            tileGroup.Init(tilesToCombine, _puzzle, this, tileGroupNeighbors);

            tileGroup.transform.SetParent(_puzzle.tilesContainer);
            _puzzle.tileGroups.Add(tileGroup);
        }

        private void HandleTileGrouping(TileGroup tileGroup) {
            // isTaken = true;
            tileGroupParent = tileGroup;
            transform.SetParent(tileGroup.transform);
            //_numberOfTilesToCombineWith--;
        }

        public void ResizeToOriginalSize() {
            var tileRectTransform = gameObject.GetComponent<RectTransform>();
            tileRectTransform.localScale = Vector3.one;
            tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _puzzle.TileDimensions.x);
            tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _puzzle.TileDimensions.y);
        }

        private bool AllNeighbouringTilesAreTaken => neighbouringTiles.TrueForAll(tile => tile.isTaken);
        private bool CannotCombine() => !(Random.Range(0f, 1f) < _combiningChance);

        #endregion

        public bool TileMatchesSlot(TileSlot slotHoveredOver) => slotHoveredOver == slot;

        public void SetTileAsMatched() {
            tileGroupParent.transform.SetParent(_puzzle.transform);
            tileGroupParent.transform.position = slot.transform.position;
            OnTilePlacedInCorrectSlot?.Invoke(tileGroupParent.TilesValue);
            IsMatched = true;
        }
        
        
    }
}