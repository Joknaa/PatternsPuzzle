using System;
using PuzzleSystem;
using UnityEngine;

namespace GameControllers {
    public class ProgressController : MonoBehaviour {
        public static ProgressController Instance => instance ??= FindObjectOfType<ProgressController>();
        private static ProgressController instance;

        private int Progress {
            get => _progress;
            set {
                _progress = value;
                UIController.Instance.UpdateProgress((int)((float)_progress * 100 / _totalProgress));
            }
        }
        private int _progress;

        private int _totalProgress;


        private void Awake() {
            PuzzleController.OnPuzzleGenerationComplete += SetupProgressController;
            Tile.OnTilePlacedInCorrectSlot += AddProgress;
        }
        
        private void SetupProgressController() {
            _totalProgress = PuzzleController.Instance.CurrentPuzzle.GetTileCount();
            Progress = 0;
        }


        private void AddProgress(int progress) => Progress += progress;
        public void Reset() => Progress = 0;


        private void OnDestroy() {
            PuzzleController.OnPuzzleGenerationComplete -= SetupProgressController;
            Tile.OnTilePlacedInCorrectSlot -= AddProgress;
        }

    }
}