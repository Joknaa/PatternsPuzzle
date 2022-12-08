using UnityEngine;
using System.Collections.Generic;
using OknaaEXTENSIONS;
using PatternsPuzzle.Scripts.Tile;

public class ImageSplitter : MonoBehaviour {
    public static ImageSplitter Instance => instance ??= FindObjectOfType<ImageSplitter>();
    private static ImageSplitter instance;
    
    [SerializeField] private SpriteRenderer spritePrefab; 
    
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
                var tileGameObject = CreateTile(i, j);
                _puzzle.tiles.Add(tileGameObject);
            }
        }
    }

    private Tile CreateTile(int i, int j) {
        var tileTexture = CopyTilePixelsToNewTexture2D();
        var tileSprite = GetTileSpriteFromTexture();
        var tileInstance = InstantiateTilePrefab();
        return ConfigureTileInstance();

        Texture2D CopyTilePixelsToNewTexture2D() {
            tileTexture = new Texture2D((int)_tileWidth, (int)_tileHeight);
            tileTexture.SetPixels(_inputImage.GetPixels(i * (int)_tileWidth, j * (int)_tileHeight, (int)_tileWidth, (int)_tileHeight));
            tileTexture.Apply();
            return tileTexture;
        }

        Sprite GetTileSpriteFromTexture() {
            var tileRect = new Rect(0, 0, tileTexture.width, tileTexture.height);
            tileSprite = Sprite.Create(tileTexture, tileRect, Vector2.one * 0.5f);
            return tileSprite;
        }

        SpriteRenderer InstantiateTilePrefab() {
            var tileSize = tileSprite.bounds.size;
            var imageSize = new Vector2(tileSize.x * _tileCount.x, tileSize.y * _tileCount.y);

            var tilePosition_X = (tileSize.x * i) - (imageSize.x * 0.5f) + (tileSize.x * 0.5f);
            var tilePosition_Y = (tileSize.y * j) - (imageSize.y * 0.5f) + (tileSize.y * 0.5f);
            var tilePosition = new Vector3(tilePosition_X, tilePosition_Y, 0);

            return Instantiate(spritePrefab, tilePosition, Quaternion.identity, transform);
        }
        
        Tile ConfigureTileInstance() {
            tileInstance.name = $"Sprite {i} {j}";
            tileInstance.sprite = tileSprite;
            tileInstance.sortingOrder = 1;
            var tileScript = tileInstance.GetComponent<Tile>();
            tileScript.Init(_puzzle, i * _tileCount.y + j, i, j);
            return tileScript;
        }
    }

     

    
}