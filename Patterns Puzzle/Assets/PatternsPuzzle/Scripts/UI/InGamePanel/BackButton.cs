using System;
using GameControllers;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class BackButton : MonoBehaviour {
        private void Awake() {
            GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked() {
            // SaveController.Instance.SavePuzzle();
            PuzzleController.Instance.CurrentPuzzle.Unload();
            GameStateController.Instance.CurrentGameState = GameState.PuzzleSelectionPanel;
        }
    }
}