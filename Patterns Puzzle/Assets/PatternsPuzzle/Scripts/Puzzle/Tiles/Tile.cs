using System;
using System.Collections.Generic;
using GameControllers;
using OknaaEXTENSIONS;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PuzzleSystem {
    public class Tile : MonoBehaviour {
        public Image spriteRenderer;
        public TileShadow shadow;

        public int X;
        public int Y;
        public bool isTaken;
        public TileGroup tileGroupParent;

        public TileMovement TileMovementScript => _tileMovement;
        private TileMovement _tileMovement;

        public RectTransform RectTransform => _rectTransform;
        private RectTransform _rectTransform;

        private readonly List<Tile> neighbouringTiles = new List<Tile>();
        private Puzzle _puzzle;
        private float _combiningChance;
        private int _numberOfTilesToCombineWith = 0;
        private bool _isMatched = false;

        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _tileMovement = GetComponent<TileMovement>();
        }

        public void Init(Puzzle puzzle, Vector2Int coord, Sprite sprite) {
            _puzzle = puzzle;
            X = coord.x;
            Y = coord.y;
            _combiningChance = _puzzle.combiningChance;
            _numberOfTilesToCombineWith = _puzzle.numberOfTilesToCombine;
            spriteRenderer.sprite = sprite;
            gameObject.name = $"Tile {X} {Y}";
            
            shadow.Init(_puzzle, coord, this);
        }
        

        public void SetUpNeighbours() {
            if (_puzzle.GetTileByIndex(X, Y + 1, out var tileAbove)) neighbouringTiles.Add(tileAbove);
            if (_puzzle.GetTileByIndex(X, Y - 1, out var tileBelow)) neighbouringTiles.Add(tileBelow);
            if (_puzzle.GetTileByIndex(X - 1, Y, out var tileLeft)) neighbouringTiles.Add(tileLeft);
            if (_puzzle.GetTileByIndex(X + 1, Y, out var tileRight)) neighbouringTiles.Add(tileRight);
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
            tileGroup.Init(tilesToCombine, _puzzle, this);

            tileGroupRectTransform.localPosition = transform.localPosition;

            foreach (var tile in tilesToCombine) {
               tile.HandleTileGrouping(tileGroup);
            }
            
            tileGroup.transform.SetParent(_puzzle.tilesContainer);
            _puzzle.tileGroups.Add(tileGroup);
        }

        private void HandleTileGrouping(TileGroup tileGroup) {
            isTaken = true;
            tileGroupParent = tileGroup;
            transform.SetParent(tileGroup.transform);
            _numberOfTilesToCombineWith--;
        }
        
        public void ResizeToOriginalSize() {
            var tileRectTransform = gameObject.GetComponent<RectTransform>();
            tileRectTransform.localScale = Vector3.one; 
            tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _puzzle.TileDimensions.x);
            tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _puzzle.TileDimensions.y);
        }

        private bool AllNeighbouringTilesAreTaken => neighbouringTiles.TrueForAll(tile => tile.isTaken);
        private bool CannotCombine() => !(Random.Range(0f, 1f) < _combiningChance);

        
        
    }
}