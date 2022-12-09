using UnityEngine;
using System.Collections.Generic;
using OknaaEXTENSIONS;
using PuzzleSystem;
using Unity.VisualScripting;

[RequireComponent(typeof(Puzzle))]
public class ImageSplitter : MonoBehaviour {
    public static ImageSplitter Instance => instance ??= FindObjectOfType<ImageSplitter>();
    private static ImageSplitter instance;

    private Tile _tilePrefab;
    private Vector2Int _tileCount;
    private Texture2D _inputImage;
    private Transform _tilesContainer;
    private Puzzle _puzzle;
    private float _imageWidth;
    private float _imageHeight;
    private float _tileWidth;
    private float _tileHeight;

    private void Init(Puzzle puzzle) {
        _puzzle = puzzle;
        _inputImage = _puzzle._inputImage;
        _tileCount = _puzzle._tileCount;
        _tilePrefab = _puzzle.tilePrefab;
        _tilesContainer = _puzzle.tilesContainer;
    }

    public void SplitImageIntoTiles(Puzzle puzzle) {
        Init(puzzle);
        _puzzle = GetComponent<Puzzle>();
        _inputImage = _inputImage.isReadable ? _inputImage : _inputImage.GetReadableCopy();
        SplitImage();
    }


    private void SplitImage() {
        int tilesCount_Width = _tileCount.x;
        int tilesCount_Height = _tileCount.y;

        _imageWidth = _inputImage.width;
        _imageHeight = _inputImage.height;

        _tileWidth = _imageWidth / tilesCount_Width;
        _tileHeight = _imageHeight / tilesCount_Height;

        for (int i = 0; i < tilesCount_Width; i++) {
            for (int j = 0; j < tilesCount_Height; j++) {
                _puzzle.tiles.Add(CreateTile(i, j));
            }
        }
    }

    private Tile CreateTile(int i, int j) {
        var tileTexture = CopyTilePixelsToNewTexture2D();
        var tileSprite = GetTileSpriteFromTexture();
        return InstantiateTilePrefab();

        Texture2D CopyTilePixelsToNewTexture2D() {
            Vector2Int tileDimensions = new Vector2Int((int)_tileWidth, (int)_tileHeight);
            tileTexture = new Texture2D(tileDimensions.x, tileDimensions.y);
            tileTexture.SetPixels(_inputImage.GetPixels(i * tileDimensions.x, j * tileDimensions.y, tileDimensions.x, tileDimensions.y));
            tileTexture.Apply();
            return tileTexture;
        }

        Sprite GetTileSpriteFromTexture() {
            var tileRect = new Rect(0, 0, tileTexture.width, tileTexture.height);
            return Sprite.Create(tileTexture, tileRect, Vector2.one * 0.5f);
        }

        Tile InstantiateTilePrefab() {
            var tileCenterPosition = CalculateTheCenterPositionOfTheTile();
            
            var tileInstance = Instantiate(_tilePrefab, tileCenterPosition, Quaternion.identity, _tilesContainer);
            // var tileInstance = Instantiate(_tilePrefab, _puzzle.tilesContainer);

            tileInstance.Init(_puzzle, i, j, tileSprite);
            return tileInstance;
        }

        Vector3 CalculateTheCenterPositionOfTheTile() {
            var rect = _puzzle.originalImageRectTransform.rect;
            var imageSize = new Vector2(rect.width, rect.height);
            
            var tileSize = new Vector2(imageSize.x / (_tileCount.x * 2), imageSize.y / (_tileCount.y * 2));
            var imageMinPosition = isZero ? Vector2.zero : new Vector2(rect.xMin, rect.yMin);
            
            var tileCenterPosition = new Vector3(
                imageMinPosition.x + (tileSize.x * i) -  (tileSize.x * 0.5f),
                imageMinPosition.y + (tileSize.y * j) -  (tileSize.y * 0.5f),
                0);
            return tileCenterPosition;
        }
    }

    public bool isZero;

    private void OnDestroy() {
        instance = null;
    }
}