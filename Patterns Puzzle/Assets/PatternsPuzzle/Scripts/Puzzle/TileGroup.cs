using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSystem {
    public class TileGroup : MonoBehaviour {
        public List<Tile> tiles = new List<Tile>();
        public int maxTiles = 5;

        
        
        public void Init(List<Tile> groupedTiles) {
            tiles = groupedTiles;
        }
    }
}