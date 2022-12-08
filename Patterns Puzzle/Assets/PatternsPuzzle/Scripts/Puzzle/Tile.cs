using System.Collections.Generic;
using GameControllers;
using OknaaEXTENSIONS;
using UnityEngine;
using UnityEngine.Assertions;

namespace PatternsPuzzle.Scripts.Tile {
    public class Tile : MonoBehaviour {
        public int X;
        public int Y;
        public int ID;
        public bool isTaken;
        public GameObject tileGroupParent;

        private Puzzle _puzzle;
        private float _combiningChance;
        private readonly List<Tile> neighbouringTiles = new List<Tile>();
        
        private bool _isMatched = false;

        public void Init(Puzzle puzzle, int id, int x, int y) {
            _puzzle = puzzle;
            ID = id;
            X = x;
            Y = y;
            _combiningChance = _puzzle.combiningChance;
            
        }


        public void SetUpNeighbours() {
            if (_puzzle.GetTileByIndex(X, Y + 1, out var tileAbove)) neighbouringTiles.Add(tileAbove);
            if (_puzzle.GetTileByIndex(X, Y - 1, out var tileBelow)) neighbouringTiles.Add(tileBelow);
            if (_puzzle.GetTileByIndex(X - 1, Y, out var tileLeft)) neighbouringTiles.Add(tileLeft);
            if (_puzzle.GetTileByIndex(X + 1, Y, out var tileRight)) neighbouringTiles.Add(tileRight);
   }
        
        
        public void CombineTilesIntoRandomGroups() {
            if (isTaken) return;
            if (CannotCombine()) return;

            Tile randomTile;
            List<Tile> tilesToCombineWith = new List<Tile>(){this};
            var numberOfTilesToBeCombined = 2;
            while (numberOfTilesToBeCombined > 0) {
                if (AllNeighbouringTilesAreTaken) break;
                
                randomTile = neighbouringTiles.Random();
                if (randomTile.isTaken) continue;
                
                tilesToCombineWith.Add(randomTile);
                numberOfTilesToBeCombined--;
            }
            CreateTileGroup(tilesToCombineWith);
        }

        private bool AllNeighbouringTilesAreTaken => neighbouringTiles.TrueForAll(tile => tile.isTaken);

        private void CreateTileGroup(List<Tile> tiles) {
            
            var tileGroup = new GameObject("TileGroup " + Puzzle.TileGroupsCount++) {
                transform = {
                    position = GetAveragePosition(),
                    rotation = Quaternion.identity,
                    localScale = Vector3.one,
                    parent = _puzzle.transform
                }
            };

            foreach (var tile in tiles) {
                tile.isTaken = true;
                tile.SetUpTileGroupParent(tileGroup);
            }

            _puzzle.tileGroups.Add(tileGroup);
            
            Vector3 GetAveragePosition() {
                var averagePosition = Vector3.zero;
                foreach (var tile in tiles) {
                    averagePosition += tile.transform.position;
                }
                return averagePosition / tiles.Count;
            } 
        }
        
        
        private void SetUpTileGroupParent(GameObject tileGroup) {
            tileGroupParent = tileGroup;
            transform.SetParent(tileGroup.transform);
        }
        
        private bool CannotCombine() => !(Random.Range(0f, 1f) < _combiningChance);
    }
}