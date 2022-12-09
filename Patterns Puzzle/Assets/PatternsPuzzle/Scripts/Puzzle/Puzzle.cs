using System.Collections.Generic;
using System.IO;
using OknaaEXTENSIONS;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSystem {
    public class Puzzle : MonoBehaviour {
        public static int TileGroupsCount = 0;
        [Header("Prefabs and Settings: ")] 
        public SpriteRenderer originalImagePrefab;
        public Tile tilePrefab;
        public TileGroup tileGroupPrefab;
        public Texture2D _inputImage;
        public Vector2Int _tileCount;
        [Range(0.0f, 1.0f)] [SerializeField] public float combiningChance;
        [Range(0, 10)] [SerializeField] public int numberOfTilesToCombine;

        [Header("Tiles and TileGroups: ")] public List<Tile> tiles;
        public List<TileGroup> tileGroups;

        private SpriteRenderer _originalSpriteInstance;
        private bool _isPuzzleGenerated;


        public void GeneratePuzzle() {
            if (_isPuzzleGenerated) return;
            _isPuzzleGenerated = true;
            SplitImageIntoTiles();
        }
        
        
        private void SplitImageIntoTiles() {
            if (CannotSplitImage()) return;
            
            InstantiateOriginalImage();
            ImageSplitter.Instance.Init(this);
            ImageSplitter.Instance.SplitImageIntoTiles();
            // CameraController.Instance.SetCameraTarget(_originalSpriteInstance);

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
            _originalSpriteInstance = Instantiate(originalImagePrefab, transform);
            originalImagePrefab.name = "Original Image";
            _originalSpriteInstance.sprite = imageSprite;
            _originalSpriteInstance.sortingOrder = -1;
            _originalSpriteInstance.transform.SetParent(transform);
            var color = _originalSpriteInstance.color;
            color.a = 0.5f;
            _originalSpriteInstance.color = color;
            
            _originalSpriteInstance.GetComponent<RectTransform>().MatchOther(transform.parent.GetComponent<RectTransform>());
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


#if UNITY_EDITOR

        public void SaveLevel() {
            var seed = Random.Range(0, 100000);

            if (!Directory.Exists("Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed)) {
                AssetDatabase.CreateFolder("Assets/PatternsPuzzle/Prefabs/Puzzles", "Puzzle_" + seed);
                AssetDatabase.CreateFolder("Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed, "Tiles");
                AssetDatabase.CreateFolder("Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed, "Sprites");
            }


            string directory = "Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed;
            string localPath = "Assets/PatternsPuzzle/Prefabs/Puzzles/Puzzle_" + seed + "/Puzzle.prefab";

            SaveTileSprites(directory);

            PrefabUtility.SaveAsPrefabAsset(gameObject, localPath, out var prefabSuccess);
            Debug.Log(prefabSuccess ? "Prefab was saved successfully" : "Prefab was not saved");
        }

        private void SaveTileSprites(string PuzzleDirectory) {
            foreach (var tile in tiles) {
                var tileSpritePath = PuzzleDirectory + "/Sprites/" + tile.name + ".png";
                File.WriteAllBytes(tileSpritePath, tile._spriteRenderer.sprite.texture.EncodeToJPG());
                //tile._spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(tileSpritePath);

                PrefabUtility.SaveAsPrefabAsset(tile.gameObject, PuzzleDirectory + "/Tiles/" + tile.name + ".prefab", out var tileSuccess);
                Debug.Log(tileSuccess ? "Tile was saved successfully" : "Tile was not saved");
            }

            AssetDatabase.Refresh();
        }

#endif

        public void ClearTiles() {
            _isPuzzleGenerated = false;
            tiles.Clear();
            tileGroups.Clear();

            var children = gameObject.GetDirectChildren();
            foreach (var child in children) {
                DestroyImmediate(child.gameObject);
            }
        }
        
    }
}