using System.Collections.Generic;
using GameControllers;
using OknaaEXTENSIONS;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace PuzzleSystem {
    public class Tile : MonoBehaviour {
        public Image spriteRenderer;
        public TileShadow shadow;

        public int X;
        public int Y;
        public bool isTaken;
        public TileGroup tileGroupParent;

        private readonly List<Tile> neighbouringTiles = new List<Tile>();
        private Puzzle _puzzle;
        private float _combiningChance;
        private int _numberOfTilesToCombineWith = 0;

        private bool _isMatched = false;
        private RectTransform _rectTransform;
        
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

                tilesToCombineWith.Add(randomTile);
                _numberOfTilesToCombineWith--;
            }
            CreateTileGroup(tilesToCombineWith);
        }
        
        private void CreateTileGroup(List<Tile> tilesToCombine) {
            var tileGroup = Instantiate(_puzzle.tileGroupPrefab, _puzzle.tilesContainer);
            tileGroup.Init(tilesToCombine, _puzzle);
            
            var tileGroupRectTransform = tileGroup.GetComponent<RectTransform>();
            tileGroupRectTransform.localPosition = GetAveragePosition();

            foreach (var tile in tilesToCombine) {
               tile.HandleTileGrouping(tileGroup);
            }
            
            tileGroup.transform.SetParent(_puzzle.tilesContainer);
            _puzzle.tileGroups.Add(tileGroup);

            Vector3 GetAveragePosition() {
                var averagePosition = Vector3.zero;
                foreach (var tile in tilesToCombine) {
                    averagePosition += tile.GetComponent<RectTransform>().localPosition;
                }

                return averagePosition / tilesToCombine.Count;
            }
        }

        private void HandleTileGrouping(TileGroup tileGroup) {
            isTaken = true;
            tileGroupParent = tileGroup;
            transform.SetParent(tileGroup.transform);
            transform.localScale = Vector3.one;
            _numberOfTilesToCombineWith--;
        }
        
        private bool AllNeighbouringTilesAreTaken => neighbouringTiles.TrueForAll(tile => tile.isTaken);
        private bool CannotCombine() => !(Random.Range(0f, 1f) < _combiningChance);
    }
}