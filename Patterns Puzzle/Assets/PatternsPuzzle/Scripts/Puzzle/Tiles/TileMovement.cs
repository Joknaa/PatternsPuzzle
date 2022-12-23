using System;
using GameControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PuzzleSystem {
    public class TileMovement : MonoBehaviour {
        [HideInInspector] public Tile OriginTile;
        [HideInInspector] public bool IsDragged;
        [HideInInspector] public bool IsSnapped;
     
        private 

        void Update() {
            if (OriginTile.IsMatched) return;
            if (!IsDragged) return;

            HandleTileMovement();
        }

        private void HandleTileMovement() {
            var hits = CanvasController.Instance.CheckRaycast(OriginTile.GetComponent<RectTransform>().position);
            if (hits == null) return;
            foreach (var hit in hits) {
                var hitGameObject = hit.gameObject;
                if (hitGameObject.CompareTag("TilesInventory")) {
                    break;
                }
                
                if (hitGameObject.CompareTag("TileSlot")) {
                    var shadow = hitGameObject.GetComponent<TileSlot>();
                    OnHoverOverEmptySlot(shadow);
                    break;
                }

                IsSnapped = false;
            }
        }

        private void OnHoverOverEmptySlot(TileSlot slot) {
            IsSnapped = true;
            transform.position = slot.RectTransform.position;
            OriginTile.CheckIfTileMatchesSlot(slot);
        }
        
        
        
    }
}