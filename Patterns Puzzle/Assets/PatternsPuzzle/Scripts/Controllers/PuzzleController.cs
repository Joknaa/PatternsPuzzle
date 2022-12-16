﻿using System;
using PuzzleSystem;
using UnityEngine;

namespace GameControllers {
    public class PuzzleController : MonoBehaviour {
        public static PuzzleController Instance => instance ??= FindObjectOfType<PuzzleController>();
        private static PuzzleController instance;
        
        public Puzzle puzzlePrefab;
        public Puzzle CurrentPuzzle {
            get {
                GetCurrentPuzzle();
                return _currentPuzzle;
            }
        }
        private Puzzle _currentPuzzle;


        private void Start() {
            GetCurrentPuzzle();
            CurrentPuzzle.GenerateNewPuzzle();
        }

        public void GetCurrentPuzzle() {
            _currentPuzzle = FindObjectOfType<Puzzle>();
            if (_currentPuzzle == null) _currentPuzzle = Instantiate(puzzlePrefab);
        }

        
        private void OnDestroy() {
            instance = null;
        }
    }
}