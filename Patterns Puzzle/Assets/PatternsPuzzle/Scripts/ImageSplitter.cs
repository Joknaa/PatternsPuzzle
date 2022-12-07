using UnityEngine;
using System.Collections.Generic;
using OknaaEXTENSIONS;

public class ImageSplitter : MonoBehaviour {
    [SerializeField] private SpriteRenderer spritePrefab;
    [SerializeField] private Texture2D inputImage;
    [SerializeField] private Vector2Int tileCount;

    [Space] [SerializeField] private List<GameObject> tiles;
    
    private Texture2D _inputImage;
    private float _imageWidth;
    private float _imageHeight;
    private float _tileWidth;
    private float _tileHeight;

    public void SplitIntoTextures() {
        _inputImage = inputImage.isReadable ? inputImage : inputImage.GetReadableCopy();
        SplitImage();
        CameraController.Instance.SetCameraTarget(gameObject.transform);
    }
    
    private void SplitImage() {
        int tilesCount_Width = tileCount.x;
        int tilesCount_Height = tileCount.y;
        
        _imageWidth = _inputImage.width;
        _imageHeight = _inputImage.height;
        
        _tileWidth = (_imageWidth / (float)tilesCount_Width);
        _tileHeight = (_imageHeight / (float)tilesCount_Height);

        for (int i = 0; i < tilesCount_Width; i++) {
            for (int j = 0; j < tilesCount_Height; j++) {
                var tileGameObject = CreateTile(i, j);
                tiles.Add(tileGameObject);
            }
        }
    }

    private GameObject CreateTile(int i, int j) {
        var tileTexture = CopyTilePixelsToNewTexture2D();
        var tileSprite = GetTileSpriteFromTexture();
        var tileGameObject = InstantiateTilePrefab();
        return tileGameObject;

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

        GameObject InstantiateTilePrefab() {
            var tileSize = tileSprite.bounds.size;
            
            var imageSize = new Vector2( tileSize.x * tileCount.x, tileSize.y * tileCount.y);
            print("imageSize: " + imageSize);

            var tilePosition_X = (tileSize.x * i) - (imageSize.x * 0.5f) + (tileSize.x * 0.5f);
            var tilePosition_Y = (tileSize.y * j) - (imageSize.y * 0.5f) + (tileSize.y * 0.5f);
            var tilePosition = new Vector3(tilePosition_X, tilePosition_Y, 0);
            
            var tile = Instantiate(spritePrefab, tilePosition, Quaternion.identity, transform);
            tile.sprite = tileSprite;
            tile.name = $"Sprite {i} {j}";
            return tile.gameObject;
        }
    }

    public void ClearTiles() {
        foreach (var tile in tiles) {
            DestroyImmediate(tile);
        }

        tiles.Clear();
    }
}