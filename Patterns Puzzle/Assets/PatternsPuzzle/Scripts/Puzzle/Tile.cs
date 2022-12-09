using System.Collections.Generic;
using GameControllers;
using OknaaEXTENSIONS;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace PuzzleSystem {
    public class Tile : MonoBehaviour {
        public SpriteRenderer _spriteRenderer;

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

        public void Init(Puzzle puzzle, int x, int y, Sprite sprite) {
            _puzzle = puzzle;
            X = x;
            Y = y;
            _combiningChance = _puzzle.combiningChance;
            _numberOfTilesToCombineWith = _puzzle.numberOfTilesToCombine;
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.sortingOrder = 1;
            gameObject.name = $"Tile {x} {y}";
        }

        public void SetUpNeighbours() {
            if (_puzzle.GetTileByIndex(X, Y + 1, out var tileAbove)) neighbouringTiles.Add(tileAbove);
            if (_puzzle.GetTileByIndex(X, Y - 1, out var tileBelow)) neighbouringTiles.Add(tileBelow);
            if (_puzzle.GetTileByIndex(X - 1, Y, out var tileLeft)) neighbouringTiles.Add(tileLeft);
            if (_puzzle.GetTileByIndex(X + 1, Y, out var tileRight)) neighbouringTiles.Add(tileRight);
        }

        public void CombineTilesIntoRandomGroups() {
            if (isTaken) {
                if (_numberOfTilesToCombineWith <= 0) return;
                ExpandTileGroup();
                return;
            }
            if (CannotCombine()) return;

            Tile randomTile;
            List<Tile> tilesToCombineWith = new List<Tile>() { this };
            while (_numberOfTilesToCombineWith > 0) {
                if (AllNeighbouringTilesAreTaken) break;

                randomTile = neighbouringTiles.Random();
                if (randomTile.isTaken) continue;

                tilesToCombineWith.Add(randomTile);
                _numberOfTilesToCombineWith--;
            }

            CreateTileGroup(tilesToCombineWith);
        }

        private void ExpandTileGroup() {
            
        }

        private bool AllNeighbouringTilesAreTaken => neighbouringTiles.TrueForAll(tile => tile.isTaken);

        private void CreateTileGroup(List<Tile> tilesToCombine) {
            var tileGroup = Instantiate(_puzzle.tileGroupPrefab, GetAveragePosition(), Quaternion.identity, _puzzle.transform);
            tileGroup.Init(tilesToCombine);
            
            foreach (var tile in tilesToCombine) tile.HandleTileGrouping(tileGroup);
            
            _puzzle.tileGroups.Add(tileGroup);

            Vector3 GetAveragePosition() {
                var averagePosition = Vector3.zero;
                foreach (var tile in tilesToCombine) {
                    averagePosition += tile.transform.position;
                }

                return averagePosition / tilesToCombine.Count;
            }
        }

        private void HandleTileGrouping(TileGroup tileGroup) {
            isTaken = true;
            tileGroupParent = tileGroup;
            transform.SetParent(tileGroup.transform);
            _numberOfTilesToCombineWith--;
        }
        

        private bool CannotCombine() => !(Random.Range(0f, 1f) < _combiningChance);
    }
}