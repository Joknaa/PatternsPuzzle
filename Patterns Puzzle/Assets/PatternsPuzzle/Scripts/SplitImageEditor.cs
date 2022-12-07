using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SplitImage))]
public class LevelGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        SplitImage splitImage = (SplitImage)target;
        // if (GUILayout.Button("Split Image (Sprites)")) splitImage.SplitIntoSprites();
        if (GUILayout.Button("Split Image")) splitImage.SplitIntoTextures();
        if (GUILayout.Button("Clear Tiles")) splitImage.ClearTiles();
    }
}