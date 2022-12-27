using System;
using System.Collections.Generic;
using OknaaEXTENSIONS;
using PuzzleSystem;
using UnityEngine;

namespace GameControllers {
    public class SaveController : MonoBehaviour {
        public static SaveController Instance => instance ??= FindObjectOfType<SaveController>();
        private static SaveController instance;


        public void SavePuzzle() {
            var tileGroupsTransforms = new List<TileGroupAndTransform>();
            var currentPuzzle = PuzzleController.Instance.CurrentPuzzle;
            
            print("Saving Puzzle " + currentPuzzle.puzzleName);
            var tileGroupsList = currentPuzzle.tileGroups;

            foreach (var tileGroup in tileGroupsList) {
                tileGroupsTransforms.Add(new TileGroupAndTransform(tileGroup, tileGroup.transform));
            }
            
            
            
            PlayerPrefs.SetString(currentPuzzle.name, JsonUtility.ToJson(tileGroupsTransforms));
            // Save(currentPuzzle.name, tileGroupsTransforms);
        }

        public void LoadPuzzle(Puzzle puzzle) {
            var tileGroupsTransforms = Load<List<TileGroupAndTransform>>(puzzle.name);

            if (tileGroupsTransforms == null || tileGroupsTransforms.Count == 0) {
                // Debug.LogError("Puzzle save file not found. Loading the puzzle anew.");
                return;
            }

            LoadTilesPositions(puzzle, tileGroupsTransforms);
        }

        private void LoadTilesPositions(Puzzle puzzle, List<TileGroupAndTransform> tileGroupsTransforms) {
            var tileGroupsList = puzzle.tileGroups;

            foreach (var tileGroup in tileGroupsList) {
                var tileGroupTransform = tileGroupsTransforms.Find(t => t.TileGroup == tileGroup);
                tileGroup.transform.SetTransform(tileGroupTransform.Transform);
            }
        }

        private void Save(string Key, object value) => PlayerPrefs.SetString(Key, JsonUtility.ToJson(value));
        private T Load<T>(string Key) => JsonUtility.FromJson<T>(PlayerPrefs.GetString(Key));
    }


    [Serializable]
    public class TileGroupAndTransform {
        public TileGroup TileGroup;
        public Transform Transform;


        public TileGroupAndTransform(TileGroup tileGroup, Transform transform) {
            TileGroup = tileGroup;
            Transform = transform;
        }
    }
}