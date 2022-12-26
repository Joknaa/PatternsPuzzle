using System;
using GameControllers;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class PuzzleSelectionButton : MonoBehaviour {
        [SerializeField] private Button _button;


        private void Awake() {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked() {
            print("Load Level");
            GameStateController.Instance.CurrentGameState = GameState.Playing;
            // PuzzleController.Instance.LoadPuzzle();
            
        }
    }
}