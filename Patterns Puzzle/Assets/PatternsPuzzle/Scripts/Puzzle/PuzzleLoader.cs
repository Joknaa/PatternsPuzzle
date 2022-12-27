using System;
using GameControllers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSystem {
    public class PuzzleLoader : MonoBehaviour {
        public string puzzleName;
        
        private Texture2D _puzzleOriginalImage;


        private void Awake() {
            _puzzleOriginalImage = GetComponent<Image>().mainTexture as Texture2D;
            GetComponent<Button>().onClick.AddListener(OnPuzzleClick);
        }

        private void OnPuzzleClick() {
            GameStateController.Instance.CurrentGameState = GameState.Playing;
            PuzzleController.Instance.GenerateNewPuzzle(puzzleName, _puzzleOriginalImage);
            SaveController.Instance.LoadPuzzle(PuzzleController.Instance.CurrentPuzzle);
        }
    }
}