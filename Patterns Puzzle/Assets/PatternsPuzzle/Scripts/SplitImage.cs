using System;
using UnityEngine;
using System.Collections.Generic;
using OknaaEXTENSIONS;

public enum SplitType {
    ByTileCount,
    ByTileSize
}

public class SplitImage : MonoBehaviour {
    public SpriteRenderer spritePrefab;

    public Texture2D inputImage;
    public SplitType splitType = SplitType.ByTileCount;

    [Header("Split by Tile Count")] public Vector2Int tileCount;
    [Header("Split by Tile Size")] public Vector2 tileDimensions;

    [Space] public List<GameObject> tiles;


    private Texture2D _inputImage;

    public void SplitIntoTextures() {
        _inputImage = inputImage.isReadable ? inputImage : inputImage.GetReadableCopy();
     
        if (splitType == SplitType.ByTileCount) {
            SplitByTileCount();
        }
        else {
            // SplitByTileSize(tileDimensions);
        }
    }

    private static Vector2 CalculateTileDimensions(Texture image, Vector2 tileCount_) => new Vector2(image.width / tileCount_.x, image.height / tileCount_.y);

    private void SplitByTileCount() {
        var newTileDimensions = CalculateTileDimensions(_inputImage, tileCount);
        SplitByTileSize(newTileDimensions);
    }

    private void SplitByTileSize(Vector2 tileDimensions_) {
        var tileWidth = tileDimensions_.x;
        var tileHeight = tileDimensions_.y;

        var offsetWidth = _inputImage.width % tileWidth;
        var offsetHeight = _inputImage.height % tileHeight;

        bool hasPerfectWidth = _inputImage.width % tileWidth == 0;
        bool hasPerfectHeight = _inputImage.height % tileHeight == 0;

        var lastTileWidth = tileWidth;
        if (offsetWidth != 0) lastTileWidth = _inputImage.width - ((_inputImage.width / tileWidth) * tileWidth);

        var lastTileHeight = tileHeight;
        if (!hasPerfectHeight) lastTileHeight = _inputImage.height - ((_inputImage.height / tileHeight) * tileHeight);

        float tilesCount_Width = _inputImage.width / tileWidth + (hasPerfectWidth ? 0 : 1);
        float tilesCount_Height = _inputImage.height / tileHeight + (hasPerfectHeight ? 0 : 1);


        for (int i = 0; i < tilesCount_Width; i++) {
            for (int j = 0; j < tilesCount_Height; j++) {
                var currentTileWidth = Math.Abs(i - (tilesCount_Width - 1)) < 0.01f ? lastTileWidth : tileWidth;
                var currentTileHeight = Math.Abs(j - (tilesCount_Height - 1)) < 0.01f ? lastTileHeight : tileHeight;
                if (currentTileWidth == 0 || currentTileHeight == 0) continue;
                
                var tileTexture = new Texture2D(Mathf.RoundToInt(currentTileWidth), Mathf.RoundToInt(currentTileHeight));
                tileTexture.SetPixels(_inputImage.GetPixels((int) (i * currentTileWidth), (int) (j * currentTileHeight), (int) currentTileWidth, (int) currentTileHeight));
                tileTexture.Apply();

                var tileRect = new Rect(0, 0, tileTexture.width, tileTexture.height);
                var tileSprite = Sprite.Create(tileTexture, tileRect, Vector2.one * 0.5f);

                var tileSize = tileSprite.bounds.size;

                var tileGameObject = Instantiate(spritePrefab, new Vector3(i * tileSize.x, j * tileSize.y, 0), Quaternion.identity, transform);
                tileGameObject.sprite = tileSprite;
                tileGameObject.name = $"Sprite {i} {j}";

                tiles.Add(tileGameObject.gameObject);
            }
        }
    }
    private void SplitByTileSize_WhileCheckingForPerfectDimensions(Vector2Int tileDimensions_) {
        int width = tileDimensions_.x;
        int height = tileDimensions_.y;

        int offsetWidth = _inputImage.width % width;
        int offsetHeight = _inputImage.height % height;

        bool hasPerfectWidth = _inputImage.width % width == 0;
        bool hasPerfectHeight = _inputImage.height % height == 0;

        int lastWidth = width;
        if (offsetWidth != 0) lastWidth = _inputImage.width - ((_inputImage.width / width) * width);

        int lastHeight = height;
        if (!hasPerfectHeight) lastHeight = _inputImage.height - ((_inputImage.height / height) * height);


        int widthPartsCount = _inputImage.width / width + (hasPerfectWidth ? 0 : 1);
        int heightPartsCount = _inputImage.height / height + (hasPerfectHeight ? 0 : 1);


        for (var i = 0; i < widthPartsCount; i++) {
            for (var j = 0; j < heightPartsCount; j++) {
                int tileWidth = i == widthPartsCount - 1 ? lastWidth : width;
                int tileHeight = j == heightPartsCount - 1 ? lastHeight : height;

                var tileTexture = new Texture2D(tileWidth, tileHeight);
                tileTexture.SetPixels(_inputImage.GetPixels(i * width, j * height, tileWidth, tileHeight));
                tileTexture.Apply();

                var tileRect = new Rect(0, 0, tileTexture.width, tileTexture.height);
                var tileSprite = Sprite.Create(tileTexture, tileRect, Vector2.one * 0.5f);

                var tileSize = tileSprite.bounds.size;

                var tileGameObject = Instantiate(spritePrefab, new Vector3(i * tileSize.x, j * tileSize.y, 0), Quaternion.identity, transform);
                tileGameObject.sprite = tileSprite;
                tileGameObject.name = $"Sprite {i} {j}";

                tiles.Add(tileGameObject.gameObject);
            }
        }
    }


    public void ClearTiles() {
        foreach (var tile in tiles) {
            DestroyImmediate(tile);
        }

        tiles.Clear();
    }

    private Texture2D DuplicateTexture(Texture source) {
        RenderTexture newRenderTexture = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);

        Graphics.Blit(source, newRenderTexture);
        RenderTexture previousRenderTexture = RenderTexture.active;
        RenderTexture.active = newRenderTexture;
        
        Texture2D readableTexture = new Texture2D(source.width, source.height);
        readableTexture.ReadPixels(new Rect(0, 0, newRenderTexture.width, newRenderTexture.height), 0, 0);
        readableTexture.Apply();
        RenderTexture.active = previousRenderTexture;
        RenderTexture.ReleaseTemporary(newRenderTexture);
        return readableTexture;
    }

    private Sprite[] SplitTextureToSprites(Texture2D source, Vector2Int tileSize) {
        var sourceWidth = source.width;
        var sourceHeight = source.height;

        var tileAmountX = Mathf.FloorToInt(sourceWidth / (float)tileSize.x);
        var tileAmountY = Mathf.FloorToInt(sourceHeight / (float)tileSize.y);

        var output = new Sprite[tileAmountX * tileAmountY];

        // starting at the top left tile
        for (var y = tileAmountY - 1; y >= 0; y--) {
            for (var x = 0; x < tileAmountX; x++) {
                var bottomLeftPixelX = x * tileSize.x;
                var bottomLeftPixelY = y * tileSize.y;

                var sprite = Sprite.Create(source, new Rect(bottomLeftPixelX, bottomLeftPixelY, tileSize.x, tileSize.y), Vector2.one * 0.5f);

                var spriteGameObject = Instantiate(spritePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                spriteGameObject.sprite = sprite;
                spriteGameObject.name = $"Sprite {x} {y}";

                output[x + y * tileAmountX] = sprite;
            }
        }

        return output;
    }
}