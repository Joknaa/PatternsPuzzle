using System.Collections.Generic;
using PuzzleSystem;
using UnityEngine;

namespace GameControllers {
    public class TileHighlightController : MonoBehaviour {
        public static TileHighlightController Instance => instance ??= FindObjectOfType<TileHighlightController>();
        private static TileHighlightController instance;

        private readonly List<TileShadow> _highlightedTileShadows = new List<TileShadow>();


        public void SetHighlight(TileShadow tileShadow, bool doHighlight) {
            var isHighlighted = _highlightedTileShadows.Contains(tileShadow);
            if (doHighlight && !isHighlighted) {
                _highlightedTileShadows.Add(tileShadow);
                return;
            }

            if (!doHighlight && isHighlighted) {
                _highlightedTileShadows.Remove(tileShadow);
            }
        }


        public void AddHighlightedTile(TileShadow tileShadow) {
            _highlightedTileShadows.Add(tileShadow);
        }

        public void RemoveHighlightedTile(TileShadow tileShadow) {
            _highlightedTileShadows.Remove(tileShadow);
        }

        public void ClearHighlightedTiles() {
            foreach (var tileShadow in _highlightedTileShadows) {
                tileShadow.ActivateHighlight(false);
            }

            _highlightedTileShadows.Clear();
        }
    }
}