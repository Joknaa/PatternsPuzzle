using System;
using PuzzleSystem;
using UnityEngine;

namespace GameControllers {
    public class ProgressController : MonoBehaviour {
        public static ProgressController Instance => instance ??= FindObjectOfType<ProgressController>();
        private static ProgressController instance;

        public float Progress { get; private set; } = 0.0f;

        private int _totalProgress;


        private void Awake() {
            PuzzleController.OnPuzzleGenerationComplete += SetupProgressController;
        }
        
        private void SetupProgressController() {
            _totalProgress = PuzzleController.Instance.CurrentPuzzle.GetTileCount();
            Progress = 0.0f;
        }

        
        public void AddProgress(int progress) {
            var addition = (float) progress / _totalProgress;
            Progress += addition;
            UIController.Instance.UpdateProgress(Progress);
        }

        
        private void OnDestroy() {
            PuzzleController.OnPuzzleGenerationComplete -= SetupProgressController;
        }
    }
}