using System;
using GameControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PuzzleSystem {
    public class TileShadow : MonoBehaviour {
        [SerializeField] private EventTrigger eventTrigger;

        private Tile _tile;
        private Image _highlightImage;
        private Vector3 _startPosition;
        private int _startIndex;
        private int _id;
        private Puzzle _puzzle;

        private void Awake() {
            _highlightImage = GetComponent<Image>();
            ActivateHighlight(false);
        }

        private void Start() {
            SetEventTrigger(EventTriggerType.PointerEnter, OnPointerEnter);
            SetEventTrigger(EventTriggerType.PointerExit, OnPointerExit);
        }
        
        public void Init(Puzzle puzzle, Vector2Int coordinates, Tile tile) {
            _tile = tile;
            name = $"TileShadow of {tile.name}";
            _puzzle = puzzle;
            _id = _puzzle.TileCoordinates2Index(coordinates);
            
            transform.SetParent(_puzzle.tileShadowsContainer);
            PuzzleGenerator.SetTileDimensionsRelativeToParentInCanvas(this, _puzzle.tileShadowsContainer, coordinates, _puzzle._tileCount);

            _puzzle.tileShadows.Add(this);
        }

        private void SetEventTrigger(EventTriggerType type, UnityAction<BaseEventData> action) {
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            eventTrigger.triggers.Add(entry);
        }

        public void OnPointerEnter(BaseEventData data) => ActivateHighlight(true);
        public void OnPointerExit(BaseEventData data) => ActivateHighlight(false);
        

        
        private void OnDrag(BaseEventData data) => transform.position = ((PointerEventData)data).position;

        private void OnPointerUp(BaseEventData data) {
            transform.position = _startPosition;
            transform.SetSiblingIndex(_startIndex);
            var tileContainer = PuzzleController.Instance._currentPuzzle.tilesContainer;
            var child0 = tileContainer.GetChild(0);
            var child1 = tileContainer.GetChild(1);
            child1.transform.SetAsFirstSibling();
        }
        
        private void ActivateHighlight(bool active) => _highlightImage.enabled = active;

       
    }
}