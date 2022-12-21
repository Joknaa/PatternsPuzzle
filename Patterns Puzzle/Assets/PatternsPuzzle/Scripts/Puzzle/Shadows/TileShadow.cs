using UnityEngine;

namespace PuzzleSystem {
    public class TileShadow : MonoBehaviour {
        private Tile _tile;
        private CanvasGroup _canvasGroup;
        private Vector3 _startPosition;
        private int _startIndex;
        private int _id;
        private Puzzle _puzzle;
        public RectTransform RectTransform => _rectTransform;
        private RectTransform _rectTransform;

        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            ActivateHighlight(false);
        }

        public void Init(Puzzle puzzle, Vector2Int coordinates, Tile tile) {
            _tile = tile;
            name = $"TileShadow of {tile.name}";
            _puzzle = puzzle;
            _id = _puzzle.TileCoordinates2Index(coordinates);

            transform.SetParent(_puzzle.tileShadowsContainer);
            PuzzleGenerator.Instance.SetTileDimensionsRelativeToParentInCanvas(this, _puzzle.tileShadowsContainer, coordinates, _puzzle._tileCount);

            _puzzle.tileShadows.Add(this);
        }


        public void ActivateHighlight(bool active) {
            _canvasGroup.alpha = active ? 1f : 0;
        }
    }
}