using System.Collections.Generic;
using PuzzleSystem;
using UnityEngine;

namespace GameControllers {
    public class TileHighlightController : MonoBehaviour {
        public static TileHighlightController Instance => instance ??= FindObjectOfType<TileHighlightController>();
        private static TileHighlightController instance;

        private readonly List<TileSlot> _highlightedTileShadows = new List<TileSlot>();


        public void SetHighlight(TileSlot tileSlot, bool doHighlight) {
            var isHighlighted = _highlightedTileShadows.Contains(tileSlot);
            if (doHighlight && !isHighlighted) {
                _highlightedTileShadows.Add(tileSlot);
                return;
            }

            if (!doHighlight && isHighlighted) {
                _highlightedTileShadows.Remove(tileSlot);
            }
        }


        public void AddHighlightedTile(TileSlot tileSlot) {
            _highlightedTileShadows.Add(tileSlot);
        }

        public void RemoveHighlightedTile(TileSlot tileSlot) {
            _highlightedTileShadows.Remove(tileSlot);
        }

        public void ClearHighlightedTiles() {
            foreach (var tileShadow in _highlightedTileShadows) {
                tileShadow.SetHoveredOver(false);
            }

            _highlightedTileShadows.Clear();
        }
    }
}