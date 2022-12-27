using System;
using System.Collections.Generic;
using PuzzleSystem;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers {
    public class PuzzleController : MonoBehaviour {
        public static PuzzleController Instance => instance ??= FindObjectOfType<PuzzleController>();
        private static PuzzleController instance;

        public static Action OnPuzzleGenerationComplete;
        public Puzzle puzzlePrefab;
        public Puzzle CurrentPuzzle {
            get {
                GetCurrentPuzzle();
                return _currentPuzzle;
            }
        }
        private Puzzle _currentPuzzle;

        public void GenerateNewPuzzle(string puzzleName, Texture2D inputImage) {
            GetCurrentPuzzle();
            CurrentPuzzle.GenerateNewPuzzle(puzzleName, inputImage);
            OnPuzzleGenerationComplete?.Invoke();
        }

        public void GetCurrentPuzzle() {
            if (_currentPuzzle == null) _currentPuzzle = FindObjectOfType<Puzzle>();
            if (_currentPuzzle == null) _currentPuzzle = Instantiate(puzzlePrefab);
        }
        
        private void OnDestroy() {
            instance = null;
        }
    }
}