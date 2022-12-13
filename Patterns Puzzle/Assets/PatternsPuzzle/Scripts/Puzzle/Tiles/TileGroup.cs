using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSystem {
    public class TileGroup : MonoBehaviour {
        public List<Tile> tiles = new List<Tile>();
        
        private Puzzle _puzzle;

        public void Init(List<Tile> groupedTiles, Puzzle puzzle) {
            tiles = groupedTiles;
            _puzzle = puzzle;
            name = "TileGroup " + (tiles.Count);
            transform.SetParent(_puzzle.tilesContainer);
        }
    }
}