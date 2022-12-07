using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(ImageSplitter))]
public class LevelGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        ImageSplitter imageSplitter = (ImageSplitter)target;
        // if (GUILayout.Button("Split Image (Sprites)")) splitImage.SplitIntoSprites();
        if (GUILayout.Button("Split Image")) imageSplitter.SplitIntoTextures();
        if (GUILayout.Button("Clear Tiles")) imageSplitter.ClearTiles();
    }
}