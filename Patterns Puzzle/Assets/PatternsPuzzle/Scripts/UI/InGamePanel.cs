using System;
using GameControllers;
using UnityEngine;

namespace UI {
    /// <summary>
    /// Handles all matters related to the InGame UI (the UI that appears while the player is solving a puzzle), and redirects specific actions and tasks to the appropriate UI element script.  
    /// </summary>
    public class InGamePanel : MonoBehaviour {
        public static InGamePanel Instance => instance ??= FindObjectOfType<InGamePanel>();
        private static InGamePanel instance;


        private void Awake() {
            UIController.OnProgressChanged += UpdateProgressUI;
        }
        
        
        private void UpdateProgressUI(float progress) {
            Debug.Log($"Progress: {progress}");
        }
        
        private void OnDestroy() {
            UIController.OnProgressChanged -= UpdateProgressUI;
        }
        
    }
}