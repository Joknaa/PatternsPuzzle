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
    private Puzzle _puzzle;
    private float _imageWidth;
    private float _imageHeight;
    private float _tileWidth;
    private float _tileHeight;

    public void Init(Puzzle puzzle) {
        _puzzle = puzzle;
        _inputImage = _puzzle._inputImage;
        _tileCount = _puzzle._tileCount;
        _tilePrefab = _puzzle.tilePrefab;
    }

    public void SplitImageIntoTiles() {
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
            
            var tileInstance = Instantiate(_tilePrefab, tileCenterPosition, Quaternion.identity, transform);

            tileInstance.Init(_puzzle, i, j, tileSprite);
            return tileInstance;
        }

        Vector3 CalculateTheCenterPositionOfTheTile() {
            var tileSize = tileSprite.bounds.size;
            var imageSize = new Vector2(tileSize.x * _tileCount.x, tileSize.y * _tileCount.y);

            var tileCenterPosition = new Vector3(
                (tileSize.x * i) - (imageSize.x * 0.5f) + (tileSize.x * 0.5f),
                (tileSize.y * j) - (imageSize.y * 0.5f) + (tileSize.y * 0.5f),
                0);
            return tileCenterPosition;
        }
    }

    private void OnDestroy() {
        instance = null;
    }
}