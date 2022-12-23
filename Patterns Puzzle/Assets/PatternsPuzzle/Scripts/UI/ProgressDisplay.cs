using System;
using GameControllers;
using TMPro;
using UnityEngine;

namespace UI {
    /// <summary>
    /// Handles the UI element for displaying the current progress of the player.
    /// </summary>
    public class ProgressDisplay : MonoBehaviour {
        private TMP_Text _progressText;


        private void Awake() {
            _progressText = GetComponent<TMP_Text>();
            UIController.OnProgressChanged += UpdateProgress;
            _progressText.text = "Progress: 0%";
        }

        private void UpdateProgress(int progress) {
            _progressText.text = $"Progress: {progress}%";
        }

        private void OnDestroy() {
            UIController.OnProgressChanged -= UpdateProgress;
        }
    }
}