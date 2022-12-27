using System;
using GameControllers;
using UnityEngine;

namespace PuzzleSystem {
    public class TileShadowDetector : MonoBehaviour {
        public bool IsDragged;

        private Tile tile;
        private TileSlot _currentHoveredSlot;

        private void Awake() {
            tile = GetComponent<Tile>();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("TileSlot")) col.GetComponent<TileSlot>().SetHoveredOver(true);
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("TileSlot")) col.GetComponent<TileSlot>().SetHoveredOver(false);
        }
        // private void Update() {
        //     if (tile.IsMatched) return;
        //     if (!IsDragged) return;
        //
        //     HandleTileMovement();
        // }

        private void HandleTileMovement() {
            var hits = CanvasController.Instance.CheckRaycast(tile.GetComponent<RectTransform>().position);
            if (hits == null) return;
            foreach (var hit in hits) {
                var hitGameObject = hit.gameObject;
                
                if (hitGameObject.CompareTag("TileSlot")) {
                    OnHoverOverEmptySlot(hitGameObject.GetComponent<TileSlot>());
                    break;
                }
            }
        }

        private void OnHoverOverEmptySlot(TileSlot slot) {
            if (slot == _currentHoveredSlot) return;
            if (slot == null) return;
            
            if (_currentHoveredSlot != null) _currentHoveredSlot.SetHoveredOver(false);
            slot.SetHoveredOver(true);
            _currentHoveredSlot = slot;
        }
    }
}