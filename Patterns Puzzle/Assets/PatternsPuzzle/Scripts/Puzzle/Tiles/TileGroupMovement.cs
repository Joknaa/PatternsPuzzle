using System;
using GameControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PuzzleSystem {
    public class TileGroupMovement : MonoBehaviour {
        [HideInInspector] public Tile OriginTile;
        [HideInInspector] public bool IsDragged;
        [HideInInspector] public bool IsSnapped;
        [HideInInspector] public bool IsInCorrectPlace;
     
        private void Update() {
            if (OriginTile.IsMatched) return;
            if (!IsDragged) return;

            HandleTileMovement();
        }

        private void HandleTileMovement() {
            var hits = CanvasController.Instance.CheckRaycast(OriginTile.GetComponent<RectTransform>().position);
            if (hits == null) return;
            foreach (var hit in hits) {
                var hitGameObject = hit.gameObject;
                
                if (hitGameObject.CompareTag("TileSlot")) {
                    OnHoverOverEmptySlot(hitGameObject.GetComponent<TileSlot>());
                    break;
                }
            }
            
            var tileShadows = OriginTile.Puzzle.tileShadows;
            foreach (var tileShadow in tileShadows) {
                // tileShadow.SetHoveredOver(false);
            }
        }

        public void SnapTileToCorrectPosition() => OriginTile.SetTileAsMatched();


        private void OnHoverOverEmptySlot(TileSlot slot) {
            IsInCorrectPlace = OriginTile.TileMatchesSlot(slot) ;
        }
    }
}