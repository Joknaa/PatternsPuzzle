using System.Collections.Generic;
using OknaaEXTENSIONS;
using UnityEngine;

namespace PatternsPuzzle.Scripts.Tile {
    public class Puzzle : MonoBehaviour {
        public static int TileGroupsCount = 0;
        
        public Texture2D _inputImage; 
        public Vector2Int _tileCount;
        [Range(0.0f, 1.0f)] [SerializeField] public float combiningChance;
        public List<Tile> tiles;
        public List<GameObject> tileGroups;
        
        private SpriteRenderer _originalSpriteInstance;
        

        public void SplitImageIntoTiles() {
            if (CannotSplitImage()) return;
            InstantiateOriginalImage();
            ImageSplitter.Instance.Init(this);
            ImageSplitter.Instance.SplitImageIntoTiles();
            CameraController.Instance.SetCameraTarget(_originalSpriteInstance);
            
            SetUpTileNeighbours();
            GenerateTileGroups();
        }

        private void SetUpTileNeighbours() {
            foreach (var tile in tiles) tile.SetUpNeighbours();
        }
        
        private void GenerateTileGroups() {
            foreach (var tile in tiles) tile.CombineTilesIntoRandomGroups();
        }
        
        private void InstantiateOriginalImage() {
            var imageRect = new Rect(0, 0, _inputImage.width, _inputImage.height);
            var imageSprite = Sprite.Create(_inputImage, imageRect, Vector2.one * 0.5f);
            _originalSpriteInstance = new GameObject("Original Sprite").AddComponent<SpriteRenderer>();
            _originalSpriteInstance.sprite = imageSprite;
            var color = _originalSpriteInstance.color;
            color.a = 0.5f;
            _originalSpriteInstance.color = color;
            _originalSpriteInstance.sortingOrder = -1;
            _originalSpriteInstance.transform.parent = transform;
        }
        
        public Tile GetTileByIndex(int x, int y, out Tile outputTile) {
            int index = TileCoordinates2Index(x, y);
            if (TileIndexIsOutsideArrayBounds() || TileCoordinatesAreOutsideGrid()) {
                outputTile = null;
                return outputTile;
            }
            
            outputTile = tiles[index];
            return outputTile;

            bool TileIndexIsOutsideArrayBounds() => index < 0 || index >= tiles.Count;
            bool TileCoordinatesAreOutsideGrid() => x < 0 || x >= _tileCount.x || y < 0 || y >= _tileCount.y;
        } 
        
        private int TileCoordinates2Index(int x, int y) => x * _tileCount.y + y;

        private bool CannotSplitImage() {
            if (_inputImage == null) {
                Debug.LogError("Input image is null");
                return true;
            }
            if (_tileCount.x <= 0 || _tileCount.y <= 0) {
                Debug.LogError("Tile count is invalid");
                return true;
            }
            return false;
        }
        
        public void ClearTiles() {
            tiles.Clear();
            tileGroups.Clear();
            
            var children = gameObject.GetDirectChildren();
            foreach (var child in children) {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}