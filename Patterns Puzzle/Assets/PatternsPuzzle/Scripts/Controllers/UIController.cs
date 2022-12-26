using System;
using UnityEngine;

namespace GameControllers {
    public class UIController : MonoBehaviour {
        public static UIController Instance => instance ??= FindObjectOfType<UIController>();
        private static UIController instance;
        public static Action<int> OnProgressChanged;
        
        public GameObject inGamePanel;
        public GameObject puzzleSelectionPanel;


        private void Awake() {
            GameStateController.Instance.OnGameStateChanged += UpdateUI;
            UpdateUI();
        }


        public void UpdateProgress(int progress) => OnProgressChanged?.Invoke(progress);

        private void UpdateUI() {
            var currentState = GameStateController.Instance.CurrentGameState;

            switch (currentState) {
                case GameState.PuzzleSelectionPanel:
                    Display(puzzleSelectionPanel_: true);
                    break;
                case GameState.Playing:
                    Display(inGamePanel_: true);
                    break;
            }
        }

        private void Display(bool puzzleSelectionPanel_ = false, bool inGamePanel_ = false) {
            puzzleSelectionPanel.SetActive(puzzleSelectionPanel_);
            inGamePanel.SetActive(inGamePanel_);
        }
        
        private void OnDestroy() {
            GameStateController.Instance.OnGameStateChanged -= UpdateUI;
        }
    }
}