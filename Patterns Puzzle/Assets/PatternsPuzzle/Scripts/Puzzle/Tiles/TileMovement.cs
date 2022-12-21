using GameControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PuzzleSystem {
    public class TileMovement : MonoBehaviour {
        public Tile OriginTile;
        public bool IsDragged;
        public bool IsSnapped;

        
        
        void Update() {
            if (!IsDragged) return;

            HandleTileMovement();
        }

        private void HandleTileMovement() {
            var hits = CanvasController.Instance.CheckRaycast(OriginTile.GetComponent<RectTransform>().position);
            if (hits == null) return;
            foreach (var hit in hits) {
                var hitGameObject = hit.gameObject;
                if (hitGameObject.CompareTag("TileShadow")) {
                    var shadow = hitGameObject.GetComponent<TileShadow>();
                    OnHoverOverEmptySlot(shadow);
                    break;
                }

                IsSnapped = false;
            }
        }

        private void OnHoverOverEmptySlot(TileShadow shadow) {
            IsSnapped = true;
            transform.position = shadow.RectTransform.position;
        }
        
        
        
    }
}