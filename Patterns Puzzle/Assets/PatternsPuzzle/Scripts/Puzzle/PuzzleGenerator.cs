using OknaaEXTENSIONS;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSystem {
    public class PuzzleGenerator {
        public static PuzzleGenerator Instance => instance ??= new PuzzleGenerator();
        private static PuzzleGenerator instance;
        
        private Puzzle _puzzle;
        private Texture2D _originalImage;
        private Texture2D _originalShadowImage;
        private Vector2Int _tileCount;
        private Tile _tilePrefab;
        private Transform _tilesContainer;
        private bool _isInit;


        public void GeneratePuzzle(Puzzle puzzle) {
            if (!_isInit) Init(puzzle);

            GeneratePuzzleTiles();
        }
        
        
        private void Init(Puzzle puzzle) {
            _puzzle = puzzle;
            _originalImage = _puzzle._inputImage;
            _tileCount = _puzzle._tileCount;
            _tilePrefab = _puzzle.tilePrefab;
            _tilesContainer = _puzzle.tilesContainer;
            _isInit = true;
        }
        
        private void GeneratePuzzleTiles() {
            _originalImage.SplitTextureIntoSprites(_tileCount, out var sprites);

            Vector2Int coordinates;
            for (var index = 0; index < sprites.Count; index++) {
                _puzzle.tiles.Add(InstantiateTilePrefab(index));
            }

            Tile InstantiateTilePrefab(int index) {
                coordinates = _puzzle.TileIndex2Coordinates(index);
                var tileInstance = Object.Instantiate(_tilePrefab, _puzzle.transform);
                SetTileDimensionsRelativeToParentInCanvas(tileInstance, _puzzle.transform, coordinates, _tileCount);
                tileInstance.Init(_puzzle, coordinates, sprites[index]);
                return tileInstance;
            }
        }

        
        public void SetTileDimensionsRelativeToParentInCanvas(Object tile, Object parentInCanvas, Vector2Int tileCoordinates, Vector2Int tileCount) {
            var i = tileCoordinates.x;
            var j = tileCoordinates.y;
            
            var parentRect = parentInCanvas.GetComponent<RectTransform>().rect;
            var parentDimensions = new Vector2(parentRect.width, parentRect.height);
            var tileDimensions = new Vector2(parentDimensions.x / tileCount.x, parentDimensions.y / tileCount.y);
            _puzzle.TileDimensions = tileDimensions;
            
            var tileRectTransform = tile.GetComponent<RectTransform>();
            tileRectTransform.localScale = Vector3.one; 
            tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tileDimensions.x);
            tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tileDimensions.y);
            tileRectTransform.localPosition = new Vector3((i + 0.5f) * tileDimensions.x, (j + 0.5f) * tileDimensions.y, 0);
        }

        
        public void Clear() => _isInit = false;
    }
}