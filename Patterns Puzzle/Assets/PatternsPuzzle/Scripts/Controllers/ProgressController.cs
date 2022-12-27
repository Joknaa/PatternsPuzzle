using System;
using PuzzleSystem;
using UnityEngine;

namespace GameControllers {
    public class ProgressController : MonoBehaviour {
        public static ProgressController Instance => instance ??= FindObjectOfType<ProgressController>();
        private static ProgressController instance;

        public int Progress { get; private set; } = 0;

        private int _totalProgress;


        private void Awake() {
            PuzzleController.OnPuzzleGenerationComplete += SetupProgressController;
            Tile.OnTilePlacedInCorrectSlot += AddProgress;
        }
        
        private void SetupProgressController() {
            _totalProgress = PuzzleController.Instance.CurrentPuzzle.GetTileCount();
            Progress = 0;
        }

        
        public void AddProgress(int progress) {
            Progress += progress;
            UIController.Instance.UpdateProgress((int)((float)Progress * 100 / _totalProgress));
        }

        
        private void OnDestroy() {
            PuzzleController.OnPuzzleGenerationComplete -= SetupProgressController;
            Tile.OnTilePlacedInCorrectSlot -= AddProgress;
        }
    }
}