using UnityEngine;
using System.Collections.Generic;
using PuzzleSystem;
using UnityEditor;

[CustomEditor(typeof(Puzzle))]
public class PuzzleEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Puzzle puzzle = (Puzzle)target;
        // if (GUILayout.Button("Split Image (Sprites)")) splitImage.SplitIntoSprites();
        if (GUILayout.Button("Generate Puzzle")) puzzle.GeneratePuzzle();
        // if (GUILayout.Button("Save Puzzle")) puzzle.SaveLevel();
        if (GUILayout.Button("Clear")) puzzle.ClearTiles();
    }
}

[CustomEditor(typeof(ImageSplitter))]
public class ImageSplitterEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        ImageSplitter imageSplitter = (ImageSplitter)target;
        // if (GUILayout.Button("Split Image (Sprites)")) splitImage.SplitIntoSprites();
        // if (GUILayout.Button("Save Textures")) imageSplitter.SaveTextures();
    }
}