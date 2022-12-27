using UnityEngine;

namespace GameControllers {
    public class SaveController : MonoBehaviour {
        public static SaveController Instance => instance ??= FindObjectOfType<SaveController>();
        private static SaveController instance;


        public void SaveCurrentPuzzleState() {
            var currentPuzzle = PuzzleController.Instance.CurrentPuzzle;
            
        }
    }
}