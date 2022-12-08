using UnityEngine;
using System.Collections.Generic;
using PatternsPuzzle.Scripts.Tile;
using UnityEditor;

[CustomEditor(typeof(Puzzle))]
public class PuzzleEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Puzzle puzzle = (Puzzle)target;
        // if (GUILayout.Button("Split Image (Sprites)")) splitImage.SplitIntoSprites();
        if (GUILayout.Button("Split Image")) puzzle.SplitImageIntoTiles();
        if (GUILayout.Button("Clear Tiles")) puzzle.ClearTiles();
    }
}