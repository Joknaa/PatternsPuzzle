using System;
using System.Collections.Generic;
using System.IO;
using GameControllers;
using OknaaEXTENSIONS;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSystem {
    public class Puzzle : MonoBehaviour {
        public static int TileGroupsCount = 0;
        [Header("Containers: ")] 
        public Transform tilesContainer;
        public Transform tileShadowsContainer;

        [Header("Prefabs: ")] 
        public Texture2D _inputImage;
        public Image originalImagePrefab;
        public Image originalShadowImagePrefab;
        public Tile tilePrefab;
        public TileGroup tileGroupPrefab;
        public Transform tileShadowPrefab;

        [Header("Settings: ")]
        public Vector2Int _tileCount;
        [Range(0.0f, 1.0f)] [SerializeField] public float combiningChance;
        [Range(0, 4)] [SerializeField] public int numberOfTilesToCombine;

        [Header("Output Lists: ")] 
        public List<Tile> tiles;
        public List<TileSlot> tileShadows;
        public List<TileGroup> tileGroups;
        
        [HideInInspector] public Vector2 TileDimensions;
        public Image OriginalImageInstance => _originalImageInstance;
        private Image _originalImageInstance;
        
        public RectTransform OriginalImageRectTransform => originalImageRectTransform;
        private RectTransform originalImageRectTransform;

        
        private bool _isPuzzleGenerated;
        private GameObject _puzzleContainer;

        
        #region Puzzle Generation

        public void GenerateNewPuzzle() {
            if (_isPuzzleGenerated) return;
            ClearTiles();
            StartPuzzleGeneration();
            _isPuzzleGenerated = true;
        }
        

        private void StartPuzzleGeneration() {
            if (CannotSplitImage()) return;

            InstantiateOriginalImage();
            GeneratePuzzle();
            SetUpTileNeighbours();

            GenerateTileGroups();
            CanvasController.Instance.UpdateContentSpacing();
        }
        
        private void InstantiateOriginalImage() {
            var imageSprite = Sprite.Create(_inputImage, _inputImage.Rect(), Vector2.one * 0.5f);
            _originalImageInstance = Instantiate(originalImagePrefab, transform);
            _originalImageInstance.name = "Original Image";
            _originalImageInstance.sprite = imageSprite;

            var color = _originalImageInstance.color;
            color.a = 0.2f;
            _originalImageInstance.color = color;
            originalImageRectTransform = _originalImageInstance.rectTransform;
            originalImageRectTransform.MatchOther(transform.parent.GetComponent<RectTransform>());
            
        }
        
        private void SetUpTileNeighbours() {
            foreach (var tile in tiles) tile.SetUpNeighbours();
        }

        private void GenerateTileGroups() {
            foreach (var tile in tiles) tile.CombineTileWithRandomNeighbors();
            tilesContainer.ShuffleChildren();
        }
        
        private void GeneratePuzzle() {
            PuzzleGenerator.Instance.GeneratePuzzle(this);
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

        public int TileCoordinates2Index(int x, int y) => x * _tileCount.y + y;
        public int TileCoordinates2Index(Vector2Int coordinates) => coordinates.x * _tileCount.y + coordinates.y;
        public Vector2Int TileIndex2Coordinates(int index) => new Vector2Int(index / _tileCount.y, index % _tileCount.y);

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

        #endregion

        #region Puzzle Saving

//
// #if UNITY_EDITOR
//
//         public void SaveLevel() {
//             var seed = Random.Range(0, 100000);
//
//             if (!Directory.Exists("Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed)) {
//                 AssetDatabase.CreateFolder("Assets/PatternsPuzzle/Prefabs/Puzzles", "Puzzle_" + seed);
//                 AssetDatabase.CreateFolder("Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed, "Tiles");
//                 AssetDatabase.CreateFolder("Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed, "Sprites");
//             }
//
//
//             string directory = "Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed;
//             string localPath = "Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed + "/Puzzle.prefab";
//
//             SaveTileSprites(directory);
//
//             PrefabUtility.SaveAsPrefabAsset(gameObject, localPath, out var prefabSuccess);
//             Debug.Log(prefabSuccess ? "Prefab was saved successfully" : "Prefab was not saved");
//         }
//
//         private void SaveTileSprites(string PuzzleDirectory) {
//             foreach (var tile in tiles) {
//                 var tileSpritePath = PuzzleDirectory + "/Sprites/" + tile.name + ".png";
//                 File.WriteAllBytes(tileSpritePath, tile.spriteRenderer.sprite.texture.EncodeToJPG());
//                 //tile._spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(tileSpritePath);
//
//                 PrefabUtility.SaveAsPrefabAsset(tile.gameObject, PuzzleDirectory + "/Tiles/" + tile.name + ".prefab", out var tileSuccess);
//                 Debug.Log(tileSuccess ? "Tile was saved successfully" : "Tile was not saved");
//             }
//
//             AssetDatabase.Refresh();
//         }
//
// #endif

        #endregion
        
        
        
        public void ClearTiles() {
            _isPuzzleGenerated = false;
            tiles.Clear();
            tileGroups.Clear();
            tileShadows.Clear();

            if (_originalImageInstance != null) DestroyImmediate(_originalImageInstance.gameObject);
            
            var children = new List<GameObject>();
            children.AddRange(gameObject.gameObject.GetDirectChildren());
            children.AddRange(tilesContainer.gameObject.GetDirectChildren());
            
            foreach (var child in children) {
                DestroyImmediate(child.gameObject);
            }
            
            PuzzleGenerator.Instance.Clear();
            
        }
        
        public int GetTileCount() => tiles.Count;
        
    }
}